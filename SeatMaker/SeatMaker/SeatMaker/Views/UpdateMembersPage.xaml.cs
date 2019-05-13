using SeatMaker.Models;
using SeatMaker.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeatMaker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateMembersPage : ContentPage
    {
        UpdateMembersViewModel viewModel;

        public UpdateMembersPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new UpdateMembersViewModel();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            viewModel.UpdateMembersCommand.Execute(null);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadMembersCommand.Execute(null);
        }
    }
}