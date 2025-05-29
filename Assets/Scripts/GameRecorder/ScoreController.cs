using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public PlayerScoreViewer scoreViewer;

    public float removeScoreMulti = 0.3f;
    public float chartHitScoreMulti = 0.7f;

    [Header("本局游戏S评级所需分数")]
    public int S_LevelScore = 5000;

    PlayerScoreMode scoreMode;

    public int PlayerCurrentRemoveScore => scoreMode.currentRemoveScore;
    public int PlayerCurrentChartHitScore => scoreMode.currentChartHitScore;

    int finalScore;
    //ComboController comboController;

    UIManager uiManager;
    bool gameOver = false;
    //游戏计时器
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
    /// 更新UI显示-最终得分=a*基础消除得分+b*卡拍得分
    /// </summary>
    /// <param name="baseRemoveScore">基础消除得分</param>
    /// <param name="hitScore">卡拍得分</param>
    void UpdateFinalScore()
    {
        finalScore = (int)(scoreMode.currentRemoveScore * removeScoreMulti +
                     scoreMode.currentChartHitScore * chartHitScoreMulti);
        scoreViewer.SetFinalScoreText(finalScore);

        //更新分数时同步更新评级进度
        scoreViewer.SetScoreLevelProgress((float)finalScore/(float)S_LevelScore);
        GameLevelCheckManager.Instance.CalculateLevel(finalScore, S_LevelScore);
    }

    public void GetCollectData()
    {
    }

    /// <summary>
    /// 更新玩家消除得分及文本
    /// </summary>
    public void UpdatePlayerRemoveScore(int removescore)
    {
        UpdateRemoveScore(1, removescore);
        UpdateFinalScore();
    } 
    /// <summary>
    /// 更新玩家卡点得分及文本
    /// </summary>
    public void UpdatePlayerChartHitScore(int chartScore)
    {
        UpdateChartScore(chartScore);
        UpdateFinalScore();
    }
}
