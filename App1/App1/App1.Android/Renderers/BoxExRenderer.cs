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

                    var xOnBox = e.Event.GetX();
                    var yOnBox = e.Event.GetY();
                    if (xOnBox < ScaleChangeWidth)
                        _scaleChangeDirections |= Directions.Left;
                    else if (xOnBox - ScaleChangeWidth > box.Width * 2)
                        _scaleChangeDirections |= Directions.Right;
                    if (yOnBox < ScaleChangeWidth)
                        _scaleChangeDirections |= Directions.Top;
                    else if (yOnBox - ScaleChangeWidth > box.Width * 2)
                        _scaleChangeDirections |= Directions.Bottom;

                    _downRect = box.ViewModel.GetRect();
                    var s1 = ScaleX;
                    var s2 = ScaleXs;
                    var s3 = ScaleY;
                    var s4 = ScaleYs;
                    System.Diagnostics.Debug.WriteLine("   xxx");
                    System.Diagnostics.Debug.WriteLine($"box x:{box.ViewModel.X}, y:{box.ViewModel.Y}, width:{box.ViewModel.Width}, height:{box.ViewModel.Height}");
                    System.Diagnostics.Debug.WriteLine($"frame x:{((Frame)box.Parent).X}, y:{((Frame)box.Parent).Y}, width:{((Frame)box.Parent).Width}, height:{((Frame)box.Parent).Height}");
                    System.Diagnostics.Debug.WriteLine($"touch x:{e.Event.RawX}, y:{e.Event.RawY}");
                    System.Diagnostics.Debug.WriteLine($"touch x:{e.Event.GetX()}, y:{e.Event.GetY()}");
                    break;
                case MotionEventActions.Move:
                    var x = (e.Event.RawX - _prevX) / 3;
                    var y = (e.Event.RawY - _prevY) / 3;
                    if(!_scaleChangeDirections.HasFlag(Directions.Right))
                    box.ViewModel.X += x;
                    if (!_scaleChangeDirections.HasFlag(Directions.Bottom))
                        box.ViewModel.Y += y;
                    if (_scaleChangeDirections.HasFlag(Directions.Left) || _scaleChangeDirections.HasFlag(Directions.Right))
                        box.ViewModel.Width += x;
                    if (_scaleChangeDirections.HasFlag(Directions.Top) || _scaleChangeDirections.HasFlag(Directions.Bottom))
                        box.ViewModel.Height += y;
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
        }
    }
}