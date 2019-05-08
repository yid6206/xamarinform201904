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

        public void UpdateLocationAndSize()
        {
            ((BoxFrame)Parent).UpdateLocationAndSize();
        }
    }
}
