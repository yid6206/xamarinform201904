using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SeatMaker.Models;

namespace SeatMaker.Views
{
    public partial class NewMemberPage : ContentPage
    {
        public Member Member { get; set; }

        public NewMemberPage()
        {
            InitializeComponent();
            BindingContext = this;

            Member = new Member { Number = "未設定" };
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddMember", Member);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}