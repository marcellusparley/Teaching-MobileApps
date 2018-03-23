﻿/* Marcellus Parley
 * CS 480 - Mobile Apps
 * Assignment 4 - Google Vision Api Revisited
 * 03/21/2018
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Text;

namespace pa3_vision
{
    [Activity(Label = "DrawActivity")]
    public class DrawActivity : Activity
    {
        DrawingCanvasView canvas;
        Bitmap drawing;
        Color strokeColor = Color.Black;
        private Google.Apis.Vision.v1.Data.BatchAnnotateImagesResponse _apiResult;
        public static Dictionary<string, float> drawDict = new Dictionary<string, float>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Draw);
            canvas = FindViewById<DrawingCanvasView>(Resource.Id.drawingCanvas);
            Button clearDraw = FindViewById<Button>(Resource.Id.buttonDrawClear);
            Button identify = FindViewById<Button>(Resource.Id.buttonIdentify);
            Button changeStroke = FindViewById<Button>(Resource.Id.buttonChangeStroke);
            Button changeColor = FindViewById<Button>(Resource.Id.buttonChangeColor);

            clearDraw.Click += ClearButtonPress;
            changeColor.Click += AlterColor;
            changeStroke.Click += ChangedStroke;
            identify.Click += IdWithVision;

        }

        private void IdWithVision(object sender, EventArgs e)
        {
            drawing = canvas.getImage();
            canvas.ClearAll();

            //convert bitmap into stream to be sent to Google API
            string bitmapString = "";
            using (var stream = new System.IO.MemoryStream())
            {
                drawing.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, stream);

                var bytes = stream.ToArray();
                bitmapString = System.Convert.ToBase64String(bytes);
            }

            //credential is stored in "assets" folder
            string credPath = "google_api.json";
            Google.Apis.Auth.OAuth2.GoogleCredential cred;

            //Load credentials into object form
            using (var stream = Assets.Open(credPath))
            {
                cred = Google.Apis.Auth.OAuth2.GoogleCredential.FromStream(stream);
            }
            cred = cred.CreateScoped(Google.Apis.Vision.v1.VisionService.Scope.CloudPlatform);

            // By default, the library client will authenticate
            // using the service account file (created in the Google Developers
            // Console) specified by the GOOGLE_APPLICATION_CREDENTIALS
            // environment variable. We are specifying our own credentials via json file.
            var client = new Google.Apis.Vision.v1.VisionService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                ApplicationName = "cs480-mobileapps-pa3-msp261",
                HttpClientInitializer = cred
            });

            //set up request
            var request = new Google.Apis.Vision.v1.Data.AnnotateImageRequest();
            request.Image = new Google.Apis.Vision.v1.Data.Image();
            request.Image.Content = bitmapString;

            //tell google that we want to perform label detection
            request.Features = new List<Google.Apis.Vision.v1.Data.Feature>();
            request.Features.Add(new Google.Apis.Vision.v1.Data.Feature() { Type = "LABEL_DETECTION" });
            var batch = new Google.Apis.Vision.v1.Data.BatchAnnotateImagesRequest();
            batch.Requests = new List<Google.Apis.Vision.v1.Data.AnnotateImageRequest>();
            batch.Requests.Add(request);

            //send request.  Note that I'm calling execute() here, but you might want to use
            //ExecuteAsync instead
            _apiResult = client.Images.Annotate(batch).Execute();

            foreach (var label in _apiResult.Responses[0].LabelAnnotations)
            {
                drawDict.Add(label.Description, (float)label.Score);
            }

            Intent DrawOutcome = new Intent(this, typeof(DrawOutcome));
            StartActivity(DrawOutcome);
        }

        private void ChangedStroke(object sender, EventArgs e)
        {
            EditText strokeText = FindViewById<EditText>(Resource.Id.editTextStroke);
            string str = strokeText.Text as string;
            canvas.StrokeWidth = float.Parse(str);
        }

        private void AlterColor(object sender, EventArgs e)
        {
            SeekBar redBar = FindViewById<SeekBar>(Resource.Id.seekBarRed);
            SeekBar greenBar = FindViewById<SeekBar>(Resource.Id.seekBarGreen);
            SeekBar blueBar = FindViewById<SeekBar>(Resource.Id.seekBarBlue);

            strokeColor.R = (byte) redBar.Progress;
            strokeColor.G = (byte) greenBar.Progress;
            strokeColor.B = (byte) blueBar.Progress;

            canvas.StrokeColor = strokeColor;
        }

        private void ClearButtonPress(object sender, EventArgs e)
        {
            canvas.ClearAll();
        }
    }
}