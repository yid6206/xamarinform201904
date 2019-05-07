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
        private List<BoxViewModel> _boxs = new List<BoxViewModel>();

        public BoxAreaViewModel()
        {
            UndoCommand = new Command(Undo);
            AddBoxCommand = new Command(AddBox);
        }

        public BoxViewModel[] Boxs => _boxs.ToArray();
        public ICommand UndoCommand { get; }
        public ICommand AddBoxCommand { get; }

        private void Undo()
        {
            UndoManager.Undo();
        }

        private void AddBox()
        {
            var box = _boxs.LastOrDefault()?.Clone();
            if (box == null)
                box = new BoxViewModel();
            else
                box.X += 50;
            _boxs.Add(box);
            RaisePropertyChanged(nameof(Boxs));
            UndoManager.Push(() =>
            {
                _boxs.Remove(box);
                RaisePropertyChanged(nameof(Boxs));
            });
        }
    }
}
