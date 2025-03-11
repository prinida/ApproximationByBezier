namespace ApproximationByBezier.Models;

public abstract class Curve
{
    public abstract double[,] GetLocalMatrix();

    public abstract double[] GetLocalRightPart();
}