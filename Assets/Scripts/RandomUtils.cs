using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility class containing a collection of methods for random calculations.
/// </summary>
/// <author>IGM</author>
/// <note>Borrowed by Dan Singer</note>
public static class RandomUtils
{
    /// <summary>
    /// Return a gaussian distributed value provided a mean and standard deviation
    /// </summary>
    public static float Gaussian(float mean, float stdDev)
    {
        float val1 = Random.Range(0f, 1f);
        float val2 = Random.Range(0f, 1f);
        float gaussValue = Mathf.Sqrt(-2.0f * Mathf.Log(val1)) * Mathf.Sin(2.0f * Mathf.PI * val2);
        return mean + stdDev * gaussValue;
    }
}