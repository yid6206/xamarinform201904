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
using App1.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BoxSelection), typeof(BoxSelectionRenderer))]
namespace App1.Droid.Renderers
{
    public class BoxSelectionRenderer : BoxRenderer
    {
        private float _prevX;
        private float _prevY;

        public BoxSelectionRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);
            this.Touch += BoxSelectionRenderer_Touch;
        }

        private void BoxSelectionRenderer_Touch(object sender, TouchEventArgs e)
        {
            var box = (BoxViewEx)Element;
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    var parent = (BoxAreaLayout)Parent;
                    parent.ResetAllSelect();
                    box.Selected = true;
                    break;
                case MotionEventActions.Move:
                    var x = (e.Event.RawX - _prevX) / 3;
                    var y = (e.Event.RawY - _prevY) / 3;
                    box.MoveToOffset(x, y);
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
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