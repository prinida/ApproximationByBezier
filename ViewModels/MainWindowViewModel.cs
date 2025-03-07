using ApproximationByBezier.Models;
using Avalonia.Media;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive;

namespace ApproximationByBezier.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> CalculateApproximation { get; }

        [Reactive] public List<Point> BezierCurvePoints { get; set; }
        
        [Reactive] public PathFigures BezierCurves { get; set; }

        private BezierCurveApproximator _approximator;
        private Grid _grid;

        public MainWindowViewModel()
        {
            _grid = new Grid(0, 200, 400, 10);
            _approximator = new BezierCurveApproximator(_grid, (double x) 
                => Math.Sqrt(10000 - (x - 100) * (x - 100)) + 400);
            CalculateApproximation = ReactiveCommand.Create(() =>
            {
                // тут нужен LINQ
                var bezierCurves = _approximator.Approximate();
                List<Point> points = [];
                PathFigures figures = [];
                foreach (var curve in bezierCurves)
                {
                    var point1 = curve.QuadraticBezierCurvePoints[0];
                    var point2 = curve.QuadraticBezierCurvePoints[1];
                    var point3 = curve.QuadraticBezierCurvePoints[2];
                    var aPoint1 = new Avalonia.Point(point1.X, point1.Y);
                    var aPoint2 = new Avalonia.Point(point2.X, point2.Y);
                    var aPoint3 = new Avalonia.Point(point3.X, point3.Y);
                    //points.AddRange(curve.QuadraticBezierCurvePoints);
                    var pathSegment = new QuadraticBezierSegment { Point1 = aPoint2, Point2 = aPoint3 };
                    var pathFigure = new PathFigure {IsClosed = false, StartPoint = aPoint1, Segments = [pathSegment]};
                    figures.Add(pathFigure);
                }
                BezierCurves = figures;
                //BezierCurvePoints = points;

                //var pathFigure = new PathFigure() {S};
                //pathFigure.
                //BezierCurves.Add(new PathFigure());
            });
        }
    }
}
