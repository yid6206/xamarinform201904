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
        private double? _prevPanX;
        private double? _prevPanY;

        public static BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(BaseViewModel[]), typeof(BoxAreaLayout), null, BindingMode.OneWay);
        public static BindableProperty BoxModeProperty = BindableProperty.Create(nameof(BoxMode), typeof(bool), typeof(BoxAreaLayout), null, BindingMode.OneWay);
        public static BindableProperty LabelModeProperty = BindableProperty.Create(nameof(LabelMode), typeof(bool), typeof(BoxAreaLayout), null, BindingMode.OneWay);

        public BoxAreaLayout()
        {
            var effect = new TEffect();
            effect.OnTouchAction += Effect_OnTouch;
            Effects.Add(effect);

            var pan = new PanGestureRecognizer();
            pan.PanUpdated += Pan_PanUpdated;
            GestureRecognizers.Add(pan);

            
        }

        private void Effect_OnTouch(object obj, TouchActionEventArgs args)
        {
            var bounds = Bounds;
            if (args.Type == TouchActionType.Pressed && ViewModel.BoxMode)
            {
                ViewModel.AddBox(args.Point.X, args.Point.Y);
                return;
            }
            if (args.Type == TouchActionType.Pressed && ViewModel.LabelMode)
            {
                ViewModel.AddLabel(args.Point.X, args.Point.Y);
                return;
            }
            if (args.Type == TouchActionType.Moved)
            {
                System.Diagnostics.Debug.WriteLine("");
                System.Diagnostics.Debug.WriteLine($"◆◆◆ box area pan {args.Point.X}, {args.Point.Y}");
                if (_prevPanX.HasValue && _prevPanY.HasValue)
                {
                    //移動距離の計算
                    var distance = new Point(args.Point.X - _prevPanX.Value, args.Point.Y - _prevPanY.Value);
                    //bounds.X += args.X - _prevPanX.Value;
                    //bounds.Y += args.Y - _prevPanY.Value;
                    System.Diagnostics.Debug.WriteLine($"◆◆◆ box area bounds {bounds.X}, {bounds.Y}");
                    //Layout(bounds);
                    ViewModel.X += distance.X;
                    ViewModel.Y += distance.Y;
                    System.Diagnostics.Debug.WriteLine($"◆◆◆ box area ViewModel {ViewModel.X}, {ViewModel.Y}");
                    UpdateLocation();
                }
                _prevPanX = ViewModel.X;
                _prevPanY = ViewModel.Y;
            }
        }

        private void Tap_Tapped(object sender, EventArgs e)
        {
            if (!BoxMode)
                return;

            var items = ViewModel.Items.ToList();
        }

        private void Pan_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType != GestureStatus.Running || ViewModel.BoxMode || ViewModel.LabelMode)
                return;
            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine($"◆◆◆ box area pan {e.TotalX}, {e.TotalY}");
            if (_prevPanX.HasValue && _prevPanY.HasValue)
            {
                //移動距離の計算
                var distance = new Point();
                if (Device.RuntimePlatform == Device.iOS)
                    distance = new Point(e.TotalX - _prevPanX.Value, e.TotalY - _prevPanY.Value);  //iOSは移動いてもTotalはリセットされないので差分を使用する
                else if (Device.RuntimePlatform == Device.Android)
                    distance = new Point(e.TotalX, e.TotalY);    // Androidは移動後にTotalがリセットされるのでそのまま使う

                var bounds = Bounds;
                //bounds.X += e.TotalX - _prevPanX.Value;
                //bounds.Y += e.TotalY - _prevPanY.Value;
                bounds.X += distance.X;
                bounds.Y += distance.Y;
                System.Diagnostics.Debug.WriteLine($"◆◆◆ box area bounds {bounds.X}, {bounds.Y}");
                //Layout(bounds);
                ViewModel.X += distance.X;
                ViewModel.Y += distance.Y;
                UpdateLocation();
            }
            _prevPanX = e.TotalX;
            _prevPanY = e.TotalY;
        }

        public BoxAreaViewModel ViewModel => (BoxAreaViewModel)BindingContext;
        public BaseViewModel[] Items
        {
            get => (BaseViewModel[])GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
        public bool BoxMode
        {
            get => (bool)GetValue(BoxModeProperty);
            set => SetValue(BoxModeProperty, value);
        }
        public bool LabelMode
        {
            get => (bool)GetValue(LabelModeProperty);
            set => SetValue(LabelModeProperty, value);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdateLocation();
            SetBinding(ItemsProperty, new Binding(nameof(BoxAreaViewModel.Items), BindingMode.OneWay) { Source = BindingContext });
            SetBinding(XProperty, new Binding(nameof(BoxAreaViewModel.X), BindingMode.TwoWay) { Source = BindingContext });
            SetBinding(YProperty, new Binding(nameof(BoxAreaViewModel.Y), BindingMode.TwoWay) { Source = BindingContext });
            SetBinding(ScaleProperty, new Binding(nameof(BoxAreaViewModel.Scale), BindingMode.Default) { Source = BindingContext });
            SetBinding(ScaleProperty, new Binding(nameof(BoxAreaViewModel.Scale), BindingMode.Default) { Source = BindingContext });
            SetBinding(ScaleProperty, new Binding(nameof(BoxAreaViewModel.Scale), BindingMode.Default) { Source = BindingContext });
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(Items))
                UpdateFrames();
            if (propertyName == nameof(X) || propertyName == nameof(Y))
                UpdateLocation();
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

        private void UpdateLocation()
        {
            var bounds = Bounds;
            bounds.X = ViewModel.X;
            bounds.Y = ViewModel.Y;
            Layout(bounds);
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
