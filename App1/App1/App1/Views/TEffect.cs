using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Views
{
    public class TEffect : RoutingEffect
    {
        public TEffect() : base("Effects.TouchEffect")
        {
        }
        public event TouchEventHundler OnTouch;
        public void OnTouchEvent(object obj, TouchEventArgs args)
        {
            OnTouch?.Invoke(obj, args);
        }
    }
    public class TouchEventArgs : EventArgs
    {
        public enum TouchEventType
        {
            Entered,
            Pressed,
            Moved,
            Released,
            Exited,
            Cancelled
        }
        public TouchEventArgs(TouchEventType type, double x, double y)
        {
            Type = type;
            X = x;
            Y = y;
        }
        public long Id { get; private set; }
        public TouchEventType Type { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
    }

    public delegate void TouchEventHundler(object obj, TouchEventArgs args);
}
