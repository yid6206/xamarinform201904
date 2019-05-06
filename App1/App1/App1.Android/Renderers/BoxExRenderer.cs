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

[assembly: ExportRenderer(typeof(BoxViewEx), typeof(BoxExRenderer))]
namespace App1.Droid.Renderers
{
    public class BoxExRenderer : BoxRenderer
    {
        private const double ScaleChangeWidth = 5;
        private float _prevX;
        private float _prevY;
        private Xamarin.Forms.Point[] _prevOnBoxPoints = new Xamarin.Forms.Point[0];
        private Directions _scaleChangeDirections = Directions.None;
        private Rectangle _downRect;

        public BoxExRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);
            this.Touch += BoxExRenderer_Touch;
        }

        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);
            //var box = (BoxViewEx)Element;
            //using (var paint = new Paint())
            //{
            //    var shadowSize = box.ShadowSize;
            //    var blur = shadowSize;
            //    var radius = box.Radius;

            //    paint.AntiAlias = true;

            //    // 影の描画（1）
            //    paint.Color = (Xamarin.Forms.Color.FromRgba(0, 0, 0, 112)).ToAndroid();
            //    paint.SetMaskFilter(new BlurMaskFilter(blur, BlurMaskFilter.Blur.Normal));
            //    var rectangle = new RectF(shadowSize, shadowSize, Width - shadowSize, Height - shadowSize);
            //    canvas.DrawRoundRect(rectangle, radius, radius, paint);

            //    // 本体の描画（2）
            //    paint.Color = box.Color.ToAndroid();
            //    paint.SetMaskFilter(null);
            //    rectangle = new RectF(0, 0, Width - shadowSize * 2, Height - shadowSize * 2);
            //    canvas.DrawRoundRect(rectangle, radius, radius, paint);
            //}
        }

        private void BoxExRenderer_Touch(object sender, TouchEventArgs e)
        {
            var box = (BoxViewEx)Element;
            var area = (BoxAreaLayout)box.Parent.Parent;
            var scale = area.Scale;
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    box.GetParent<BoxAreaLayout>().ResetAllSelect();
                    box.Selected = true;
                    break;
                case MotionEventActions.Move:
                    var isScaleMode = e.Event.PointerCount == 2 && _prevOnBoxPoints.Length == 2;
                    if (isScaleMode)
                    {
                        var prevRect = CreateRectangle(_prevOnBoxPoints[0].X, _prevOnBoxPoints[0].Y, _prevOnBoxPoints[1].X, _prevOnBoxPoints[1].Y);
                        var newRect = CreateRectangle(e.Event.GetX(0), e.Event.GetY(0), e.Event.GetX(1), e.Event.GetY(1));
                        box.ViewModel.X += newRect.X - prevRect.X;
                        box.ViewModel.Y += newRect.Y - prevRect.Y;
                        box.ViewModel.Width += newRect.Width - prevRect.Width;
                        box.ViewModel.Height += newRect.Height - prevRect.Height;
                    }
                    else
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
                    _scaleChangeDirections = Directions.None;
                    UndoManager.Push(() =>
                    {
                        box.ViewModel.X = _downRect.X;
                        box.ViewModel.Y = _downRect.Y;
                        box.ViewModel.Width = _downRect.Width;
                        box.ViewModel.Height = _downRect.Height;
                        box.UpdateLocationAndSize();
                    });
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