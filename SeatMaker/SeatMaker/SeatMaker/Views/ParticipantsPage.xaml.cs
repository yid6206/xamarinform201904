using SeatMaker.Models;
using SeatMaker.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeatMaker.Views
{
    public partial class ParticipantsPage : ContentPage
    {
        ParticipantsViewModel viewModel;

        public ParticipantsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ParticipantsViewModel();
        }

        async void OnMemberSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var member = args.SelectedItem as Member;
            if (member == null)
                return;

            await Navigation.PushModalAsync(new NavigationPage(new AssignNumberPage(member)));

            // Manually deselect item.
            ParticipantsListView.SelectedItem = null;
        }

        async void UpdateMembers_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new UpdateMembersPage()));
        }

        async void AssignNumber_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("確認", "番号が未設定の参加者にランダムに番号を割り振ります。\nよろしいですか？", "OK", "キャンセル");
            if (result)
                viewModel.AssignNumberCommand.Execute(null);
        }

        async void DeleteNumber_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("確認", "すべての参加者の番号を未設定にします。\nよろしいですか？", "OK", "キャンセル");
            if (result)
                viewModel.DeleteNumberCommand.Execute(null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadParticipantsCommand.Execute(null);
        }
    }
}