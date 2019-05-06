using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace App1.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public BoxAreaViewModel BoxAreaVM { get; }

        public AboutViewModel()
        {
            BoxAreaVM = new BoxAreaViewModel();
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}