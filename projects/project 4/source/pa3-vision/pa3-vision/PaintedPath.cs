/* Marcellus Parley
 * CS 480 - Mobile Apps
 * Assignment 4 - Google Vision Api Revisited
 * 03/21/2018
 * */
using Android.Graphics;

/* Basic class for paths to hold information about their own color
 * and stroke width. Taken from the multi-touch example
 * https://docs.microsoft.com/en-us/xamarin/android/app-fundamentals/touch/touch-tracking
 * */

namespace pa3_vision
{
    class PaintedPath
    {
        public Path PathLine { private set; get; }
        public Color PathColor { set; get; }
        public float PathStrokeWidth { set; get; }

        public PaintedPath()
        {
            PathLine = new Path();
        }
    }
}