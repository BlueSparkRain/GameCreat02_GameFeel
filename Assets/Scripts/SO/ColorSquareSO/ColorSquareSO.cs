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
    ����, ����, �ż�, ����, ����, ����, ����, ��
}