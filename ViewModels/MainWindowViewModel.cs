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

        private BezierCurveApproximator _approximator;
        private Grid _grid;

        public MainWindowViewModel()
        {
            _intervalsCount = 3;
            CalculateApproximationCommand = ReactiveCommand.CreateFromTask(CalculateApproximation);
        }

        private async Task CalculateApproximation()
        {
            _grid = new Grid(0, 400, 0, IntervalsCount);
            _approximator = new BezierCurveApproximator(_grid, (double x) 
                => Math.Sqrt(210 * 210 - (x - 200) * (x - 200)) + 10);
                
            var bezierCurves = _approximator.Approximate();
            PathFigures figures = [];
            foreach (var curve in bezierCurves)
            {
                var point1 = curve.QuadraticBezierCurvePoints[0];
                var point2 = curve.QuadraticBezierCurvePoints[1];
                var point3 = curve.QuadraticBezierCurvePoints[2];
                // await Observable.Start(() =>
                // {
                //     var aPoint1 = new Avalonia.Point(point1.X, 400 - point1.Y);
                //     var aPoint2 = new Avalonia.Point(point2.X, 400 - point2.Y);
                //     var aPoint3 = new Avalonia.Point(point3.X, 400 - point3.Y);
                //     var bezierSegment = new QuadraticBezierSegment { Point1 = aPoint2, Point2 = aPoint3 };
                //     var bezierPathFigure = new PathFigure
                //     {
                //         IsClosed = false, StartPoint = aPoint1, Segments = [bezierSegment]
                //     };
                //     figures.Add(bezierPathFigure);
                // }, RxApp.MainThreadScheduler);
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var aPoint1 = new Avalonia.Point(point1.X, 400 - point1.Y);
                    var aPoint2 = new Avalonia.Point(point2.X, 400 - point2.Y);
                    var aPoint3 = new Avalonia.Point(point3.X, 400 - point3.Y);
                    var bezierSegment = new QuadraticBezierSegment { Point1 = aPoint2, Point2 = aPoint3 };
                    var bezierPathFigure = new PathFigure {IsClosed = false, StartPoint = aPoint1, Segments = [bezierSegment]};
                    figures.Add(bezierPathFigure);
                });
            }

            var arcPoint1 = new Point(0, 10);
            var arcPoint2 = new Point(400, 10);
            var aArcPoint1 = new Avalonia.Point(arcPoint1.X, 400 - arcPoint1.Y);
            var aArcPoint2 = new Avalonia.Point(arcPoint2.X, 400 - arcPoint2.Y);
            var arcSegment = new ArcSegment { Point = aArcPoint2, Size = new Size(210, 210)};
            var arcPathFigure = new PathFigure {IsClosed = false, StartPoint = aArcPoint1, Segments = [arcSegment]};
            Curve = [arcPathFigure];
                
            BezierCurves = figures;
        }
    }
}
