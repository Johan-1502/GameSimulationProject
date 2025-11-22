using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

public class LinealCongruenceGenerator
{
    private int index = 0;
    public string[] lines;
    public LinealCongruenceGenerator()
    {
        lines = File.ReadAllLines(Application.dataPath + "/numeros.txt");

        Debug.Log("Primer línea: " + lines[0]);
        Debug.Log("Primer número: " + float.Parse(lines[0], CultureInfo.InvariantCulture));
        Debug.Log("Total cargado: " + lines.Length);
    }

    public float getValue()
    {
        float value = float.Parse(lines[index], CultureInfo.InvariantCulture);
        index++;
        return value;
    }

    public float uniformTransform(float min, float max, float number)
    {
        return min + (max - min) * number;
    }
}
