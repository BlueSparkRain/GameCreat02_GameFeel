using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreViewer : MonoBehaviour
{
    [Header("玩家得分文本")]
    public TMP_Text scoreText;

    [Header("本局游戏剩余时长")]
    public TMP_Text gameTimerText;

    [Header("本局游戏剩余时长时间条")]
    public Image gameTimerFillment;

    public void SetScoreText(int score) 
    {
        scoreText.text = score.ToString();
    }

    public void GetTimeText(int remainTime) 
    {
        gameTimerText.text =remainTime.ToString();
    }

    public  void SetFillImage( float gameTimer,float gameDuration) 
    {
        gameTimerFillment.fillAmount = gameTimer / gameDuration;
    }
}
