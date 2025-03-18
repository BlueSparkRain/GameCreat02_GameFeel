using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ColorSquareData",menuName ="MyCustomData/ColorSquareSO")]
public class ColorSquareSO : ScriptableObject
{
    public E_Color E_Color;
    public Color SquareColor;
    public Sprite ColorSquareSprite;
}

public enum E_Color 
{
    ³à,³È,»Æ,ÂÌ,Çà,À¶,×Ï,°×
}