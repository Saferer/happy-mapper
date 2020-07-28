using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathConversionUtil
{
    public static float[] DoubleArrayToFloat(double[] d)
    {
        float[] f = new float[d.Length];
        for (int i = 0; i < d.Length; i++)
        {
            f[i] = (float)d[i];
        }
        return f;
    }
}
