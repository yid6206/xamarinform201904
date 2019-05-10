using App1.Droid.Gestures;
using App1.Views;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Effects")]
[assembly: ExportEffect(typeof(TouchEffect), "TouchEffect")]
namespace App1.Droid.Gestures
{
    public class TouchEffect : PlatformEffect
    {
        Android.Views.View view;
        Element element;
        TEffect tEffect;

        static Dictionary<Android.Views.View, TouchEffect> list = new Dictionary<Android.Views.View, TouchEffect>();

        protected override void OnAttached()
        {
            view = Control == null ? Container : Control;
            element = Element;
            if (view != null)
            {
                view.Touch += OnTouch;
                list.Add(view, this);
                tEffect = (TEffect)element.Effects.FirstOrDefault(e => e is TEffect);
            }
        }

        protected override void OnDetached()
        {
            if (list.ContainsKey(view))
            {
                view.Touch -= OnTouch;
                list.Remove(view);
            }
        }

        private void OnTouch(object obj, Android.Views.View.TouchEventArgs ev)
        {
            int actID = ev.Event.ActionIndex;
            int pointID = ev.Event.GetPointerId(actID);
            float x = ev.Event.GetX(actID);
            float y = ev.Event.GetY(actID);

            System.Diagnostics.Debug.WriteLine($"◆◆◆ touch effect {ev.Event.Action.ToString()} {x}, {y}");

            switch (ev.Event.Action)
            {
                case Android.Views.MotionEventActions.Down:
                    tEffect?.OnTouchEvent(obj, new TouchEventArgs(TouchEventArgs.TouchEventType.Pressed, x, y));
                    break;
                case Android.Views.MotionEventActions.Move:
                    tEffect?.OnTouchEvent(obj, new TouchEventArgs(TouchEventArgs.TouchEventType.Moved, x, y));
                    break;
                case Android.Views.MotionEventActions.Up:
                    tEffect?.OnTouchEvent(obj, new TouchEventArgs(TouchEventArgs.TouchEventType.Entered, x, y));
                    break;
                case Android.Views.MotionEventActions.Cancel:
                    tEffect?.OnTouchEvent(obj, new TouchEventArgs(TouchEventArgs.TouchEventType.Cancelled, x, y));
                    break;
            }
        }
    }

}