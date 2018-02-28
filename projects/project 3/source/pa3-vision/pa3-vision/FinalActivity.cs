/* Marcellus Parley
 * CS 480 - Mobile Apps
 * Assignment 3 - Google Vision Api
 * 02/27/2018
 * */

using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

// This is the last activity. The app will either gloat over knowing that label
// or wallow in defeat. Offers a button to return to start.

namespace pa3_vision
{
    [Activity(Label = "FinalActivity")]
    public class FinalActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.FinalLayout);

            TextView message = FindViewById<TextView>(Resource.Id.finalText);
            Button playAgain = FindViewById<Button>(Resource.Id.playAgainButton);

            playAgain.Click += ReturnStart;

            string actualLabel = this.Intent.GetStringExtra("actualDescription").ToLower();
            string pre = "aeiou".Contains(actualLabel[0]) ? "an" : "a";
            float labelScore;
            
            if (GuessActivity.resultDict.TryGetValue(actualLabel, out labelScore))
            {
                this.Title = "YOU LOSE!";
                message.Text = String.Format(
                    "{0}? ... I knew it! I was {1:P2} sure that image was {2} {0}! HAH!",
                    actualLabel.ToUpper(), labelScore, pre 
                    );
            }
            else
            {
                this.Title = "Well played...";
                message.Text = "Wow, you stumped me... I had no idea that image was of "
                    + pre + " " + actualLabel.ToUpper() + ".";
            }
        }

        // This just clears the activity stack above MainActivity and returns there
        private void ReturnStart(object sender, EventArgs e)
        {
            Intent toStart = new Intent(this, typeof(MainActivity));
            toStart.SetFlags(ActivityFlags.ClearTop);
            StartActivity(toStart);
        }
    }
}