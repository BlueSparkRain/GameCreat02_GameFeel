using UnityEngine;

[CreateAssetMenu(fileName = "SpecialSquareData", menuName = "MyCustomData/SpecialSquareSO")]

public class SpecialSquareSO : ScriptableObject
{
    [Header("特殊方块类型")]
    public E_SpecialSquareType specialType;

    [Header("特殊方块精灵")]
    public Sprite specialSprite;

    [TextArea]
    [Header("特殊方块描述")]
    public string description;


    [Header("任务触发方块批次")]
    public int taskTriggerIndex;
}
