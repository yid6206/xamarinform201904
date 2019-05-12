using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using TouchTracking;
using TouchTrackingEffect;

namespace TouchTrackingEffectDemos
{
    public partial class EllipseDrawingPage : ContentPage
    {
        private enum Mode { None, Square, Ellipse }
        private const float GridWidth = 40f;
        private static readonly float SquareWidth = GridWidth * 2;
        private static readonly float EllipseWidth = GridWidth * 2;

        private Mode _mode = Mode.None;
        private List<EllipseDrawingFigure> _editingEllipses = new List<EllipseDrawingFigure>();
        private List<SquareDrawingFigure> _editingSquares = new List<SquareDrawingFigure>();
        private List<EllipseDrawingFigure> _completedEllipses = new List<EllipseDrawingFigure>();
        private List<SquareDrawingFigure> _completedSquares = new List<SquareDrawingFigure>();
        private SKPaint _paint = new SKPaint { Style = SKPaintStyle.Fill };

        private SKPoint _pressPoint;
        private BaseDrawingFigure _selectedFigure;

        public EllipseDrawingPage()
        {
            InitializeComponent();
        }

        public bool IsSquareMode => _mode == Mode.Square;
        public bool IsEllipseMode => _mode == Mode.Ellipse;

        private void OnClearButtonClicked(object sender, EventArgs args)
        {
            _completedEllipses.Clear();
            canvasView.InvalidateSurface();
        }

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            var location = ConvertToPixel(args.Location);
            _editingSquares.Clear();
            _editingEllipses.Clear();
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    {
                        _pressPoint = location;
                        switch (_mode)
                        {
                            case Mode.Square:
                                {
                                    _selectedFigure = _completedSquares.LastOrDefault(q => q.IsIn(_pressPoint));
                                }
                                break;
                            case Mode.Ellipse:
                                {
                                    _selectedFigure = _completedEllipses.LastOrDefault(q => q.IsIn(_pressPoint));
                                    break;
                                }
                        }
                        canvasView.InvalidateSurface();
                        break;
                    }

                case TouchActionType.Moved:
                    if (_selectedFigure != null)
                        _selectedFigure.MoveTo(location);
                    else
                    {
                        switch (_mode)
                        {
                            case Mode.Square:
                                _editingSquares.Add(CreateSquare(_pressPoint, location));
                                break;
                            case Mode.Ellipse:
                                _editingEllipses.AddRange(CreateEllipses(_pressPoint, location));
                                    break;
                        }
                    }
                    canvasView.InvalidateSurface();
                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    switch (_mode)
                    {
                        case Mode.Square:
                            {
                                if (_selectedFigure == null)
                                    PutSquare(CreateSquare(_pressPoint, location));
                            }
                            break;
                        case Mode.Ellipse:
                            {
                                if (_selectedFigure == null)
                                    PutEllipses(CreateEllipses(_pressPoint, location));
                                break;
                            }
                    }
                    _selectedFigure = null;
                    canvasView.InvalidateSurface();
                    break;
            }
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();

            foreach (var figure in _completedSquares.Concat(_editingSquares))
                DrawSquare(canvas, figure);
            foreach (var figure in _completedEllipses.Concat(_editingEllipses))
                DrawEllipse(canvas, figure);
        }

        private void DrawSquare(SKCanvas canvas, SquareDrawingFigure square)
        {
            _paint.Color = new SKColor(0, 0, 0);
            canvas.DrawRect(square.Rectangle.Margin(2), _paint);
            _paint.Color = new SKColor(255, 255, 255);
            canvas.DrawRect(square.Rectangle, _paint);
        }
        private void DrawEllipse(SKCanvas canvas, EllipseDrawingFigure ellipse)
        {
            _paint.Color = new SKColor(0, 0, 0);
            canvas.DrawOval(ellipse.Rectangle.Margin(2), _paint);
            _paint.Color = new SKColor(255, 255, 255);
            canvas.DrawOval(ellipse.Rectangle, _paint);
            _paint.Color = new SKColor(0, 0, 0);
            _paint.TextAlign = SKTextAlign.Center;
            _paint.TextSize = GridWidth;
            canvas.DrawText(ellipse.Text, ellipse.Center, _paint);
        }

        private SquareDrawingFigure CreateSquare(SKPoint pt1, SKPoint pt2)
        {
            var width = Math.Max(SquareWidth, (int)(Math.Abs(pt1.X - pt2.X) / SquareWidth) * SquareWidth);
            var height = Math.Max(SquareWidth, (int)(Math.Abs(pt1.Y - pt2.Y) / SquareWidth) * SquareWidth);
            return new SquareDrawingFigure(pt1.X, pt1.Y, width, height);
        }
        private void PutSquare(SquareDrawingFigure square)
        {
            _completedSquares.Add(square);
            UndoManager.Push(() =>
            {
                _completedSquares.Remove(square);
                canvasView.InvalidateSurface();
            });
        }

        private EllipseDrawingFigure[] CreateEllipses(SKPoint pt1, SKPoint pt2)
        {
            var left = Math.Min(pt1.X, pt2.X);
            var top = Math.Min(pt1.Y, pt2.Y);
            var right = Math.Max(pt1.X, pt2.X);
            var bottom = Math.Max(pt1.Y, pt2.Y);

            var ellipses = new List<EllipseDrawingFigure>();
            for (var x = left; x <= right; x += EllipseWidth)
            {
                for (var y = top; y <= bottom; y += EllipseWidth)
                {
                    var ellipse = new EllipseDrawingFigure
                    {
                        Center = new SKPoint(x, y),
                        Width = EllipseWidth,
                        Text = (_completedEllipses.Count + ellipses.Count + 1).ToString(),
                    };
                    ellipses.Add(ellipse);
                }
            }
            return ellipses.ToArray();
        }
        private void PutEllipses(EllipseDrawingFigure[] ellipses)
        {
            _completedEllipses.AddRange(ellipses);
            UndoManager.Push(() =>
            {
                _completedEllipses.RemoveAll(q => ellipses.Contains(q));
                canvasView.InvalidateSurface();
            });
        }

        private SKPoint ConvertToPixel(Point pt)
        {
            var x = (float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width);
            var y = (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height);
            return new SKPoint((int)(x / GridWidth + 0.5) * GridWidth, (int)(y / GridWidth + 0.5) * GridWidth);
        }

        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            UndoManager.Undo();
        }

        private void BtnSquare_Clicked(object sender, EventArgs e)
        {
            _mode = _mode == Mode.Square ? Mode.None : Mode.Square;
            btnSquare.BackgroundColor = _mode == Mode.Square ? Color.Orange : Color.White;
            btnEllipse.BackgroundColor = Color.White;
        }

        private void BtnEllipse_Clicked(object sender, EventArgs e)
        {
            _mode = _mode == Mode.Ellipse ? Mode.None : Mode.Ellipse;
            btnSquare.BackgroundColor = Color.White;
            btnEllipse.BackgroundColor = _mode == Mode.Ellipse ? Color.Orange : Color.White;
        }

        private void BtnScaleUp_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnScaleDown_Clicked(object sender, EventArgs e)
        {

        }
    }
}
