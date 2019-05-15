using SeatMaker.Helper;
using SeatMaker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using Xamarin.Forms;

namespace SeatMaker.ViewModels
{
    public class ParticipantsViewModel : BaseViewModel
    {
        public ObservableCollection<Member> Members { get; set; }
        public Command LoadParticipantsCommand { get; set; }
        public Command AssignNumberCommand { get; set; }
        public Command DeleteNumberCommand { get; set; }

        public ParticipantsViewModel()
        {
            Title = "参加者";
            Members = new ObservableCollection<Member>();
            LoadParticipantsCommand = new Command(() => LoadParticipants());
            AssignNumberCommand = new Command(() => AssignNumber());
            DeleteNumberCommand = new Command(() => DeleteNumber());
        }

        private string _countText = "";
        public string CountText
        {
            get => _countText;
            set
            {
                if (_countText != value)
                {
                    _countText = value;
                    OnPropertyChanged(nameof(CountText));
                }
            }
        }

        private void LoadParticipants()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Update();
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

        private void AssignNumber()
        {
            var randNumbers = new List<int>();
            for (int i = 1; i <= Members.Count; i++)
                randNumbers.Add(i);

            string[] removeNumbers = Members.Select(q => q.Number).Where(q => q != "未設定").ToArray();
            foreach (var removeNumber in removeNumbers)
                randNumbers.Remove(int.Parse(removeNumber));

            var random = new Random();
            int n = randNumbers.Count();
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                int tmp = randNumbers[k];
                randNumbers[k] = randNumbers[n];
                randNumbers[n] = tmp;
            }

            int index = 0;
            foreach (var member in Members)
            {
                if (member.Number != "未設定")
                    continue;

                member.Number = randNumbers[index].ToString();
                index++;
            }


            SerializationHelper.SaveMembers(Members.ToArray());
            Update();
        }

        private void DeleteNumber()
        {
            foreach (var member in Members)
                member.Number = "未設定";

            SerializationHelper.SaveMembers(Members.ToArray());
            Update();
        }

        private void Update()
        {
            Members.Clear();
            Member[] members = SerializationHelper.LoadMembers().Where(q => q.IsParticipation).OrderBy(q => q.Department).ThenBy(q => q.Name).ToArray();
            foreach (var member in members)
                Members.Add(member);

            CountText = $"参加人数：{members.Count()}人";
        }
    }
}