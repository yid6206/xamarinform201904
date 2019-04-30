using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Forms;

namespace App1.Views
{
    public class BoxAreaLayout : StackLayout
    {
        public void ResetAllSelect()
        {
            foreach (var box in GetBoxs(this))
                box.Selected = false;
        }

        private BoxViewEx[] GetBoxs<T>(Layout<T> layout) where T : View
        {
            var result = new List<BoxViewEx>();
            foreach (var child in layout.Children.OfType<View>())
            {
                if (child is BoxViewEx)
                    result.Add((BoxViewEx)child);
                else if (child is Layout<T>)
                    result.AddRange(GetBoxs((Layout<T>)child));
            }
            return result.ToArray();
        }
    }
}
