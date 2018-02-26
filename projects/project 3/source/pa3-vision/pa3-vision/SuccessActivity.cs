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
    [Activity(Label = "WrongActivity")]
    public class SuccessActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Success);
            
            Button returnToStart = FindViewById<Button>(Resource.Id.returnToStartButton);
            Button backToGuess = FindViewById<Button>(Resource.Id.backToGuessButton);
            
            returnToStart += ReturnStart;
            backToGuess += GoBack;
        }
        
        private void GoBack(object sender, EventArgs e)
        {
            Finish();
        }
        
        private void returnToStart(object sender, EventArgs e)
        {
            
        }
    }
}