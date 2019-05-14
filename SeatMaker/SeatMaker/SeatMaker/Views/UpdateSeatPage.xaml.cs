using SeatMaker.Helper;
using SeatMaker.Models;
using SeatMaker.Models.Figures;
using SeatMaker.Views.Touch;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeatMaker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateSeatPage : ContentPage
    {
        private enum Mode { None, InOut, Square, Ellipse }
        private const float GridWidth = 40f;
        private static readonly float InOutWidth = GridWidth * 2;
        private static readonly float SquareWidth = GridWidth * 2;
        private static readonly float EllipseWidth = GridWidth * 2;

        private Mode _mode = Mode.None;
        private List<InOutDrawingFigure> _editingInOuts = new List<InOutDrawingFigure>();
        private List<SquareDrawingFigure> _editingSquares = new List<SquareDrawingFigure>();
        private List<EllipseDrawingFigure> _editingEllipses = new List<EllipseDrawingFigure>();
        private List<InOutDrawingFigure> _completedInOuts = new List<InOutDrawingFigure>();
        private List<SquareDrawingFigure> _completedSquares = new List<SquareDrawingFigure>();
        private List<EllipseDrawingFigure> _completedEllipses = new List<EllipseDrawingFigure>();
        private SKPaint _paint = new SKPaint { Style = SKPaintStyle.Fill };

        private SKPoint _pressPoint;
        private SKPoint _moveStartPoint;
        private BaseDrawingFigure _selectedFigure;

        public UpdateSeatPage()
        {
            InitializeComponent();
        }

        private void OnClearButtonClicked(object sender, EventArgs args)
        {
            _completedEllipses.Clear();
            canvasView.InvalidateSurface();
        }

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            var location = ConvertToPixel(args.Location);
            _editingInOuts.Clear();
            _editingSquares.Clear();
            _editingEllipses.Clear();
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    {
                        _pressPoint = location;
                        switch (_mode)
                        {
                            case Mode.InOut:
                                _selectedFigure = _completedInOuts.LastOrDefault(q => q.IsIn(_pressPoint));
                                break;
                            case Mode.Square:
                                _selectedFigure = _completedSquares.LastOrDefault(q => q.IsIn(_pressPoint));
                                break;
                            case Mode.Ellipse:
                                _selectedFigure = _completedEllipses.LastOrDefault(q => q.IsIn(_pressPoint));
                                break;
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
                            case Mode.InOut:
                                _editingInOuts.Add(CreateInOut(_pressPoint, location));
                                break;
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
                        case Mode.InOut:
                            if (_selectedFigure == null)
                                PutInOut(CreateInOut(_pressPoint, location));
                            break;
                        case Mode.Square:
                            if (_selectedFigure == null)
                                PutSquare(CreateSquare(_pressPoint, location));
                            break;
                        case Mode.Ellipse:
                            if (_selectedFigure == null)
                                PutEllipses(CreateEllipses(_pressPoint, location));
                            break;
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
            Draw(canvas);
        }

        private void Draw(SKCanvas canvas)
        {
            foreach (var figure in _completedInOuts.Concat(_editingInOuts))
                DrawInOut(canvas, figure);
            foreach (var figure in _completedSquares.Concat(_editingSquares))
                DrawSquare(canvas, figure);
            foreach (var figure in _completedEllipses.Concat(_editingEllipses))
                DrawEllipse(canvas, figure);
        }

        private void DrawInOut(SKCanvas canvas, InOutDrawingFigure inout)
        {
            _paint.Color = new SKColor(0, 0, 0);
            canvas.DrawRect(inout.Rectangle.Margin(2), _paint);
            _paint.Color = new SKColor(255, 255, 255);
            canvas.DrawRect(inout.Rectangle, _paint);
            _paint.Color = new SKColor(0, 0, 0);
            var center = inout.Rectangle.GetCenter();
            if (inout.Width >= inout.Height)
                DrawText(canvas, center, "in/out");
            else
                DrawText(canvas, center, "in", "out");
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
            DrawText(canvas, ellipse.Center, ellipse.Text);
        }

        private void DrawText(SKCanvas canvas, SKPoint textCenter, params string[] texts)
        {
            var offsetY = _paint.TextSize / 10f;
            _paint.TextAlign = SKTextAlign.Center;
            _paint.TextSize = GridWidth;
            if (texts.Length == 1)
            {
                canvas.DrawText(texts[0], textCenter.X, textCenter.Y + _paint.TextSize / 2 - offsetY, _paint);
            }
            else if (texts.Length == 2)
            {
                canvas.DrawText(texts[0], textCenter.X, textCenter.Y - _paint.TextSize / 2 - offsetY, _paint);
                canvas.DrawText(texts[1], textCenter.X, textCenter.Y + _paint.TextSize / 2 - offsetY, _paint);
            }
            else
                throw new NotImplementedException();
        }

        private InOutDrawingFigure CreateInOut(SKPoint pt1, SKPoint pt2)
        {
            var width = Math.Max(InOutWidth, (int)(Math.Abs(pt1.X - pt2.X) / InOutWidth) * InOutWidth);
            var height = Math.Max(InOutWidth, (int)(Math.Abs(pt1.Y - pt2.Y) / InOutWidth) * InOutWidth);
            return new InOutDrawingFigure(pt1.X, pt1.Y, width, height);
        }
        private SquareDrawingFigure CreateSquare(SKPoint pt1, SKPoint pt2)
        {
            var width = Math.Max(SquareWidth, (int)(Math.Abs(pt1.X - pt2.X) / SquareWidth) * SquareWidth);
            var height = Math.Max(SquareWidth, (int)(Math.Abs(pt1.Y - pt2.Y) / SquareWidth) * SquareWidth);
            return new SquareDrawingFigure(pt1.X, pt1.Y, width, height);
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

        private void PutInOut(InOutDrawingFigure inout)
        {
            _completedInOuts.Add(inout);
            UndoManager.Push(() =>
            {
                _completedInOuts.Remove(inout);
                canvasView.InvalidateSurface();
            });
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

        private void SaveToPng()
        {
            var bitmap = new SKBitmap((int)canvasView.CanvasSize.Width, (int)canvasView.CanvasSize.Height, isOpaque: false);
            var canvas = new SKCanvas(bitmap);
            Draw(canvas);
            SerializationHelper.SaveImage(bitmap);
        }

        private void UpdateButtonColor()
        {
            btnInOut.BackgroundColor = _mode == Mode.InOut ? Color.Orange : Color.White;
            btnSquare.BackgroundColor = _mode == Mode.Square ? Color.Orange : Color.White;
            btnEllipse.BackgroundColor = _mode == Mode.Ellipse ? Color.Orange : Color.White;
        }

        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            UndoManager.Undo();
        }

        private void BtnInOut_Clicked(object sender, EventArgs e)
        {
            _mode = _mode == Mode.InOut ? Mode.None : Mode.InOut;
            UpdateButtonColor();
        }

        private void BtnSquare_Clicked(object sender, EventArgs e)
        {
            _mode = _mode == Mode.Square ? Mode.None : Mode.Square;
            UpdateButtonColor();
        }

        private void BtnEllipse_Clicked(object sender, EventArgs e)
        {
            _mode = _mode == Mode.Ellipse ? Mode.None : Mode.Ellipse;
            UpdateButtonColor();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            SaveToPng();
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}