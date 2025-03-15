using ApproximationByBezier.Models;
using Avalonia;
using Avalonia.Media;
using Avalonia.Threading;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Threading.Tasks;
using Point = ApproximationByBezier.Models.Point;

namespace ApproximationByBezier.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> CalculateApproximationCommand { get; }
        [Reactive] public PathFigures BezierCurves { get; set; }
        [Reactive] public PathFigures Curve { get; set; }
        [Reactive] public bool IsCurveVisible { get; set; }

        private int _selectedBezierCurveIndex;
        public int SelectedBezierCurveIndex
        {
            get => _selectedBezierCurveIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedBezierCurveIndex, value);
                DoublingRate = _doublingRate; // это как-то криво, но не понятно как еще заставить
                                              // пересчитаться это свойство
                CalculateApproximationCommand.Execute();
            }
        }

        private int _intervalsCount;
        public int IntervalsCount
        {
            get => _intervalsCount;
            set
            {
                this.RaiseAndSetIfChanged(ref _intervalsCount, value);
                CalculateApproximationCommand.Execute();
            }
        }

        private int _doublingRate;
        public int DoublingRate
        {
            get => _doublingRate;
            set
            {
                this.RaiseAndSetIfChanged(ref _doublingRate, value);
                int bezierCurveOrder = SelectedBezierCurveIndex + 2;
                InternalPointsCount = (int)(bezierCurveOrder * Math.Pow(2, value) - 1);
            }
        }
        
        private int _internalPointsCount;
        public int InternalPointsCount
        {
            get => _internalPointsCount;
            set
            {
                this.RaiseAndSetIfChanged(ref _internalPointsCount, value);
                CalculateApproximationCommand.Execute();
            }
        }

        public MainWindowViewModel()
        {
            _intervalsCount = 3;
            _selectedBezierCurveIndex = 0;
            _doublingRate = 0;
            int bezierCurveOrder = _selectedBezierCurveIndex + 2;
            _internalPointsCount = (int)(bezierCurveOrder * Math.Pow(2, _doublingRate) - 1);
            CalculateApproximationCommand = ReactiveCommand.CreateFromTask(CalculateApproximation);
            
            var arcPoint1 = new Point(0, 0);
            var arcPoint2 = new Point(400, 0);
            var aArcPoint1 = new Avalonia.Point(arcPoint1.X, 400 - arcPoint1.Y);
            var aArcPoint2 = new Avalonia.Point(arcPoint2.X, 400 - arcPoint2.Y);
            var arcSegment = new ArcSegment
                { Point = aArcPoint2, Size = new Size(200, 200)};
            var arcPathFigure = new PathFigure
                {IsClosed = false, StartPoint = aArcPoint1, Segments = [arcSegment]};
            Curve = [arcPathFigure];
        }

        private async Task CalculateApproximation()
        {
            int bezierCurveOrder = SelectedBezierCurveIndex + 2;
            var grid = new Grid(0, 400, 0, IntervalsCount, InternalPointsCount);
            var approximator = new BezierCurveApproximator(grid, (double x) 
                => Math.Sqrt(200 * 200 - (x - 200) * (x - 200)), bezierCurveOrder);
                
            var bezierCurves = approximator.Approximate();
            PathFigures figures = [];
            foreach (var curve in bezierCurves)
            {
                switch (bezierCurveOrder)
                {
                    case 2:
                        {
                            var point1 = curve.Points[0];
                            var point2 = curve.Points[1];
                            var point3 = curve.Points[2];
                            await Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                var aPoint1 = new Avalonia.Point(point1.X, 400 - point1.Y);
                                var aPoint2 = new Avalonia.Point(point2.X, 400 - point2.Y);
                                var aPoint3 = new Avalonia.Point(point3.X, 400 - point3.Y);
                                var bezierSegment = new QuadraticBezierSegment
                                    { Point1 = aPoint2, Point2 = aPoint3};
                                var bezierPathFigure = new PathFigure 
                                    {IsClosed = false, StartPoint = aPoint1, Segments = [bezierSegment]};
                                figures.Add(bezierPathFigure);
                            });
                            break;
                        }
                    case 3:
                        {
                            var point1 = curve.Points[0];
                            var point2 = curve.Points[1];
                            var point3 = curve.Points[2];
                            var point4 = curve.Points[3];
                            await Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                var aPoint1 = new Avalonia.Point(point1.X, 400 - point1.Y);
                                var aPoint2 = new Avalonia.Point(point2.X, 400 - point2.Y);
                                var aPoint3 = new Avalonia.Point(point3.X, 400 - point3.Y);
                                var aPoint4 = new Avalonia.Point(point4.X, 400 - point4.Y);
                                var bezierSegment = new BezierSegment
                                    { Point1 = aPoint2, Point2 = aPoint3, Point3 = aPoint4};
                                var bezierPathFigure = new PathFigure 
                                    {IsClosed = false, StartPoint = aPoint1, Segments = [bezierSegment]};
                                figures.Add(bezierPathFigure);
                            });
                            break;
                        }
                }
            }
            BezierCurves = figures;
        }
    }
}
