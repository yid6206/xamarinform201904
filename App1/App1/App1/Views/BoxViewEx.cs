using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Views
{
    public class BoxViewEx : BoxView
    {
        public BoxViewEx()
        {
            BackgroundColor = Color.Transparent;
        }

        public BoxViewModel ViewModel => (BoxViewModel)BindingContext;
        public int Radius { get; set; } = 10;     // 角丸のサイズ
        public int ShadowSize { get; set; } = 5; // 影の幅

        public void UpdateLocationAndSize()
        {
            ((BoxFrame)Parent).UpdateLocationAndSize();
        }
    }
}
