namespace ApproximationByBezier.Models;

public abstract class Curve
{
    public Point[] Points { get; set; }
    
    public abstract double[,] GetLocalMatrix();

    public abstract double[] GetLocalRightPart();
}