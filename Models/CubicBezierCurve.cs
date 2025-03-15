using System;

namespace ApproximationByBezier.Models;

public class CubicBezierCurve : Curve
{
    private readonly double[] _derivativePointsX;
    private readonly Func<double, double> _curve;

    public CubicBezierCurve(double startX, double endX, double[] derivativePointsX, Func<double, double> curve)
    {
        Points = new Point[4];
        Points[0] = new Point(startX, 0);
        // желательно все таки учитывать какие точки нам пришли, а не брать середину
        // хотя в наборе и приходит всегда точка, ктр находится посередине
        double step = Math.Abs(endX - startX) / 3d;
        Points[1] = new Point(startX + step, 0);
        Points[2] = new Point(endX - step, 0);
        Points[3] = new Point(endX, 0);
        _derivativePointsX = derivativePointsX;
        _curve = curve;
    }
        
    public override double[,] GetLocalMatrix()
    {
        double[,] matrix = {{2d, 0d, 0d, 0d}, {0d, 0d, 0d, 0d}, {0d, 0d, 0d, 0d}, {0d, 0d, 0d, 2d}};
        foreach (double der in _derivativePointsX)
        {
            double t = (der - Points[0].X) / (Points[3].X - Points[0].X);
            matrix[0, 0] += 2 * Math.Pow(1 - t, 6);
            matrix[0, 1] += 6 * t * Math.Pow(1 - t, 5);
            matrix[0, 2] += 6 * t * t * Math.Pow(1 - t, 4);
            matrix[0, 3] += 2 * t * t * t * Math.Pow(1 - t, 3);
            matrix[1, 0] += 6 * t * Math.Pow(1 - t, 5);
            matrix[1, 1] += 18 * t * t * Math.Pow(1 - t, 4);
            matrix[1, 2] += 18 * t * t * t * Math.Pow(1 - t, 3);
            matrix[1, 3] += 6 * t * t * t * t * Math.Pow(1 - t, 2);
            matrix[2, 0] += 6 * t * t * Math.Pow(1 - t, 4);
            matrix[2, 1] += 18 * t * t * t * Math.Pow(1 - t, 3);
            matrix[2, 2] += 18 * t * t * t * t * Math.Pow(1 - t, 2);
            matrix[2, 3] += 6 * t * t * t * t * t * (1 - t);
            matrix[3, 0] += 2 * t * t * t * Math.Pow(1 - t, 3);
            matrix[3, 1] += 6 * t * t * t * t * Math.Pow(1 - t, 2);
            matrix[3, 2] += 6 * t * t * t * t * t * (1 - t);
            matrix[3, 3] += 2 * t * t * t * t * t * t;
        }
        return matrix;
    }

    public override double[] GetLocalRightPart()
    {
        double fStart = _curve(Points[0].X);
        double fEnd = _curve(Points[3].X);
        double[] rightPart = [2 * fStart, 0d, 0d, 2 * fEnd];
        foreach (double der in _derivativePointsX)
        {
            double t = (der - Points[0].X) / (Points[3].X - Points[0].X);
            double f = _curve(der);
            rightPart[0] += 2 * (1 - t) * (1 - t) * (1 - t) * f;
            rightPart[1] += 6 * t * (1 - t) * (1 - t) * f;
            rightPart[2] += 6 * t * t * (1 - t) * f;
            rightPart[3] += 2 * t * t * t * f;
        }
        return rightPart;
    }
}