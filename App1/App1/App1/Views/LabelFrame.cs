using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace App1.Views
{
    public class LabelFrame : Frame
    {
        public LabelFrame()
        {
            Padding = new Thickness();
            BorderColor = Color.Black;
            BackgroundColor = Color.Transparent;
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.Center;
            CornerRadius = 50;
            Label = new LabelEx
            {
                Text = "1",
            };
            Content = Label;
        }

        public LabelEx Label { get; }
        public LabelViewModel ViewModel => (LabelViewModel)BindingContext;

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
                case nameof(LabelViewModel.X):
                case nameof(LabelViewModel.Y):
                case nameof(LabelViewModel.Size):
                    UpdateLocationAndSize();
                    break;
            }
        }

        public void UpdateLocationAndSize()
        {
            Label.FontSize = ViewModel.Size / 2;
            CornerRadius = (float)ViewModel.Size / 2;
            Layout(ViewModel.GetRect());
        }
    }
}