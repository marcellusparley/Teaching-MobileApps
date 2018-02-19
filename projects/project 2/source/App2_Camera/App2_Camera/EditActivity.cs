//Marcellus Parley
//msp261
//480 mobile apps - pa2
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
using Android.Graphics.Drawables;
using System.IO;
using Android.Provider;

namespace App2_Camera
{
    [Activity(Label = "EditActivity")]
    public class EditActivity : Activity
    {
        public static Android.Graphics.Bitmap _b;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Edit);

            //Android.Graphics.Bitmap bitmap = null;
            //Android.Graphics.Bitmap scaled = null;

            //set up imageView
            ImageView imageView = FindViewById<ImageView>(Resource.Id.takenPictureImageView);
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = imageView.Height;

            //getting the bitmap from the intent
            int request = (int)Intent.Extras.Get("requestcode");
            if (request == 0)
            {
                _b = (Android.Graphics.Bitmap)Intent.Extras.Get("data");
                _b = Android.Graphics.Bitmap.CreateScaledBitmap(_b, 480, 640, true);
            }
            else
            {
                imageView.SetImageURI((Android.Net.Uri)Intent.Extras.Get("imageuri"));
                _b = ((BitmapDrawable)imageView.Drawable).Bitmap;
                _b = Android.Graphics.Bitmap.CreateScaledBitmap(_b, 480, 640, true);

            }
            // Getting button and effects spinner
            Button buttonApply = FindViewById<Button>(Resource.Id.applyButton);
            Button buttonSave = FindViewById<Button>(Resource.Id.buttonSave);
            Button buttonReturn = FindViewById<Button>(Resource.Id.buttonReturn);
            Spinner effectSelect = FindViewById<Spinner>(Resource.Id.effectSpinner);

            var adapter = ArrayAdapter.CreateFromResource(
                this, Resource.Array.effects_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            effectSelect.Adapter = adapter;

            buttonApply.Click += ApplyEffect;
            buttonSave.Click += SaveImage;
            buttonReturn.Click += GoBack;
            

            if (_b != null)
            {
                imageView.SetImageBitmap(_b);
                imageView.Visibility = Android.Views.ViewStates.Visible;
                //_b = null;
            }

            // Dispose of the Java side bitmap.
            System.GC.Collect();
        }

        private void SaveImage(object sender, EventArgs e)
        {
            using (var output = new System.IO.FileStream(MainActivity._dir + "/" + string.Format("myPhoto_{0}.jpg", System.Guid.NewGuid()),
                System.IO.FileMode.CreateNew))
            {
                _b.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 95, output);
            }
        }

        private void GoBack(object sender, EventArgs e)
        {
            _b = null;
            Finish();
        }

        private void ApplyEffect(object sender, EventArgs e)
        {
            ImageView imageView = FindViewById<ImageView>(Resource.Id.takenPictureImageView);
            Spinner spinner = FindViewById<Spinner>(Resource.Id.effectSpinner);
            string effect = spinner.SelectedItem.ToString();
            Android.Graphics.Bitmap copyBitmap = null;

            switch (effect)
            {
                case "Remove Red":
                    copyBitmap = BitmapHelpers.RemoveRed(_b);
                    break;
                case "Remove Green":
                    copyBitmap = BitmapHelpers.RemoveGreen(_b);
                    break;
                case "Remove Blue":
                    copyBitmap = BitmapHelpers.RemoveBlue(_b);
                    break;
                case "Negate Red":
                    copyBitmap = BitmapHelpers.NegateRed(_b);
                    break;
                case "Negate Green":
                    copyBitmap = BitmapHelpers.NegateGreen(_b);
                    break;
                case "Negate Blue":
                    copyBitmap = BitmapHelpers.NegateBlue(_b);
                    break;
                case "Grayscale":
                    copyBitmap = BitmapHelpers.Grayscale(_b);
                    break;
                case "High Contrast":
                    copyBitmap = BitmapHelpers.HighContrast(_b);
                    break;
                case "Add Noise":
                    copyBitmap = BitmapHelpers.AddNoise(_b);
                    break;
                default:
                    break;
            }
            
            if (copyBitmap != null)
            {
                imageView.SetImageBitmap(copyBitmap);
                imageView.Visibility = Android.Views.ViewStates.Visible;
                _b = copyBitmap;
                copyBitmap = null;
            }

            System.GC.Collect();
        }
    }
}