using SeatMaker.Helper;
using SeatMaker.Interface;
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
        private INativeService _nativeService = null;

        public CreateSeatPage(INativeService nativeService)
        {
            InitializeComponent();
            _nativeService = nativeService;
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
            e.Surface.Canvas.Clear();
            if (_bitmap != null)
                e.Surface.Canvas.DrawBitmap(_bitmap, 0, 0);
        }

        private async void SendToTeams_Clicked(object sender, EventArgs e)
        {
            var url = "https://outlook.office.com/webhook/494fc077-7de1-46a5-9729-1f46db7d8e48@a5d1e37f-5f76-475e-8278-441b98d2aae5/IncomingWebhook/a1691fcc9fa74124840a50692fe0cf8e/6c96ace7-adb0-43b6-8a50-6bef721f3412";

            _nativeService.ShowLoading();
            await SerializationHelper.SendImageToTeamsAsync(url);
            _nativeService.HideLoading();
        }
    }
}