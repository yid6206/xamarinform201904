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
        public bool Capture { set; get; }
        public Action<Element, TouchActionEventArgs> OnTouchAction { get; set; }
        public void OnTouchEvent(Element obj, TouchActionEventArgs args)
        {
            OnTouchAction?.Invoke(obj, args);
        }
    }
    public enum TouchActionType
    {
        Entered,
        Pressed,
        Moved,
        Released,
        Exited,
        Cancelled
    }
    public class TouchActionEventArgs : EventArgs
    {
        public TouchActionEventArgs(int id, TouchActionType type, Point point, bool isInContact)
        {
            Type = type;
            Point = point;
            IsInContact = isInContact;
        }
        public long Id { get; private set; }
        public TouchActionType Type { get; private set; }
        public Point Point { get; private set; }
        public bool IsInContact { get; private set; }
    }
}
