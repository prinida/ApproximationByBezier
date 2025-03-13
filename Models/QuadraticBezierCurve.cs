using System;

namespace ApproximationByBezier.Models;
public class QuadraticBezierCurve : Curve
{
    public Point Point1 { get; set; }
    public Point Point2 { get; set; }
    public Point Point3 { get; set; }
    
    private readonly double[] _derivativePointsX;
    private readonly Func<double, double> _curve;

    public QuadraticBezierCurve(double startX, double endX, double[] derivativePointsX, Func<double, double> curve)
    {
        Point1 = new Point(startX, 0);
        Point2 = new Point((startX + endX) / 2d, 0);
        Point3 = new Point(endX, 0);
        _derivativePointsX = derivativePointsX;
        _curve = curve;
    }

    public override double[,] GetLocalMatrix()
    {
        double[,] matrix = {{2d, 0d, 0d}, {0d, 0d, 0d}, {0d, 0d, 2d}};
        foreach (double der in _derivativePointsX)
        {
            double t = (der - Point1.X) / (Point3.X - Point1.X);
            matrix[0, 0] += 2 * (1 - t) * (1 - t) * (1 - t) * (1 - t);
            matrix[0, 1] += 4 * t * (1 - t) * (1 - t) * (1 - t);
            matrix[0, 2] += 2 * t * t * (1 - t) * (1 - t);
            matrix[1, 0] += 4 * t * (1 - t) * (1 - t) * (1 - t);
            matrix[1, 1] += 8 * t * t * (1 - t) * (1 - t);
            matrix[1, 2] += 4 * t * t * t * (1 - t);
            matrix[2, 0] += 2 * t * t * (1 - t) * (1 - t);
            matrix[2, 1] += 4 * t * t * t * (1 - t);
            matrix[2, 2] += 2 * t * t * t * t;
        }
        return matrix;
    }

    public override double[] GetLocalRightPart()
    {
        double fStart = _curve(Point1.X);
        double fEnd = _curve(Point3.X);
        double[] rightPart = [2 * fStart, 0, 2 * fEnd];
        foreach (double der in _derivativePointsX)
        {
            double t = (der - Point1.X) / (Point3.X - Point1.X);
            double f = _curve(der);
            rightPart[0] += 2 * (1 - t) * (1 - t) * f;
            rightPart[1] += 4 * t * (1 - t) * f;
            rightPart[2] += 2 * t * t * f;
        }
        return rightPart;
    }
}
    