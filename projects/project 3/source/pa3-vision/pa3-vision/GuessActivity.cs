/* Marcellus Parley
 * CS 480 - Mobile Apps
 * Assignment 3 - Google Vision Api
 * 02/27/2018
 * */

using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Collections.Generic;


// This is the second screen/activity where in the App gives it's best guess for the image
// and the user decides if it's correct or not. If so it goes to SuccessActivity if not
// WrongActivity

namespace pa3_vision
{
    [Activity(Label = "GuessActivity")]
    public class GuessActivity : Activity
    {
        // I have the result be a class variable so I can access it during the button presses
        private Google.Apis.Vision.v1.Data.BatchAnnotateImagesResponse _apiResult;

        // The dictionary is public and static because I need to access it across activities and you
        // cannot bundle dictionaries. I didn't want to bundle two separate arrays and attatch them
        // across two intents/activits
        public static Dictionary<string, float> resultDict = new Dictionary<string, float>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = "Hmm...";

            // Here I clear the dictionary so using the back button works to travers from later
            // activities
            resultDict.Clear();
            SetContentView(Resource.Layout.Guess);

            TextView guess = FindViewById<TextView>(Resource.Id.guessText);
            Button correctButton = FindViewById<Button>(Resource.Id.guessCorrectButton);
            Button incorrectButton = FindViewById<Button>(Resource.Id.guessIncorrectButton);
            
            correctButton.Click += GuessSuccess;
            incorrectButton.Click += GuessFailure;

            // Display in ImageView. We will resize the bitmap to fit the display.
            // Loading the full sized image will consume too much memory
            // and cause the application to crash.
            ImageView imageView = FindViewById<ImageView>(Resource.Id.takenPictureImageView);
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = Resources.DisplayMetrics.WidthPixels; // imageView.Height;

            //AC: workaround for not passing actual files
            //Android.Graphics.Bitmap bitmap = (Android.Graphics.Bitmap)Intent.Extras.Get("data");
            
            //load picture from file
            Android.Graphics.Bitmap bitmap = MainActivity._file.Path.LoadAndResizeBitmap(width, height);

            //convert bitmap into stream to be sent to Google API
            string bitmapString = "";
            using (var stream = new System.IO.MemoryStream())
            {
                bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, stream);

                var bytes = stream.ToArray();
                bitmapString = System.Convert.ToBase64String(bytes);
            }

            //credential is stored in "assets" folder
            string credPath = "google_api.json";
            Google.Apis.Auth.OAuth2.GoogleCredential cred;

            //Load credentials into object form
            using (var stream = Assets.Open(credPath))
            {
                cred = Google.Apis.Auth.OAuth2.GoogleCredential.FromStream(stream);
            }
            cred = cred.CreateScoped(Google.Apis.Vision.v1.VisionService.Scope.CloudPlatform);

            // By default, the library client will authenticate
            // using the service account file (created in the Google Developers
            // Console) specified by the GOOGLE_APPLICATION_CREDENTIALS
            // environment variable. We are specifying our own credentials via json file.
            var client = new Google.Apis.Vision.v1.VisionService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                ApplicationName = "cs480-mobileapps-pa3-msp261",
                HttpClientInitializer = cred
            });

            //set up request
            var request = new Google.Apis.Vision.v1.Data.AnnotateImageRequest();
            request.Image = new Google.Apis.Vision.v1.Data.Image();
            request.Image.Content = bitmapString;

            //tell google that we want to perform label detection
            request.Features = new List<Google.Apis.Vision.v1.Data.Feature>();
            request.Features.Add(new Google.Apis.Vision.v1.Data.Feature() { Type = "LABEL_DETECTION" });
            var batch = new Google.Apis.Vision.v1.Data.BatchAnnotateImagesRequest();
            batch.Requests = new List<Google.Apis.Vision.v1.Data.AnnotateImageRequest>();
            batch.Requests.Add(request);

            //send request.  Note that I'm calling execute() here, but you might want to use
            //ExecuteAsync instead
            _apiResult = client.Images.Annotate(batch).Execute();
            

            if (bitmap != null)
            {
                imageView.SetImageBitmap(bitmap);
                imageView.Visibility = Android.Views.ViewStates.Visible;
                bitmap = null;

                string topResponse = _apiResult.Responses[0].LabelAnnotations[0].Description;
                guess.Text = "Is this a " + topResponse + "?";
            }
            

            // Dispose of the Java side bitmap.
            System.GC.Collect();
        }
        
        private void GuessSuccess(object sender, EventArgs e)
        {
            var SuccessIntent = new Intent(this, typeof(SuccessActivity));
            StartActivity(SuccessIntent);
        }
        
        private void GuessFailure(object sender, EventArgs e)
        {
            var FailureIntent = new Intent(this, typeof(WrongActivity));
            
            foreach (var label in _apiResult.Responses[0].LabelAnnotations)
            {
                resultDict.Add(label.Description, (float)label.Score);
            }

            StartActivity(FailureIntent);
        }
    }
}
