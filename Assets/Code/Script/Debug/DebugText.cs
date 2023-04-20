using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    private static Text _text;
    private void Awake()
    {
        _text = GetComponent<Text>();
    }
    public static void WriteText(string text)
    {
        _text.text = text;
    }
}
