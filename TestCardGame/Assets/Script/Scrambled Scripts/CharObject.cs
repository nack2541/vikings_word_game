using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharObject : MonoBehaviour
{
    public char charactor;
    public Text text;
    public RectTransform rectTransform;
    public int index;

    [Header("Appearence")]
    public Color normalColor;
    public Color selectedColor;
    public CharObject init(char c)
    {
        c=charactor;
        text.text=c.ToString();
        gameObject.SetActive(true);
        return this;
    }

}
