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
            List<Curve> curves = [];
            var intervalsAndDerivativesPoints = _grid.GridIntervals.Zip(_grid.DerivativePoints,
                (inter, der) => (inter.start, inter.end, der));
            foreach ((Point start, Point end, Point[] der) in intervalsAndDerivativesPoints)
            {
                double[] derivativePointsX = der.Select(p => p.X).ToArray();
                var curve = new QuadraticBezierCurve(start.X, end.X, derivativePointsX, _curve);
                curves.Add(curve);
            }

            SLAE slae = new(2 * curves.Count + 1);
            SLAEbuilder builder = new(slae);
            builder.BuildSLAE(curves.ToArray());
            slae.CalcSLAEbyGauss();
            double[] solution = slae.Solution;
            List<QuadraticBezierCurve> quadraticBezierCurves = [];
            int counter = 0;
            foreach ((Point start, Point end, Point[] der) in intervalsAndDerivativesPoints)
            {
                double[] derivativePointsX = der.Select(p => p.X).ToArray();
                var qBcurve = new QuadraticBezierCurve(start.X, end.X, derivativePointsX, _curve);
                qBcurve.Point1 = new Point(qBcurve.Point1.X, solution[counter]);
                qBcurve.Point2 = new Point(qBcurve.Point2.X, solution[counter + 1]);
                qBcurve.Point3 = new Point(qBcurve.Point3.X, solution[counter + 2]);
                counter += 2;
                quadraticBezierCurves.Add(qBcurve);
            }
            return quadraticBezierCurves;
        }
    }
}