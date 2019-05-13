using SeatMaker.Helper;
using SeatMaker.Models;
using SeatMaker.Models.Figures;
using SeatMaker.Views.Touch;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeatMaker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSeatPage : ContentPage
    {
        private SKBitmap _bitmap;

        public CreateSeatPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _bitmap = SerializationHelper.LoadImage();
            canvasView.InvalidateSurface();
        }

        private async void UpdateSeat_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new UpdateSeatPage()));
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (_bitmap != null)
                e.Surface.Canvas.DrawBitmap(_bitmap, 0, 0);
        }
    }
}