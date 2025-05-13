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
    光盘,警告,信件,爱心,耳机,星星,电视,白
}