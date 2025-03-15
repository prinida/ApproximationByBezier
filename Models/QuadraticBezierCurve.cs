using System;

namespace ApproximationByBezier.Models;
public class QuadraticBezierCurve : Curve
{
    private readonly double[] _derivativePointsX;
    private readonly Func<double, double> _curve;

    public QuadraticBezierCurve(double startX, double endX, double[] derivativePointsX, Func<double, double> curve)
    {
        Points = new Point[3];
        Points[0] = new Point(startX, 0);
        // желательно все таки учитывать какие точки нам пришли, а не брать середину
        // хотя в наборе и приходит всегда точка, ктр находится посередине
        Points[1] = new Point((startX + endX) / 2d, 0);
        Points[2] = new Point(endX, 0);
        _derivativePointsX = derivativePointsX;
        _curve = curve;
    }

    public override double[,] GetLocalMatrix()
    {
        double[,] matrix = {{2d, 0d, 0d}, {0d, 0d, 0d}, {0d, 0d, 2d}};
        foreach (double der in _derivativePointsX)
        {
            double t = (der - Points[0].X) / (Points[2].X - Points[0].X);
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
        double fStart = _curve(Points[0].X);
        double fEnd = _curve(Points[2].X);
        double[] rightPart = [2 * fStart, 0, 2 * fEnd];
        foreach (double der in _derivativePointsX)
        {
            double t = (der - Points[0].X) / (Points[2].X - Points[0].X);
            double f = _curve(der);
            rightPart[0] += 2 * (1 - t) * (1 - t) * f;
            rightPart[1] += 4 * t * (1 - t) * f;
            rightPart[2] += 2 * t * t * f;
        }
        return rightPart;
    }
}
    