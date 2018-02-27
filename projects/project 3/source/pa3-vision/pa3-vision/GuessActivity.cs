using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Provider;
using System;
using System.Linq;
using System.Text;
using Android.Runtime;
using Android.Views;
using Android.Graphics.Drawables;
using System.IO;

namespace pa3_vision
{
    [Activity(Label = "GuessActivity")]
    public class GuessActivity : Activity
    {
        //private Google.Apis.Vision.v1.Data.BatchAnnotateImagesResponse _result;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
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
            var apiResult = client.Images.Annotate(batch).Execute();
            

            if (bitmap != null)
            {
                imageView.SetImageBitmap(bitmap);
                imageView.Visibility = Android.Views.ViewStates.Visible;
                bitmap = null;

                string topResponse = apiResult.Responses[0].LabelAnnotations[0].Description;
                guess.Text = "Is this a " + topResponse + "?";
                //_result = apiResult;
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
            //send responses along with intent
            //FailureIntent.PutExtra("apiresult", _result);
            StartActivity(FailureIntent);
        }
    }
}
