using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using App1.Models;
using App1.Services;

namespace App1.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private Dictionary<string, double> _doubleValues = new Dictionary<string, double>();
        private Dictionary<string, string> _stringValues = new Dictionary<string, string>();
        private Dictionary<string, bool> _boolValues = new Dictionary<string, bool>();

        public event PropertyChangedEventHandler PropertyChanged;

        // INotifyPropertyChanged.PropertyChangedイベントを発生させる。
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected double Get(string name) => _doubleValues.TryGetValue(name, out double value) ? value : 0;
        protected void Set(string name, double value)
        {
            if (_doubleValues.TryGetValue(name, out double old) && value == old)
                return;
            _doubleValues[name] = value;
            RaisePropertyChanged(name);
        }

        protected string GetString(string name) => _stringValues.TryGetValue(name, out string value) ? value : "";
        protected void SetString(string name, string value)
        {
            if (_stringValues.TryGetValue(name, out string old) && value == old)
                return;
            _stringValues[name] = value;
            RaisePropertyChanged(name);
        }

        protected bool GetBool(string name) => _boolValues.TryGetValue(name, out bool value) ? value : false;
        protected void SetBool(string name, bool value)
        {
            if (_boolValues.TryGetValue(name, out bool old) && value == old)
                return;
            _boolValues[name] = value;
            RaisePropertyChanged(name);
        }




        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>() ?? new MockDataStore();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
