using System.Collections;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public PlayerScoreViewer scoreViewer;

    [Header("������Ϸʱ��")]
    public float gameDuration = 60;

    [Header("������Ϸ�������")]
    public int GameOverScore = 4000;

    PlayerScoreMode scoreMode;
    public int PlayerCurrentScore => scoreMode.playerCurrentScore;

    ComboController comboController;

    UIManager uiManager;
    bool gameOver = false;
    //��Ϸ��ʱ��
    float gameTimer;

    int highestStarNum;

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<int>(E_EventType.E_GetSquareScore, UpdatePlayerScore);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<int>(E_EventType.E_GetSquareScore, UpdatePlayerScore);
    }

    private void Start()
    {
        scoreMode = new PlayerScoreMode();
        comboController = FindAnyObjectByType<ComboController>();
        uiManager = UIManager.Instance;
        GameStart();
    }
    public void GameStart()
    {
        gameTimer = gameDuration;
        StartCoroutine(Gaming());
    }
    IEnumerator Gaming()
    {
        while (gameTimer >= 0)
        {
            gameTimer -= Time.deltaTime;
            scoreViewer.GetTimeText((int)gameTimer);
            scoreViewer.SetFillImage(gameTimer, gameDuration);

            yield return null;
        }
        if (!gameOver)
        {
            gameOver = true;
            uiManager.ShowPanel<GameOverPanel>(panel => panel.LostGame(PlayerCurrentScore));
        }
    }


    void UpdateScore(float multi, int score)
    {
        scoreMode.GetScore(multi, score);
        scoreViewer.SetScoreText(scoreMode.playerCurrentScore);
    }

    public void GetCollectData()
    {
        GetStarNum(1);
    }


    void GetStarNum(int starNum)
    {
        highestStarNum += starNum;
    }

    /// <summary>
    /// ������ҵ÷ּ��ı�
    /// </summary>
    public void UpdatePlayerScore(int score)
    {
        UpdateScore(comboController.CurrentMulti, score);
        if (!gameOver && PlayerCurrentScore >= GameOverScore)
        {
            GetStarNum(2);
            gameOver = true;
            uiManager.ShowPanel<GameOverPanel>(panel => panel.GetPlayerData(gameDuration - gameTimer, PlayerCurrentScore, highestStarNum));
        }
    }
}
