
using Android.App;
using SeatMaker.Interface;
using Xamarin.Forms;

namespace SeatMaker.Droid
{
    public class NativeService : INativeService
    {
        private ProgressDialog _progress;
        public void ShowLoading()
        {
            _progress = new ProgressDialog(Forms.Context);
            _progress.Indeterminate = true;
            _progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            _progress.SetCancelable(false);
            _progress.SetMessage("転送中...");
            _progress.Show();
        }

        public void HideLoading()
        {
            _progress?.Dismiss();
        }
    }
}