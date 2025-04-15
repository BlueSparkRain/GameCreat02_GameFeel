using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreViewer : MonoBehaviour
{
    [Header("��ҵ÷��ı�")]
    public TMP_Text scoreText;

    [Header("������Ϸʣ��ʱ��")]
    public TMP_Text gameTimerText;

    [Header("������Ϸʣ��ʱ��ʱ����")]
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
