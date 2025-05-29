using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreViewer : MonoBehaviour
{
    [Header("玩家当前总得分文本")]
    public TMP_Text finalScoreText;

    [Header("本局游戏剩余时长时间条")]
    public Image gameTimerFillment;

    [Header("本局游戏S评级所需分数文本")]
    public TMP_Text S_levelText;

    [Header("评级进度")]
    public Image scoreLevelImage;

    public void SetSLevelStandard(int S_LevelScore) 
    {
        S_levelText.text = S_LevelScore.ToString();
    }

    public void SetFinalScoreText(int finalScore)
    {
        finalScoreText.text = finalScore.ToString();
    }

    public void SetScoreLevelProgress(float value) 
    {
      scoreLevelImage.fillAmount= value;
    }

    public void SetMusicProgressFillImage(float gameTimer, float musicDuration)
    {
        gameTimerFillment.fillAmount = gameTimer / musicDuration;

    }
}
