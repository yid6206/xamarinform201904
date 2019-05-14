using System;
using System.Collections.Generic;
using System.Text;

namespace SkiaSharp
{
    public static class SKRectExtensions
    {
        public static SKRect Margin(this SKRect rect, int value)
        {
            return SKRect.Create(rect.Left - value, rect.Top - value, rect.Width + value * 2, rect.Height + value * 2);
        }

        public static SKPoint GetCenter(this SKRect rect)
        {
            return new SKPoint(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
        }
    }
}
