using SeatMaker.Helper;
using SeatMaker.Models;
using System.Linq;
using Xamarin.Forms;

namespace SeatMaker.ViewModels
{
    class AssignNumberViewModel : BaseViewModel
    {
        public Member Member { get; set; }
        public Command UpdateMembersCommand { get; set; }

        public AssignNumberViewModel(Member member)
        {
            Title = "編集";
            Member = member;
            UpdateMembersCommand = new Command(() => UpdateMembers());
        }

        private void UpdateMembers()
        {
            Member[] members = SerializationHelper.LoadMembers();
            members.FirstOrDefault(q => q.Name == Member.Name).Number = Member.Number;
            SerializationHelper.SaveMembers(members.ToArray());
        }
    }
}
