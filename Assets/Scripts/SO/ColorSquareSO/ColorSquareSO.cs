using UnityEngine;

[CreateAssetMenu(fileName = "ColorSquareData", menuName = "MyCustomData/ColorSquareSO")]
public class ColorSquareSO : ScriptableObject
{
    public E_ColorSquareType E_Color;
    public Color SquareColor;
    public Sprite ColorSquareSprite;
}

public enum E_ColorSquareType
{
    光盘, 警告, 信件, 爱心, 耳机, 星星, 电视, 白
}