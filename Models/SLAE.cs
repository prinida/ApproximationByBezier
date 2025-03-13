using System;

namespace ApproximationByBezier.Models;

public class SLAE
{
    private double[,] _matrix;
    private double[] _rightPart;
    private double[] _solution;
    
    public double[] Solution { get; private set; }

    public SLAE(int matrixSize)
    {
        _matrix = new double[matrixSize, matrixSize];
        _rightPart = new double[matrixSize];
        _solution = new double[matrixSize];
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

    public void CalcSLAEbyGauss()
    {
        int n = _solution.Length;
        
        for (int k = 0; k <= n - 2; k++)
        {
            int m = k;
            for (int i = k; i < n; i++)
            {
                if (Math.Abs(_matrix[i, k]) >= Math.Abs(_matrix[m, k]))
                {
                    m = i;
                }
            }

            (_rightPart[k], _rightPart[m]) = (_rightPart[m], _rightPart[k]);
            for (int i = 0; i < n; ++i)
            {
                (_matrix[k, i], _matrix[m, i]) = (_matrix[m, i], _matrix[k, i]);
            }

            for (int i = k + 1; i <= n - 1; i++)
            {
                double tmp = _rightPart[i];
                double t = _matrix[i, k] / _matrix[k, k];
                _rightPart[i] = tmp - t * _rightPart[k];

                for (int j = k + 1; j <= n - 1; j++)
                {
                    tmp = _matrix[i, j];
                    _matrix[i, j] = tmp - t * _matrix[k, j];
                }
            }
        }

        _solution[n - 1] = _rightPart[n - 1] / _matrix[n - 1, n - 1];
        
        for (int k = n - 2 ; k >= 0; k--)
        {
            double sum = 0;
            for (int j = k + 1; j < n; j++)
            {
                sum += _matrix[k, j] * _solution[j];
            }
            _solution[k] = (_rightPart[k] - sum) / _matrix[k, k];
        }
    }
}