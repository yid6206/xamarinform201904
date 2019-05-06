using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Models
{
    public static class UndoManager
    {
        private static Stack<Action> _undoList = new Stack<Action>();

        public static void Push(Action action) => _undoList.Push(action);
        public static void Undo()
        {
            if (_undoList.Count > 0)
                _undoList.Pop().Invoke();
        }
    }
}
