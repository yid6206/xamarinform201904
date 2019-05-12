using System;
using Xamarin.Forms;
using SkiaSharp;
using TouchTrackingEffect;

namespace TouchTrackingEffectDemos
{
    public class EllipseDrawingFigure : BaseDrawingFigure
    {
        public EllipseDrawingFigure()
        {
        }

        public SKPoint Center { get; set; }
        public override SKRect Rectangle => new SKRect(Center.X - Width / 2, Center.Y - Width / 2, Center.X + Width / 2, Center.Y + Width / 2);
        public string Text { get; set; }
        public Point LastFingerLocation { set; get; }

        public override void MoveTo(SKPoint center)
        {
            Center = center;
        }
    }
}
