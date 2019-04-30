using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Views
{
    public class BoxViewEx : BoxView
    {
        private bool _selected;

        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (value != _selected)
                {
                    _selected = value;
                    VisibleSelectionFrame(value);
                }
            }
        }

        public void MoveToOffset(double x, double y)
        {
            var frame = this.GetParent<Frame>();
            var bounds = frame.Bounds;
            bounds.X += x;
            bounds.Y += y;
            frame.Layout(bounds);
        }

        private void VisibleSelectionFrame(bool vibible)
        {
            var frame = this.GetParent<Frame>();
            frame.Padding = vibible ? new Thickness(3) : new Thickness();
        }
    }
}
