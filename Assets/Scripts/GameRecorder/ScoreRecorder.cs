using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRecorder : MonoBehaviour
{
    [Header("玩家得分文本")]
    public TMP_Text scoreText;

    [Header("玩家当前得分")]
    public int playerCurrentScore;
    [Header("本局游戏时长")]
    public float gameDuration=60;

    [Header("本局游戏剩余时长")]
    public TMP_Text gameTimerText;

    [Header("本局游戏剩余时长时间条")]
    public Image gameTimerFillment;


    [Header("本局游戏所需分数")]
    public int GameOverScore=8000;

    bool gameOver = false;


    //游戏计时器
    float gameTimer;

    /// <summary>
    /// 更新玩家得分及文本
    /// </summary>
   public void UpdatePlayerScore(int score)
   {
        playerCurrentScore += score;
        scoreText.text = playerCurrentScore.ToString();

        if (!gameOver && playerCurrentScore >= GameOverScore)
        {
            gameOver = true;
            UIManager.Instance.ShowPanel<GameOverPanel>(panel => panel.GetData(gameDuration - gameTimer, playerCurrentScore));
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
          gameTimerText.text = ((int)gameTimer).ToString();//更新剩余时间文本

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
