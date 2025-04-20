using UnityEngine;

[CreateAssetMenu(fileName = "AchievementData", menuName = "MyCustomData/AchievementSO")]
public class AchievementDataSO : ScriptableObject
{
    [Header("成就ID")]
    public int slefID;

    [Header("成就图示")]
    public Sprite achievementSprite;

    [Header("成就描述")]
    [TextArea]
    public string achievementDescription;

    //成就达成
    [Header("成就已达成")]
    public bool haveReach;

    /// <summary>
    /// 达成成就
    /// </summary>
    public void ReachAchievement()
    {
        haveReach = true;
        GameProfileSaveManager.Instance.SetAchievementsData(slefID);
    }
}
