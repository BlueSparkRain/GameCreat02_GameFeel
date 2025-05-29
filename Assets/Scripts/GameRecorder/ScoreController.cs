using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public PlayerScoreViewer scoreViewer;

    public float removeScoreMulti = 0.3f;
    public float chartHitScoreMulti = 0.7f;

    [Header("������ϷS�����������")]
    public int S_LevelScore = 5000;

    PlayerScoreMode scoreMode;

    public int PlayerCurrentRemoveScore => scoreMode.currentRemoveScore;
    public int PlayerCurrentChartHitScore => scoreMode.currentChartHitScore;

    int finalScore;
    //ComboController comboController;

    UIManager uiManager;
    bool gameOver = false;
    //��Ϸ��ʱ��
    float gameTimer;

    int highestStarNum;

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<int>(E_EventType.E_GetSquareRemoveScore, UpdatePlayerRemoveScore);
        EventCenter.Instance.AddEventListener<int>(E_EventType.E_GetHitChartScore, UpdatePlayerChartHitScore);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<int>(E_EventType.E_GetSquareRemoveScore, UpdatePlayerRemoveScore);
        EventCenter.Instance.RemoveEventListener<int>(E_EventType.E_GetHitChartScore, UpdatePlayerChartHitScore);
    }
    private void Awake()
    {
        //comboController = FindAnyObjectByType<ComboController>();
        scoreMode = new PlayerScoreMode();
        uiManager = UIManager.Instance;
        scoreViewer.SetSLevelStandard(S_LevelScore);
        UpdateFinalScore();
    }

    public void SelfInit(float musicDuration) 
    {
        gameTimer = 0;
        StartCoroutine(GamTimming(musicDuration));
    }

    float wholeMusicDuration;
  
    IEnumerator GamTimming(float musicDuration)
    {
        while (gameTimer <musicDuration)
        {
            gameTimer += Time.deltaTime;
            scoreViewer.SetMusicProgressFillImage(gameTimer, musicDuration);
            yield return null;
        }

        GameLevelCheckManager.Instance.EndLevel(finalScore);
    }

    

    void UpdateRemoveScore(float multi, int score)
    {
        scoreMode.GetRemoveScore(multi, score);
    }
    void UpdateChartScore(int score)
    {
        scoreMode.GetChartScore(score);
    }

    /// <summary>
    /// ����UI��ʾ-���յ÷�=a*���������÷�+b*���ĵ÷�
    /// </summary>
    /// <param name="baseRemoveScore">���������÷�</param>
    /// <param name="hitScore">���ĵ÷�</param>
    void UpdateFinalScore()
    {
        finalScore = (int)(scoreMode.currentRemoveScore * removeScoreMulti +
                     scoreMode.currentChartHitScore * chartHitScoreMulti);
        scoreViewer.SetFinalScoreText(finalScore);

        //���·���ʱͬ��������������
        scoreViewer.SetScoreLevelProgress((float)finalScore/(float)S_LevelScore);
        GameLevelCheckManager.Instance.CalculateLevel(finalScore, S_LevelScore);
    }

    public void GetCollectData()
    {
    }

    /// <summary>
    /// ������������÷ּ��ı�
    /// </summary>
    public void UpdatePlayerRemoveScore(int removescore)
    {
        UpdateRemoveScore(1, removescore);
        UpdateFinalScore();
    } 
    /// <summary>
    /// ������ҿ���÷ּ��ı�
    /// </summary>
    public void UpdatePlayerChartHitScore(int chartScore)
    {
        UpdateChartScore(chartScore);
        UpdateFinalScore();
    }
}
