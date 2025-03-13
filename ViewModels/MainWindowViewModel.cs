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
        private Grid Grid => new (0, 400, 0, IntervalsCount);

        public MainWindowViewModel()
        {
            _intervalsCount = 3;
            CalculateApproximationCommand = ReactiveCommand.CreateFromTask(CalculateApproximation);
        }

        private async Task CalculateApproximation()
        {
            var approximator = new BezierCurveApproximator(Grid, (double x) 
                => Math.Sqrt(200 * 200 - (x - 200) * (x - 200)));
                
            var bezierCurves = approximator.Approximate();
            PathFigures figures = [];
            foreach (var curve in bezierCurves)
            {
                var point1 = curve.Point1;
                var point2 = curve.Point2;
                var point3 = curve.Point3;
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
            BezierCurves = figures;
            
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                var arcPoint1 = new Point(0, 0);
                var arcPoint2 = new Point(400, 0);
                var aArcPoint1 = new Avalonia.Point(arcPoint1.X, 400 - arcPoint1.Y);
                var aArcPoint2 = new Avalonia.Point(arcPoint2.X, 400 - arcPoint2.Y);
                var arcSegment = new ArcSegment { Point = aArcPoint2, Size = new Size(200, 200)};
                var arcPathFigure = new PathFigure {IsClosed = false, StartPoint = aArcPoint1, Segments = [arcSegment]};
                Curve = [arcPathFigure];
            });
        }
    }
}
