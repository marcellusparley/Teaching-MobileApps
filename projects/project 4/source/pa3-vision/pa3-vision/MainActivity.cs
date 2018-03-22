/* Marcellus Parley
 * CS 480 - Mobile Apps
 * Assignment 4 - Google Vision Api Revisited
 * 03/21/2018
 * */

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Provider;
using System;

// Most of this is taken straight from the example code I moved all the code 
// dealing with the image into the GuessActivty

namespace pa3_vision
{
    [Activity(Label = "Beat Google Vision", MainLauncher = true)]
    public class MainActivity : Activity
    {
        //Used to track the file we're manipulating between functions
        public static Java.IO.File _file;
        
        //Used to track the directory that we'll be writing to between functions
        public static Java.IO.File _dir;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            //see https://forums.xamarin.com/discussion/97273/launch-camera-activity-with-saving-file-in-external-storage-crashes-the-app
            //I'm not sure if doing the two lines below is a great idea, but it does seem to fix passing files around
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            
            base.OnCreate(savedInstanceState);
            this.Title = "Try to Stump Google Vision";

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            if (IsThereAnAppToTakePictures() == true)
            {
                CreateDirectoryForPictures();
                FindViewById<Button>(Resource.Id.launchCameraButton).Click += TakePicture;
            }

            FindViewById<Button>(Resource.Id.buttonOpenCanvas).Click += ToDrawingMode;
        }

        private void ToDrawingMode(object sender, EventArgs e)
        {
            Intent drawIntent = new Intent(this, typeof(DrawActivity));
            StartActivity(drawIntent);
        }

        /// <summary>
        /// Apparently, some android devices do not have a camera.  To guard against this,
        /// we need to make sure that we can take pictures before we actually try to take a picture.
        /// </summary>
        /// <returns></returns>
        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities
                (intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }
        
        /// <summary>
        /// Creates a directory on the phone that we can place our images
        /// </summary>
        private void CreateDirectoryForPictures()
        {
            _dir = new Java.IO.File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "GoogleApiVision");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }


        private void TakePicture(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            _file = new Java.IO.File(_dir, string.Format("myPhoto_{0}.jpg", System.Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
            StartActivityForResult(intent, 0);
        }

        // <summary>
        // Called automatically whenever an activity finishes
        // </summary>
        // <param name = "requestCode" ></ param >
        // < param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //Creates intent to start GuessActivity with, and starts it
            var GuessIntent = new Intent(this, typeof(GuessActivity));
            StartActivity(GuessIntent);

            // Dispose of the Java side bitmap.
            System.GC.Collect();
        }
    }
}