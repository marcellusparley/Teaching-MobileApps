using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App2_Camera
{
    [Activity(Label = "EditActivity")]
    public class EditActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Edit);

            //set up imageView
            ImageView imageView = FindViewById<ImageView>(Resource.Id.takenPictureImageView);
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = imageView.Height;

            //getting the bitmap from the intent
            Android.Graphics.Bitmap bitmap = (Android.Graphics.Bitmap)Intent.Extras.Get("data");

            // Getting button and effects spinner
            //Button buttonApply = FindViewById<Button>(Resource.Id.applyButton);
            //Spinner effectSelect = FindViewById<Spinner>(Resource.Id.effectSpinner);

            Android.Graphics.Bitmap copyBitmap = BitmapHelpers.RemoveRed(bitmap);
            if (copyBitmap != null)
            {
                imageView.SetImageBitmap(copyBitmap);
                imageView.Visibility = Android.Views.ViewStates.Visible;
                bitmap = null;
                copyBitmap = null;
            }

            // Dispose of the Java side bitmap.
            System.GC.Collect();
        }
    }
}