using System.Collections.Generic;

namespace ApproximationByBezier.Models
{
    public class Grid
    {
        private readonly double _startX;
        private readonly double _endX;
        private readonly double _yCoordinate;
        private readonly int _intervalsNumber;
        
        public List<(Point start, Point end)> GridIntervals { get; }
        public List<Point> DerivativePoints { get; }

        public Grid(double startX, double endX, double yCoordinate, int intervalsNumber)
        {
            _startX = startX;
            _endX = endX;
            _intervalsNumber = intervalsNumber;
            _yCoordinate = yCoordinate;
            GridIntervals = [];
            DerivativePoints = [];
            CalcGrid();
        }

        private void CalcGrid()
        {
            double step = (_endX - _startX) / _intervalsNumber;
            for (int i = 0; i < _intervalsNumber; ++i)
            {
                double x1 = _startX + i * step;
                double x2 = _startX + (i + 1) * step;
                double derivativeX = (x2 + x1) / 2;
                GridIntervals.Add((new Point(x1, _yCoordinate), new Point(x2, _yCoordinate)));
                DerivativePoints.Add(new Point(derivativeX, _yCoordinate));
            }
        }
    }
}