using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using App1.ViewModels;
using System.Runtime.CompilerServices;

namespace App1.Views
{
    public class BoxAreaLayout : StackLayout
    {
        public static BindableProperty BoxsProperty = BindableProperty.Create(nameof(Boxs), typeof(BoxViewModel[]), typeof(BoxAreaLayout), null, BindingMode.OneWay);

        public void ResetAllSelect()
        {
        }

        public BoxViewModel[] Boxs
        {
            get => (BoxViewModel[])GetValue(BoxsProperty);
            set => SetValue(BoxsProperty, value);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            SetBinding(BoxsProperty, new Binding(nameof(BoxAreaViewModel.Boxs), BindingMode.OneWay) { Source = BindingContext });
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(Boxs))
                UpdateBoxs();
        }

        private void UpdateBoxs()
        {
            var views = Children.OfType<BoxFrame>().ToArray();
            Children.Clear();
            foreach (var vm in Boxs)
            {
                var view = views.FirstOrDefault(q => q.BindingContext == vm);
                if (view == null)
                    view = new BoxFrame { BindingContext = vm };
                Children.Add(view);
            }
        }

        private BoxFrame[] GetBoxs<T>(Layout<T> layout) where T : View
        {
            var result = new List<BoxFrame>();
            foreach (var child in layout.Children.OfType<View>())
            {
                if (child is BoxFrame)
                    result.Add((BoxFrame)child);
                else if (child is Layout<T>)
                    result.AddRange(GetBoxs((Layout<T>)child));
            }
            return result.ToArray();
        }
    }
}
