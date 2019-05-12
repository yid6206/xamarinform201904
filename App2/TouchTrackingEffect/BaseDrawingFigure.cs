using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TouchTrackingEffect
{
    public abstract class BaseDrawingFigure
    {
        public float Width { get; set; }
        public abstract SKRect Rectangle { get; }

        public abstract void MoveTo(SKPoint center);

        public bool IsIn(SKPoint pt)
        {
            SKRect rect = Rectangle;

            return pt.X >= rect.Left && pt.X <= rect.Right && pt.Y >= rect.Top && pt.Y <= rect.Bottom;
        }
    }
}
