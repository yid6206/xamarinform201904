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
        public static BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(BaseViewModel[]), typeof(BoxAreaLayout), null, BindingMode.OneWay);

        public BaseViewModel[] Items
        {
            get => (BaseViewModel[])GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            SetBinding(ItemsProperty, new Binding(nameof(BoxAreaViewModel.Items), BindingMode.OneWay) { Source = BindingContext });
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(Items))
                UpdateFrames();
        }

        private void UpdateFrames()
        {
            var boxViews = Children.OfType<BoxFrame>().ToArray();
            var labelViews = Children.OfType<LabelFrame>().ToArray();

            var boxVMs = Items.OfType<BoxViewModel>().ToArray();
            var labelVMs = Items.OfType<LabelViewModel>().ToArray();

            var removeViews = new List<View>();
            removeViews.AddRange(boxViews.Skip(boxVMs.Length));
            removeViews.AddRange(labelViews.Skip(labelVMs.Length));
            var addViews = new List<View>();
            for (var i = 0; i < boxVMs.Length; i++)
            {
                var view = boxViews.ElementAtOrDefault(i);
                if (view == null)
                {
                    view = new BoxFrame();
                    addViews.Add(view);
                }
                view.BindingContext = boxVMs[i];
            }
            for (var i = 0; i < labelVMs.Length; i++)
            {
                var view = labelViews.ElementAtOrDefault(i);
                if (view == null)
                {
                    view = new LabelFrame();
                    addViews.Add(view);
                }
                view.BindingContext = labelVMs[i];
            }

            foreach (var view in removeViews)
                Children.Remove(view);
            foreach (var view in addViews)
                Children.Add(view);
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
