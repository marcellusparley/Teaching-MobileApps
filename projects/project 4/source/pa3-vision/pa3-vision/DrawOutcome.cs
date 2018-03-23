/* Marcellus Parley
 * CS 480 - Mobile Apps
 * Assignment 4 - Google Vision Api Revisited
 * 03/21/2018
 * */
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

namespace pa3_vision
{
    [Activity(Label = "DrawOutcome")]
    public class DrawOutcome : Activity
    {
        Button returnToStart;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.DrawOutcome);

            Button submit = FindViewById<Button>(Resource.Id.buttonSubmitDrawing);
            returnToStart = FindViewById<Button>(Resource.Id.buttonReturnToStart);
            

            submit.Click += TryingToDraw;
            returnToStart.Click += ReturnStart;
        }

        private void ReturnStart(object sender, EventArgs e)
        {
            Intent toStart = new Intent(this, typeof(MainActivity));
            toStart.SetFlags(ActivityFlags.ClearTop);
            DrawActivity.drawDict.Clear();
            StartActivity(toStart);
        }

        private void TryingToDraw(object sender, EventArgs e)
        {
            EditText drawText = FindViewById<EditText>(Resource.Id.editTextDrawing);
            TextView outcomeText = FindViewById<TextView>(Resource.Id.textViewOutcome);

            if (drawText.Text.Length > 0)
            {
                string drawingDescription = drawText.Text.ToLower();
                float labelScore;
                if (DrawActivity.drawDict.TryGetValue(drawingDescription, out labelScore))
                {
                    outcomeText.Text = String.Format("You drew a {0}? ... Ah! " +
                        "I see it now! It looks about {1:P2} {0}! You are a regular Picasso!",
                        drawingDescription, labelScore);
                }
                else
                {
                    outcomeText.Text = String.Format("{0}? ... No way! I don't see it."
                        + " You're cerntainly no Picasso.", drawingDescription);
                }

                returnToStart.Visibility = ViewStates.Visible;

            }
            else
            {
                Toast.MakeText(this, "Hey! You need to say what it was!", ToastLength.Long).Show();
            }
        }
    }
}