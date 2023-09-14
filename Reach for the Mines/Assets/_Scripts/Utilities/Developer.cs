using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Developer
{
    [MenuItem("Developer/Example")]
    public static void Example()
    {
        Debug.Log("Triggered the example.");
    }
}
