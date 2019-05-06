using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class BoxViewModel : BaseViewModel
    {
        public BoxViewModel()
        {
            X = 150;
            Y = 150;
            Width = 50;
            Height = 50;
        }

        public double X { get => Get(nameof(X)); set => Set(nameof(X), value); }
        public double Y { get => Get(nameof(Y)); set => Set(nameof(Y), value); }
        public double Width { get => Get(nameof(Width)); set => Set(nameof(Width), value); }
        public double Height { get => Get(nameof(Height)); set => Set(nameof(Height), value); }

        public Rectangle GetRect() => new Rectangle(X, Y, Width, Height);
    }
}
