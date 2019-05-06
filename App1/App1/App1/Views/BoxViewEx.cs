using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Views
{
    public class BoxViewEx : BoxView
    {
        private bool _selected;

        public BoxViewEx()
        {
            BackgroundColor = Color.Red;
        }

        public BoxViewModel ViewModel => (BoxViewModel)BindingContext;
        public int Radius { get; set; } = 10;     // 角丸のサイズ
        public int ShadowSize { get; set; } = 5; // 影の幅

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

        public void UpdateLocationAndSize()
        {
            ((BoxFrame)Parent).UpdateLocationAndSize();
        }

        private void VisibleSelectionFrame(bool vibible)
        {
            var frame = this.GetParent<Frame>();
            frame.Padding = vibible ? new Thickness(3) : new Thickness();
        }
    }
}
