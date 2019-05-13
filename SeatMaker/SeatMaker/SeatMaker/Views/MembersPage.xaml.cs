using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SeatMaker.Models;
using SeatMaker.Views;
using SeatMaker.ViewModels;

namespace SeatMaker.Views
{
    public partial class MembersPage : ContentPage
    {
        MembersViewModel viewModel;

        public MembersPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MembersViewModel();
        }

        async void OnMemberSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var member = args.SelectedItem as Member;
            if (member == null)
                return;

            bool result = await DisplayAlert("確認", $"{member.Name}を削除しますか？", "OK", "キャンセル");
            if (result)
                viewModel.RemoveMembersCommand.Execute(member);

            // Manually deselect item.
            MembersListView.SelectedItem = null;
        }

        async void AddMember_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewMemberPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadMembersCommand.Execute(null);
        }
    }
}