using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreViewer : MonoBehaviour
{
    [Header("��ҵ�ǰ�ܵ÷��ı�")]
    public TMP_Text finalScoreText;

    [Header("������Ϸʣ��ʱ��ʱ����")]
    public Image gameTimerFillment;

    [Header("������ϷS������������ı�")]
    public TMP_Text S_levelText;

    [Header("��������")]
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
