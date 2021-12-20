using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixDebug : MonoBehaviour
{
    public static void CheckMatrix(string[,] input)
    {

        int rowLength = input.GetLength(0);
        int colLength = input.GetLength(1);
        string arrayString = "";
        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < colLength; j++)
            {
                if (input[i, j] != null)
                {
                    arrayString += string.Format("{0} ", input[i, j]);
                }
                else
                {
                    arrayString += string.Format("{0} ", "-");
                }
            }
            arrayString += System.Environment.NewLine;
        }

        Debug.Log(arrayString);
    }
}
