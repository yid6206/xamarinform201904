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
    }
}
