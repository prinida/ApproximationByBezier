using System;
using System.Collections.Generic;
using System.Linq;

namespace ApproximationByBezier.Models
{
    public class BezierCurveApproximator
    {
        private readonly Grid _grid;
        private readonly Func<double, double> _curve;
        private readonly int _bezierCurveOrder;

        public BezierCurveApproximator(Grid grid, Func<double, double> curve, int bezierCurveOrder)
        {
            _grid = grid;
            _curve = curve;
            _bezierCurveOrder = bezierCurveOrder;
        }

        public List<Curve> Approximate()
        {
            List<Curve> curves = [];
            var intervalsAndDerivativesPoints = _grid.GridIntervals.Zip(_grid.DerivativePoints,
                (inter, der) => (inter.start, inter.end, der));
            foreach ((Point start, Point end, Point[] der) in intervalsAndDerivativesPoints)
            {
                double[] derivativePointsX = der.Select(p => p.X).ToArray();
                switch (_bezierCurveOrder)
                {
                    case 2:
                        {
                            var curve = new QuadraticBezierCurve(start.X, end.X, derivativePointsX, _curve);
                            curves.Add(curve);
                            break;
                        }
                    case 3:
                        {
                            var curve = new CubicBezierCurve(start.X, end.X, derivativePointsX, _curve);
                            curves.Add(curve);
                            break;
                        }
                }
            }
            
            SLAE slae = new(_bezierCurveOrder * curves.Count + 1);
            SLAEbuilder builder = new(slae);
            builder.BuildSLAE(curves.ToArray());
            slae.CalcSLAEbyGauss();
            double[] solution = slae.Solution;
            int counter = 0;
            foreach (var curve in curves)
            {
                for (int i = 0; i <= _bezierCurveOrder; ++i)
                {
                    curve.Points[i] = new Point(curve.Points[i].X, solution[counter + i]);
                }
                counter += _bezierCurveOrder;
            }
            return curves;
        }
    }
}