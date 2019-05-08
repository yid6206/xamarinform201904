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
            UndoCommand = new Command(Undo);
            AddBoxCommand = new Command(AddBox);
            AddLabelCommand = new Command(AddLabel);
        }

        public BaseViewModel[] Items => _items.ToArray();
        public ICommand UndoCommand { get; }
        public ICommand AddBoxCommand { get; }
        public ICommand AddLabelCommand { get; }

        private void Undo()
        {
            UndoManager.Undo();
        }

        private void AddBox()
        {
            var box = _items.OfType<BoxViewModel>().LastOrDefault()?.Clone();
            if (box == null)
                box = new BoxViewModel();
            else
                box.X += 50;
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
            var label = _items.OfType<LabelViewModel>().LastOrDefault()?.Clone();
            if (label == null)
                label = new LabelViewModel();
            else
                label.X += 50;
            label.Text = (_items.OfType<LabelViewModel>().Count() + 1).ToString();
            _items.Add(label);
            RaisePropertyChanged(nameof(Items));
            UndoManager.Push(() =>
            {
                _items.Remove(label);
                RaisePropertyChanged(nameof(Items));
            });
        }
    }
}
