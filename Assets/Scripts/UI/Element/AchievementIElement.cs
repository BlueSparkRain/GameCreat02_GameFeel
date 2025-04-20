using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用于读取并显示成就条目
/// </summary>
public class AchievementIElement : MonoBehaviour
{
    [Header("成就描述文本")]
    public TMP_Text achievementDescription;
    [Header("成就图例")]
    public Image achievementImage;
    [Header("成就SO数据")]
    public AchievementDataSO currentAchievementData;

    [Header("成就达成图示")]
    public Image reachImage;
    
  

    /// <summary>
    /// 根据成就数据更新UI显示
    /// </summary>
    /// <param name="data"></param>
    public void UpdateAchievementData(AchievementDataSO data) 
    {
        currentAchievementData=data;
        achievementImage.sprite=achievementImage.sprite;
        achievementDescription.text=currentAchievementData.achievementDescription;
    }
}
