using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    void Start()
    {
        string[] lines = File.ReadAllLines(Application.dataPath + "numeros.txt");

        List<float> values = new List<float>();
        foreach (string line in lines)
        {
            values.Add(float.Parse(line));
        }

        Debug.Log("Primer número: " + values[0]);
        Debug.Log("Total cargado: " + values.Count);
    }
}
