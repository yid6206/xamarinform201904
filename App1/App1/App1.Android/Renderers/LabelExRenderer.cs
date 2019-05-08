using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using App1;
using App1.Droid;
using App1.Droid.Renderers;
using App1.Models;
using App1.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LabelEx), typeof(LabelExRenderer))]
namespace App1.Droid.Renderers
{
    public class LabelExRenderer : LabelRenderer
    {
        private const double ScaleChangeWidth = 5;
        private float _prevX;
        private float _prevY;
        private Xamarin.Forms.Point[] _prevOnBoxPoints = new Xamarin.Forms.Point[0];
        private Rectangle _downRect;

        public LabelExRenderer(Context context)
            : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            this.Touch += LabelExRenderer_Touch; ;
        }

        private void LabelExRenderer_Touch(object sender, TouchEventArgs e)
        {
            var box = (LabelEx)Element;
            var area = (BoxAreaLayout)box.Parent.Parent;
            var scale = area.Scale;
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    _downRect = box.ViewModel.GetRect();
                    break;
                case MotionEventActions.Move:
                    var isScaleMode = e.Event.PointerCount == 2 && _prevOnBoxPoints.Length == 2;
                    if (isScaleMode)
                    {
                        var prevRect = CreateRectangle(_prevOnBoxPoints[0].X, _prevOnBoxPoints[0].Y, _prevOnBoxPoints[1].X, _prevOnBoxPoints[1].Y);
                        var newRect = CreateRectangle(e.Event.GetX(0), e.Event.GetY(0), e.Event.GetX(1), e.Event.GetY(1));
                        box.ViewModel.X += (newRect.X - prevRect.X) / 3;
                        box.ViewModel.Y += (newRect.Y - prevRect.Y) / 3;
                        box.ViewModel.Size += (newRect.Width - prevRect.Width) / 3;
                    }
                    else if (_prevOnBoxPoints.Length == 1)
                    {
                        var x = (e.Event.RawX - _prevX) / 3;
                        var y = (e.Event.RawY - _prevY) / 3;
                        box.ViewModel.X += x;
                        box.ViewModel.Y += y;
                    }
                    box.UpdateLocationAndSize();
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    var upper = 20;
                    box.ViewModel.X = ((int)(box.ViewModel.X / upper + 0.5)) * upper;
                    box.ViewModel.Y = ((int)(box.ViewModel.Y / upper + 0.5)) * upper;
                    box.ViewModel.Size = ((int)(box.ViewModel.Size / upper + 0.5)) * upper;
                    box.UpdateLocationAndSize();
                    var rect = box.ViewModel.GetRect();
                    if (!rect.Equals(_downRect))
                    {
                        UndoManager.Push(() =>
                        {
                            box.ViewModel.X = _downRect.X;
                            box.ViewModel.Y = _downRect.Y;
                            box.ViewModel.Size = _downRect.Width;
                            box.UpdateLocationAndSize();
                        });
                    }
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("touch " + e.Event.Action.ToString());
                    break;
            }
            _prevX = e.Event.RawX;
            _prevY = e.Event.RawY;
            _prevOnBoxPoints = Enumerable.Range(0, e.Event.PointerCount).Select(q => new Xamarin.Forms.Point(e.Event.GetX(q), e.Event.GetY(q))).ToArray();
        }

        private Rectangle CreateRectangle(double x1, double y1, double x2, double y2)
        {
            return new Rectangle(
                Math.Min(x1, x2),
                Math.Min(y1, y2),
                Math.Abs(x1 - x2),
                Math.Abs(y1 - y2));
        }
    }
}