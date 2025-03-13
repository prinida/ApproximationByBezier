namespace ApproximationByBezier.Models;

public class SLAE
{
    private double[,] _matrix;
    private double[] _rightPart;

    public SLAE(int matrixSize)
    {
        _matrix = new double[matrixSize, matrixSize];
        _rightPart = new double[matrixSize];
    }

    public void PutElementInMatrix(double element, int i, int j, bool isSum)
    {
        if (isSum)
        {
            _matrix[i, j] += element;
        }
        _matrix[i, j] = element;
    }

    public void PutElementInRightPart(double element, int i, bool isSum)
    {
        if (isSum)
        {
            _rightPart[i] += element;
        }
        _rightPart[i] = element;
    }
}