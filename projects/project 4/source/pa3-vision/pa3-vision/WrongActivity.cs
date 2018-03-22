/* Marcellus Parley
 * CS 480 - Mobile Apps
 * Assignment 4 - Google Vision Api Revisited
 * 03/21/2018
 * */

using System;
using Android.Widget;
using Android.App;
using Android.Content;
using Android.OS;


// This activity is when the user presses no at the app's guess
// The app asks for the actual name of the item in the image

namespace pa3_vision
{
    [Activity(Label = "WrongActivity")]
    public class WrongActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Title = "No?";

            // Create your application here
            SetContentView(Resource.Layout.Wrong);

            Button submit = FindViewById<Button>(Resource.Id.submitButton);
            submit.Click += SubmitActualImageDescription;

        }

        private void SubmitActualImageDescription(object sender, EventArgs e)
        {
            EditText ActualField = FindViewById<EditText>(Resource.Id.editTextImageActual);
            string actualDescription = ActualField.Text;

            if (actualDescription != "" && actualDescription != null)
            {
                var FinalIntent = new Intent(this, typeof(FinalActivity));
                FinalIntent.PutExtra("actualDescription", actualDescription);
                StartActivity(FinalIntent);
            }
            else
            {
                Toast.MakeText(this, "Hey! You need to say what it was!", ToastLength.Long).Show();
                
            }

        }
    }
}