using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeatMaker.Models.Figures
{
    public class InOutDrawingFigure : BaseDrawingFigure
    {
        public InOutDrawingFigure(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Height { get; set; }
        public override SKRect Rectangle => new SKRect(X, Y, X + Width, Y + Height);

        public override void MoveTo(SKPoint center)
        {
            X = center.X - Width / 2;
            Y = center.Y - Height / 2;
        }
    }
}
