using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace TouchTrackingEffectDemos
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            NavigateCommand = new Command<Type>((Type pageType) =>
            {
                Page page = (Page)Activator.CreateInstance(pageType);
                Navigation.PushAsync(page).Wait();
            });

            BindingContext = this;
        }

        public ICommand NavigateCommand { private set; get; }
    }
}
