using System.Collections.Generic;

namespace ApproximationByBezier.Models
{
    public class Grid
    {
        private readonly double _startX;
        private readonly double _endX;
        private readonly double _yCoordinate;
        private readonly int _intervalsNumber;
        private readonly int _internalPointsNumber;
        
        public List<(Point start, Point end)> GridIntervals { get; }
        public List<Point[]> DerivativePoints { get; }

        public Grid(double startX, double endX, double yCoordinate, int intervalsNumber, int internalPointsNumber)
        {
            _startX = startX;
            _endX = endX;
            _intervalsNumber = intervalsNumber;
            _internalPointsNumber = internalPointsNumber;
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
                GridIntervals.Add((new Point(x1, _yCoordinate), new Point(x2, _yCoordinate)));

                Point[] derivativePoints = new Point[_internalPointsNumber];
                double derivativeStep = (x2 - x1) / (_internalPointsNumber + 1);
                for (int j = 0; j < _internalPointsNumber; ++j)
                {
                    derivativePoints[j] = new Point(x1 + (j + 1) * derivativeStep, _yCoordinate);
                }
                DerivativePoints.Add(derivativePoints);
            }
        }
    }
}