/* Marcellus Parley
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
using Android.Util;
using Android.Views;
using Android.Widget;

/* Custom view that will allow for multi-touch drawing
 * Mostly taken from this tutorial with a few names changed for my sanity
 * https://docs.microsoft.com/en-us/xamarin/android/app-fundamentals/touch/touch-tracking
 * */
namespace pa3_vision
{
    [Register("pa3_vision.DrawingCanvasView")]
    public class DrawingCanvasView : View
    {
        //Two collections for holding the PaintedPaths with their ids to allow
        //for multi-touch
        Dictionary<int, PaintedPath> currentPaths = new Dictionary<int, PaintedPath>();
        List<PaintedPath> completedPaths = new List<PaintedPath>();

        Paint paint = new Paint();

        public DrawingCanvasView(Context context) : base(context)
        {
            Initialize();
        }

        public DrawingCanvasView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public DrawingCanvasView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            DrawingCacheEnabled = true;
        }

        public Color StrokeColor { set; get; } = Color.Black;

        public float StrokeWidth { set; get; } = 2;

        public void ClearAll()
        {
            DrawingCacheEnabled = false;
            completedPaths.Clear();
            Invalidate();
            DrawingCacheEnabled = true;
        }

        public Bitmap getImage()
        {
            Bitmap bm = GetDrawingCache(false).Copy(Bitmap.Config.Argb4444, false);
            return bm;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            // get pointer index and it's id to track the finger
            int pointerIndex = e.ActionIndex;
            int id = e.GetPointerId(pointerIndex);

            switch (e.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:

                    //create Path, set the initial point, and store in dict
                    PaintedPath p = new PaintedPath
                    {
                        PathColor = StrokeColor,
                        PathStrokeWidth = StrokeWidth
                    };
                    p.PathLine.MoveTo(e.GetX(pointerIndex), e.GetY(pointerIndex));
                    currentPaths.Add(id, p);
                    break;

                case MotionEventActions.Move:

                    //extend the lines for multiple moving pointers
                    for (pointerIndex = 0; pointerIndex < e.PointerCount; pointerIndex++)
                    {
                        id = e.GetPointerId(pointerIndex);
                        currentPaths[id].PathLine.LineTo(e.GetX(pointerIndex),
                            e.GetY(pointerIndex));
                    }
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:

                    //extend line to last location
                    currentPaths[id].PathLine.LineTo(e.GetX(pointerIndex),
                        e.GetY(pointerIndex));

                    // "finish" the path
                    completedPaths.Add(currentPaths[id]);
                    currentPaths.Remove(id);
                    break;

                case MotionEventActions.Cancel:
                    currentPaths.Remove(id);
                    break;
            }

            //Apparently this will update the view
            Invalidate();

            return true;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            //Clear canvas - draw white background
            paint.SetStyle(Paint.Style.Fill);
            paint.Color = Color.White;
            canvas.DrawPaint(paint);

            //Set the paint for drawing the paths
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeCap = Paint.Cap.Round;
            paint.StrokeJoin = Paint.Join.Round;

            //Draw completed paths first - below the current paths
            foreach (PaintedPath p in completedPaths)
            {
                paint.Color = p.PathColor;
                paint.StrokeWidth = p.PathStrokeWidth;
                canvas.DrawPath(p.PathLine, paint);
            }

            //Finally draw the current in progress paths on top
            foreach (PaintedPath p in currentPaths.Values)
            {
                paint.Color = p.PathColor;
                paint.StrokeWidth = p.PathStrokeWidth;
                canvas.DrawPath(p.PathLine, paint);
            }

        }

    }
}