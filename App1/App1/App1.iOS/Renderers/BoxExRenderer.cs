using System;
using App1;
using App1.iOS;
using App1.iOS.Renderers;
using App1.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BoxViewEx), typeof(BoxExRenderer))]
namespace App1.iOS.Renderers
{
    internal class BoxExRenderer : BoxRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            var exImage = Element as BoxViewEx;
            var gr = new UITapGestureRecognizer(); // 3

            AddGestureRecognizer(gr); // ← 4
        }
    }
}