using System;
using System.Collections.Generic;
using System.Linq;

namespace ApproximationByBezier.Models
{
    public class BezierCurveApproximator
    {
        private Grid _grid;
        private Func<double, double> _curve;

        public BezierCurveApproximator(Grid grid, Func<double, double> curve)
        {
            _grid = grid;
            _curve = curve;
        }

        public List<QuadraticBezierCurve> Approximate()
        {
            List<QuadraticBezierCurve> curves = [];
            var intervalsAndDerivativesPoints = _grid.GridIntervals.Zip(_grid.DerivativePoints,
                (inter, der) => (inter.start, inter.end, der));
            foreach ((Point start, Point end, Point der) in intervalsAndDerivativesPoints)
            {
                var curveStart = new Point(start.X, _curve(start.X));
                var curveEnd = new Point(end.X, _curve(end.X));
                var curve = new QuadraticBezierCurve(curveStart, curveEnd, der.X, _curve);
                curves.Add(curve);
            }
            return curves;
        }
    }
}