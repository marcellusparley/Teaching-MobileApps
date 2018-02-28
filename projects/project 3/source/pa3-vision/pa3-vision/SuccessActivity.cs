/* Marcellus Parley
 * CS 480 - Mobile Apps
 * Assignment 3 - Google Vision Api
 * 02/27/2018
 * */

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

// Simple page where app gloats at it's win. Offers a button to return to start.

namespace pa3_vision
{
    [Activity(Label = "SuccessActivity")]
    public class SuccessActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = "SUCCESS!";

            // Create your application here
            SetContentView(Resource.Layout.Success);
            
            Button returnToStart = FindViewById<Button>(Resource.Id.returnToStartButton);
            
            returnToStart.Click += ReturnStart;
        }
        
        private void ReturnStart(object sender, EventArgs e)
        {
            Intent toStart = new Intent(this, typeof(MainActivity));
            toStart.SetFlags(ActivityFlags.ClearTop);
            StartActivity(toStart);
        }
    }
}