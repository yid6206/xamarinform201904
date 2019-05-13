using SeatMaker.Helper;
using SeatMaker.Models;
using SeatMaker.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace SeatMaker.ViewModels
{
    public class MembersViewModel : BaseViewModel
    {
        public ObservableCollection<Member> Members { get; set; }
        public Command LoadMembersCommand { get; set; }
        public Command RemoveMembersCommand { get; set; }

        public MembersViewModel()
        {
            Title = "全員";
            Members = new ObservableCollection<Member>();
            LoadMembersCommand = new Command(() => LoadMembers());
            RemoveMembersCommand = new Command((obj) =>
            {
                var deleteMember = obj as Member;
                Members.Remove(deleteMember);
                SerializationHelper.SaveMembers(Members.ToArray());
            });

            MessagingCenter.Subscribe<NewMemberPage, Member>(this, "AddMember", (obj, member) =>
            {
                var newMember = member as Member;
                Members.Add(newMember);
                SerializationHelper.SaveMembers(Members.ToArray());
            });
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
    }
}