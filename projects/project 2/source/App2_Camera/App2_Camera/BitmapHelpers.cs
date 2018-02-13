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
                    c.R = (byte)(255 - c.R);
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
                    c.B = (byte)(255 - c.B);
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
                    c.G = (byte)(255 - c.G);
                    copyBitmap.SetPixel(i, j, c);
                }

            return copyBitmap;
        }

        // Grayscale by taking average of red, green, and blue values and applying
        // that value to each of the colors
        public static Bitmap Grayscale(Bitmap b)
        {
            Bitmap copyBitmap = b.Copy(Bitmap.Config.Argb8888, true);
            for (int i = 0; i < b.Width; i++)
                for (int j = 0; j < b.Height; j++)
                {
                    int p = b.GetPixel(i, j);
                    Color c = new Color(p);
                    int avg = (c.G + c.R + c.B) / 3;
                    c.G = (byte)avg;
                    copyBitmap.SetPixel(i, j, c);
                }

            return copyBitmap;
        }

        // Applies a high contrast effect by maxing values above half of 255 and
        // flooring values below for each pixel
        public static Bitmap HighContrast(Bitmap b)
        {
            Bitmap copyBitmap = b.Copy(Bitmap.Config.Argb8888, true);
            for (int i = 0; i < b.Width; i++)
                for (int j = 0; j < b.Height; j++)
                {
                    int p = b.GetPixel(i, j);
                    Color c = new Color(p);

                    c.R = (byte)((c.R > 255 / 2) ? 255 : 0);
                    c.G = (byte)((c.G > 255 / 2) ? 255 : 0);
                    c.B = (byte)((c.B > 255 / 2) ? 255 : 0);

                    copyBitmap.SetPixel(i, j, c);
                }

            return copyBitmap;
        }

        // Adds Random noise to an image by randomly selecting a value between -10
        // and 10 and applies that value to each of the red, green, and blue values
        // of that pixel
        public static Bitmap AddNoise(Bitmap b)
        {
            Bitmap copyBitmap = b.Copy(Bitmap.Config.Argb8888, true);
            for (int i = 0; i < b.Width; i++)
                for (int j = 0; j < b.Height; j++)
                {
                    int p = b.GetPixel(i, j);
                    Color c = new Color(p);
                    Random rand = new Random();
                    int randVal = rand.Next(-10, 10);

                    int[] colors = { c.R + randVal, c.G + randVal, c.B + randVal };
                    for (int k = 0; k < 3; k++)
                    {
                        if (colors[k] > 255)
                            colors[k] = 255;
                        else if (colors[k] < 0)
                            colors[k] = 0;
                    }

                    c.R = (byte)colors[0];
                    c.G = (byte)colors[1];
                    c.B = (byte)colors[2];
                }

            return copyBitmap;
        }
    }
}