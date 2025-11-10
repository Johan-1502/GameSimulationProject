using UnityEngine;

public class MarkovAssignation
{
    private float[,] probabilityMatrix;
    private int lastPosition;

    public MarkovAssignation(float[,] matrix, int initialPosition)
    {
        probabilityMatrix = matrix;
        lastPosition = initialPosition;
    }

    public int transformValue(float value)
    {
        float acumulativeProbability = 0f;
        for (int i = 0; i < probabilityMatrix.GetLength(1); i++)
        {
            acumulativeProbability += probabilityMatrix[lastPosition, i];
            if (value < acumulativeProbability)
            {
                lastPosition = i;
                return lastPosition;
            }
        }
        return -1;
    }
}
