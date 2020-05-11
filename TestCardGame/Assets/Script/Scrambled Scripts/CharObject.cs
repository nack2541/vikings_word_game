using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharObject : MonoBehaviour
{
    public char charactor;
    public Text text;
    public Image image;
    public RectTransform rectTransform;
    public int index;

    bool isSelected = false;

    [Header("Appearence")]
    public Color normalColor;
    public Color selectedColor;
    public CharObject Init(char c)
    {
        charactor = c;
        text.text = c.ToString();
        gameObject.SetActive(true);
        return this;
    }
    public void Select()
    {
        isSelected = !isSelected;

        image.color = isSelected ? selectedColor : normalColor;
        if (isSelected)
        {
            WordScramble.main.Select(this);
        }
        else
        {
            WordScramble.main.Unselect();
        }
    }
}
