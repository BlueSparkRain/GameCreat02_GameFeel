using UnityEngine;

[CreateAssetMenu(fileName = "SpecialSquareData", menuName = "MyCustomData/SpecialSquareSO")]

public class SpecialSquareSO : ScriptableObject
{
    [Header("���ⷽ������")]
    public E_SpecialSquareType specialType;

    [Header("���ⷽ�龫��")]
    public Sprite specialSprite;

    [TextArea]
    [Header("���ⷽ������")]
    public string description;


    [Header("���񴥷���������")]
    public int taskTriggerIndex;
}
