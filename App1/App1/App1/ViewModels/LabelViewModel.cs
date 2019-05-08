using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class LabelViewModel : BaseViewModel
    {
        public LabelViewModel()
        {
            X = 150;
            Y = 150;
            Size = 40;
        }

        public double X { get => Get(nameof(X)); set => Set(nameof(X), value); }
        public double Y { get => Get(nameof(Y)); set => Set(nameof(Y), value); }
        public double Size { get => Get(nameof(Size)); set => Set(nameof(Size), value); }
        public string Text { get => GetString(nameof(Y)); set => SetString(nameof(Y), value); }

        public Rectangle GetRect() => new Rectangle(X, Y, Size, Size);

        // MemberwiseCloneを使うとイベントもコピーされておかしくなる
        public LabelViewModel Clone() => new LabelViewModel { X = X, Y = Y, Size = Size };
    }
}
