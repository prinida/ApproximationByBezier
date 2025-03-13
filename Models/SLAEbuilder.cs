namespace ApproximationByBezier.Models;

public class SLAEbuilder
{
    private SLAE _slae;
    
    public SLAE SLAE { get; private set; }
    
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
                int globalI = n * elementsNumber + i;
                if (n != 0)
                {
                    globalI--;
                }
                for (int j = 0; j < elementsNumber; ++j)
                {
                    int globalJ = n * elementsNumber + j;
                    if (n != 0)
                    {
                        globalJ--;
                    }
                    double matrixElement = localMatrix[i, j];
                    _slae.PutElementInMatrix(matrixElement, globalI, globalJ, i == 0);
                }
                double rightPartElement = localRightPart[i];
                _slae.PutElementInRightPart(rightPartElement, globalI, i == 0);
            }
        }
    }
}