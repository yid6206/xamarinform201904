using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace App1.Views
{
    public class LabelEx : Label
    {
        public LabelEx()
        {
            HorizontalTextAlignment = TextAlignment.Center;
            VerticalTextAlignment = TextAlignment.Center;
            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;
      }

        public LabelViewModel ViewModel => (LabelViewModel)BindingContext;

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (ViewModel == null)
                return;
            UpdateLocationAndSize();
            Text = ViewModel.Text;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (ViewModel == null)
                return;
            switch (propertyName)
            {
                case nameof(LabelViewModel.Size):
                    UpdateLocationAndSize();
                    break;
                case nameof(LabelViewModel.Text):
                    Text = ViewModel.Text;
                    break;
            }
        }

        public void UpdateLocationAndSize()
        {
            ((LabelFrame)Parent).UpdateLocationAndSize();
            WidthRequest = ViewModel.Size;
            HeightRequest = ViewModel.Size;
        }
    }
}
