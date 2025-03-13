namespace ApproximationByBezier.Models;

public class SLAEbuilder
{
    private SLAE _slae;
    
    public SLAEbuilder(SLAE slae)
    {
        _slae = slae;
    }

    public void BuildSLAE(Curve[] curves)
    {
        for (int n = 0; n < curves.Length; ++n)
        {
            Curve curve = curves[n];
            double[,] localMatrix = curve.GetLocalMatrix();
            double[] localRightPart = curve.GetLocalRightPart();
            int elementsNumber = localRightPart.Length;

            for (int i = 0; i < elementsNumber; ++i)
            {
                int factor = n == 0 ? elementsNumber : elementsNumber - 1;
                int globalI = n * factor + i;
                for (int j = 0; j < elementsNumber; ++j)
                {
                    int globalJ = n * factor + j;
                    double matrixElement = localMatrix[i, j];
                    _slae.PutElementInMatrix(matrixElement, globalI, globalJ, i == 0);
                }
                double rightPartElement = localRightPart[i];
                _slae.PutElementInRightPart(rightPartElement, globalI, i == 0);
            }
        }
    }
}