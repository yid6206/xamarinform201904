using App1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class BoxAreaViewModel : BaseViewModel
    {
        private List<BaseViewModel> _items = new List<BaseViewModel>();

        public BoxAreaViewModel()
        {
            X = 0;
            Y = 0;
            Scale = 1;
            UndoCommand = new Command(Undo);
            AddBoxCommand = new Command(AddBox);
            AddLabelCommand = new Command(AddLabel);
            ScaleUpCommand = new Command(ScaleUp);
            ScaleDownCommand = new Command(ScaleDown);
        }

        public double X { get => Get(nameof(X)); set => Set(nameof(X), value); }
        public double Y { get => Get(nameof(Y)); set => Set(nameof(Y), value); }
        public double Scale { get => Get(nameof(Scale)); set => Set(nameof(Scale), value); }
        public bool BoxMode { get => GetBool(nameof(BoxMode)); set => SetBool(nameof(BoxMode), value); }
        public bool LabelMode { get => GetBool(nameof(LabelMode)); set => SetBool(nameof(LabelMode), value); }
        public BaseViewModel[] Items => _items.ToArray();
        public ICommand UndoCommand { get; }
        public ICommand AddBoxCommand { get; }
        public ICommand AddLabelCommand { get; }
        public ICommand ScaleUpCommand { get; }
        public ICommand ScaleDownCommand { get; }

        private void Undo()
        {
            UndoManager.Undo();
        }

        private void AddBox()
        {
            BoxMode = !BoxMode;
        }
        public void AddBox(double x, double y)
        {
            var box = new BoxViewModel { X = x, Y = y };
            _items.Add(box);
            RaisePropertyChanged(nameof(Items));
            UndoManager.Push(() =>
            {
                _items.Remove(box);
                RaisePropertyChanged(nameof(Items));
            });
        }

        private void AddLabel()
        {
            LabelMode = !LabelMode;
        }
        public void AddLabel(double x, double y)
        {
            var label = new LabelViewModel { X = x, Y = y };
            label.Text = (_items.OfType<LabelViewModel>().Count() + 1).ToString();
            _items.Add(label);
            RaisePropertyChanged(nameof(Items));
            UndoManager.Push(() =>
            {
                _items.Remove(label);
                RaisePropertyChanged(nameof(Items));
            });
        }

        private void ScaleUp() { Scale *= 1.5; RaisePropertyChanged("Width"); }
        private void ScaleDown() => Scale /= 1.5;
    }
}
