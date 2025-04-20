using UnityEngine;

[CreateAssetMenu(fileName = "AchievementData", menuName = "MyCustomData/AchievementSO")]
public class AchievementDataSO : ScriptableObject
{
    [Header("�ɾ�ID")]
    public int slefID;

    [Header("�ɾ�ͼʾ")]
    public Sprite achievementSprite;

    [Header("�ɾ�����")]
    [TextArea]
    public string achievementDescription;

    //�ɾʹ��
    [Header("�ɾ��Ѵ��")]
    public bool haveReach;

    /// <summary>
    /// ��ɳɾ�
    /// </summary>
    public void ReachAchievement()
    {
        haveReach = true;
        GameProfileSaveManager.Instance.SetAchievementsData(slefID);
    }
}
