using App1.Models;
using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace App1.Views
{
    public class BoxFrame : Frame
    {
        public BoxFrame()
        {
            Padding = new Thickness();
            BorderColor = Color.Black;
            BackgroundColor = Color.Transparent;
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.Center;
            Box = new BoxViewEx();
            Content = Box;
        }

        public BoxViewEx Box { get; }
        public BoxViewModel ViewModel => (BoxViewModel)BindingContext;

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdateLocationAndSize();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(BoxViewModel.X):
                case nameof(BoxViewModel.Y):
                case nameof(BoxViewModel.Width):
                case nameof(BoxViewModel.Height):
                    UpdateLocationAndSize();
                    break;
            }
        }

        public void UpdateLocationAndSize()
        {
            Layout(ViewModel.GetRect());
            //Margin = new Thickness(ViewModel.X, ViewModel.Y, 0, 0);
            //WidthRequest = ViewModel.Width;
            //HeightRequest = ViewModel.Height;
        }
    }
}
