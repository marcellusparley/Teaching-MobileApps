using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App2_Camera
{
    public static class BitmapHelpers
    {
        public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }

        //Removes red from the bitmap and returns a copy
        public static Bitmap RemoveRed(Bitmap b)
        {
            Android.Graphics.Bitmap copyBitmap =
                b.Copy(Android.Graphics.Bitmap.Config.Argb8888, true);
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    int p = b.GetPixel(i, j);
                    Android.Graphics.Color c = new Android.Graphics.Color(p);
                    c.R = 0;
                    copyBitmap.SetPixel(i, j, c);
                }
            }

            return copyBitmap;
        }

        // Removes blue from the bitmap and returns a copy
        public static Bitmap RemoveBlue(Bitmap b)
        {
            Bitmap copyBitmap = b.Copy(Bitmap.Config.Argb8888, true);
            for (int i=0; i < b.Width; i++)
                for (int j=0; j < b.Height; j++)
                {
                    int p = b.GetPixel(i, j);
                    Color c = new Color(p);
                    c.B = 0;
                    copyBitmap.SetPixel(i, j, c);
                }

            return copyBitmap;
        }

        // Removes green from the bitmap and returns a copy
        public static Bitmap RemoveGreen(Bitmap b)
        {
            Bitmap copyBitmap = b.Copy(Bitmap.Config.Argb8888, true);
            for (int i = 0; i < b.Width; i++)
                for (int j = 0; j < b.Height; j++)
                {
                    int p = b.GetPixel(i, j);
                    Color c = new Color(p);
                    c.G = 0;
                    copyBitmap.SetPixel(i, j, c);
                }

            return copyBitmap;
        }

        // Negates red from the bitmap and returns a copy. Negate is achieved by
        // subtracting the current value from the max of 255
        public static Bitmap NegateRed(Bitmap b)
        {
            Bitmap copyBitmap = b.Copy(Bitmap.Config.Argb8888, true);
            for (int i = 0; i < b.Width; i++)
                for (int j = 0; j < b.Height; j++)
                {
                    int p = b.GetPixel(i, j);
                    Color c = new Color(p);
                    c.R = 255 - c.R;
                    copyBitmap.SetPixel(i, j, c);
                }

            return copyBitmap;
        }

        // Negates blue from the bitmap and returns a copy. Negate is achieved by
        // subtracting the current value from the max of 255
        public static Bitmap NegateBlue(Bitmap b)
        {
            Bitmap copyBitmap = b.Copy(Bitmap.Config.Argb8888, true);
            for (int i = 0; i < b.Width; i++)
                for (int j = 0; j < b.Height; j++)
                {
                    int p = b.GetPixel(i, j);
                    Color c = new Color(p);
                    c.B = 255 - c.B;
                    copyBitmap.SetPixel(i, j, c);
                }

            return copyBitmap;
        }

        // Negates green from the bitmap and returns a copy. Negate is achieved by
        // subtracting the current value from the max of 255
        public static Bitmap NegateGreen(Bitmap b)
        {
            Bitmap copyBitmap = b.Copy(Bitmap.Config.Argb8888, true);
            for (int i = 0; i < b.Width; i++)
                for (int j = 0; j < b.Height; j++)
                {
                    int p = b.GetPixel(i, j);
                    Color c = new Color(p);
                    c.G = 255 - c.G;
                    copyBitmap.SetPixel(i, j, c);
                }

            return copyBitmap;
        }
    }
}