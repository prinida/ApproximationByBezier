using System;

namespace ApproximationByBezier.Models;
public class QuadraticBezierCurve : Curve
{
    public Point Point1 { get; set; }
    public Point Point2 { get; set; }
    public Point Point3 { get; set; }
    private readonly double _derivativePointX;
    private readonly Func<double, double> _curve;

    public QuadraticBezierCurve(Point start, Point end, double derivativePointX, Func<double, double> curve)
    {
        Point1 = start;
        Point3 = end;
        _derivativePointX = derivativePointX;
        _curve = curve;
    }

    public override double[,] GetLocalMatrix()
    {
        double t = (_derivativePointX - Point1.X) / (Point3.X - Point1.X);
        return new[,]
        {
            {2 + 2 * Math.Pow(1 - t, 4), 4 * t * Math.Pow(1 - t, 3), 2 * t * t * Math.Pow(1 - t, 2)}, 
            {4 * t * Math.Pow(1 - t, 3), 8 * t * t * Math.Pow(1 - t, 2), 4 * t * t * t * (1 - t) },
            {2 * t * t * Math.Pow(1 - t, 2), 4 * t * t * t * (1 - t), 2 + 2 * t * t * t * t}
        };
    }

    public override double[] GetLocalRightPart()
    {
        double t = (_derivativePointX - Point1.X) / (Point3.X - Point1.X);
        double f1 = _curve(Point1.X);
        double f2 = _curve(_derivativePointX);
        double f3 = _curve(Point3.X);
        return
        [
            2 * f1 + 2 * Math.Pow(1 - t, 2) * f2,
            4 * t * (1 - t) * f2,
            2 * t * t * f2 + 2 * f3
        ];
    }
}
    