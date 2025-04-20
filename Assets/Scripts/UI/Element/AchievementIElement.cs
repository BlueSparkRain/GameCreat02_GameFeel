using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���ڶ�ȡ����ʾ�ɾ���Ŀ
/// </summary>
public class AchievementIElement : MonoBehaviour
{
    [Header("�ɾ������ı�")]
    public TMP_Text achievementDescription;
    [Header("�ɾ�ͼ��")]
    public Image achievementImage;
    [Header("�ɾ�SO����")]
    public AchievementDataSO currentAchievementData;

    [Header("�ɾʹ��ͼʾ")]
    public Image reachImage;
    
  

    /// <summary>
    /// ���ݳɾ����ݸ���UI��ʾ
    /// </summary>
    /// <param name="data"></param>
    public void UpdateAchievementData(AchievementDataSO data) 
    {
        currentAchievementData=data;
        achievementImage.sprite=achievementImage.sprite;
        achievementDescription.text=currentAchievementData.achievementDescription;
    }
}
