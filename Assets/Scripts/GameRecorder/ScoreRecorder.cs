using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRecorder : MonoBehaviour
{
    [Header("��ҵ÷��ı�")]
    public TMP_Text scoreText;

    [Header("��ҵ�ǰ�÷�")]
    public int playerCurrentScore;
    [Header("������Ϸʱ��")]
    public float gameDuration=60;

    [Header("������Ϸʣ��ʱ��")]
    public TMP_Text gameTimerText;

    [Header("������Ϸʣ��ʱ��ʱ����")]
    public Image gameTimerFillment;


    [Header("������Ϸ�������")]
    public int GameOverScore=8000;

    bool gameOver = false;
    //��Ϸ��ʱ��
    float gameTimer;

    int highestStarNum;

    void GetStarNum(int starNum)
    {
        highestStarNum += starNum;
    }


    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<int>(E_EventType.E_GetSquareScore,UpdatePlayerScore);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<int>(E_EventType.E_GetSquareScore,UpdatePlayerScore);

    }

    public void GetCollectData() 
    {
     GetStarNum(1);
    }
    /// <summary>
    /// ������ҵ÷ּ��ı�
    /// </summary>
    public void UpdatePlayerScore(int score)
   {
        int finalScore=(int)(FindAnyObjectByType<ComboRecorder>().currentMulti *score);
        playerCurrentScore += finalScore;
        scoreText.text = playerCurrentScore.ToString();

        if (!gameOver && playerCurrentScore >= GameOverScore)
        {
            GetStarNum(2);
            gameOver = true;
            UIManager.Instance.ShowPanel<GameOverPanel>(panel => panel.GetPlayerData(gameDuration - gameTimer, playerCurrentScore,highestStarNum));
        }
   }

    private void Start()
    {
        GameStart();
    }
    public  void GameStart() 
    {
        gameTimer = gameDuration;
        StartCoroutine(Gaming());
    }
    IEnumerator Gaming() 
    {
        while (gameTimer>=0) 
        {
          gameTimer-=Time.deltaTime;
          gameTimerText.text = ((int)gameTimer).ToString();//����ʣ��ʱ���ı�

          gameTimerFillment.fillAmount = gameTimer/gameDuration;
          yield return null;
        }
        if (!gameOver) 
        {
            gameOver = true;
            UIManager.Instance.ShowPanel<GameOverPanel>(panel=>panel.LostGame(playerCurrentScore));
        }
    }
}
