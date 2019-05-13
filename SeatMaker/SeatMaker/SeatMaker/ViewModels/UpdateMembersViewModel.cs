using SeatMaker.Helper;
using SeatMaker.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace SeatMaker.ViewModels
{
    public class UpdateMembersViewModel : BaseViewModel
    {
        public ObservableCollection<Member> Members { get; set; }
        public Command LoadMembersCommand { get; set; }
        public Command UpdateMembersCommand { get; set; }

        public UpdateMembersViewModel()
        {
            Title = "更新";
            Members = new ObservableCollection<Member>();
            LoadMembersCommand = new Command(() => LoadMembers());
            UpdateMembersCommand = new Command(() => UpdateMembers());
        }

        private void LoadMembers()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Members.Clear();
                Member[] members = SerializationHelper.LoadMembers().OrderBy(q => q.Department).ThenBy(q => q.Name).ToArray();
                foreach (var member in members)
                    Members.Add(member);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void UpdateMembers()
        {
            SerializationHelper.SaveMembers(Members.ToArray());
        }
    }
}
