using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
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
        private float _prevX;
        private float _prevY;
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

        private void BoxExRenderer_Touch(object sender, TouchEventArgs e)
        {
            var box = (BoxViewEx)Element;
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    box.GetParent<BoxAreaLayout>().ResetAllSelect();
                    box.Selected = true;
                    _downRect = box.ViewModel.GetRect();
                    break;
                case MotionEventActions.Move:
                    var x = (e.Event.RawX - _prevX) / 3;
                    var y = (e.Event.RawY - _prevY) / 3;
                    box.ViewModel.X += x;
                    box.ViewModel.Y += y;
                    box.UpdateLocationAndSize();
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
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