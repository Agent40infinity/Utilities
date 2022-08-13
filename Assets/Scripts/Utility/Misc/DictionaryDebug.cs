using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DictionaryDebug : MonoBehaviour
{
    public static string Call(Dictionary<string, KeyCode> input)
    {
        string output = "";

        if (input == null)
        {
            return "Error: Dictionary Null.";
        }

        for (int i = 0; i < input.Count; i++)
        {
            output += string.Format("'{0}': {1}", input.ElementAt(i).Key, input.ElementAt(i).Value.ToString());
            output += System.Environment.NewLine;
        }

        return output;
    }
}
