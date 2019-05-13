using SeatMaker.Models;
using SeatMaker.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeatMaker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssignNumberPage : ContentPage
    {
        AssignNumberViewModel viewModel;

        public AssignNumberPage(Member member)
        {
            InitializeComponent();
            BindingContext = viewModel = new AssignNumberViewModel(member);
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
    }
}