using System;

namespace ApproximationByBezier.Models;
public class QuadraticBezierCurve
{
    private readonly double _point1X;
    private readonly double _point1Y;
    private readonly double _point2X;
    private readonly double _point2Y;
    private readonly double _derivativePointX;
    private readonly Func<double, double> _curve;
    private double _pointX;
    private double _pointY;

    public Point[] QuadraticBezierCurvePoints { get; }

    public QuadraticBezierCurve(Point point1, Point point2, double derivativePointX, Func<double, double> curve)
    {
        _point1X = point1.X;
        _point1Y = point1.Y;
        _point2X = point2.X;
        _point2Y = point2.Y;
        _derivativePointX = derivativePointX;
        _curve = curve;
        
        FindCurvePoint();
        Point point = new(_pointX, _pointY);
        QuadraticBezierCurvePoints = [point1, point, point2];
    }

    private void FindCurvePoint()
    {
        double t = (_derivativePointX - _point1X) / (_point2X - _point1X);
        double f = _curve(_derivativePointX);
        //_pointX = (_derivativePointX - ((1 - t) * (1 - t) * _point1X) - (t * t * _point2X)) / (2 * t * (1 - t));
        _pointX = _derivativePointX;
        _pointY = (f - ((1 - t) * (1 - t) * _point1Y) - (t * t * _point2Y)) / (2 * t * (1 - t));
    }
}
    