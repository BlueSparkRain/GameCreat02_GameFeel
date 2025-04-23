using UnityEngine;

/// <summary>
/// 接收分数并进行评级检测
/// </summary>
public class GameLevelCheckManager :MonoSingleton<GameLevelCheckManager>
{
    public float baseScoreMulti=0.3f;
    public float hitScoreMulti=0.7f;

    [Header("基础消除得分")]
    public int currentBaseRemoveScore=>scoreController.PlayerCurrentScore;
    
    [Header("卡拍操作得分")]
    public int currentHitScore;

    [Header("最终得分")]
    public float finalScore;
    
    ScoreController scoreController;

    /// <summary>
    /// ScoreController是每关定制的
    /// </summary>
    public void GetCurrentScoreControl()
    {
        scoreController = FindAnyObjectByType<ScoreController>();
    }

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<int>(E_EventType.E_GetHitScore,UpdateHitScore);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<int>(E_EventType.E_GetHitScore,UpdateHitScore);
    }

    void UpdateHitScore(int hitScore) 
    {
        currentHitScore += hitScore;
    }

    /// <summary>
    /// 最终得分=0.3*基础消除得分+0.7*卡拍得分
    /// </summary>
    /// <param name="baseRemoveScore">基础消除得分</param>
    /// <param name="hitScore">卡拍得分</param>
    void GetFinalScore(int baseRemoveScore,int hitScore) 
    {
        finalScore = baseRemoveScore* baseScoreMulti + 
                     hitScore*hitScoreMulti ;
    }

    void GetFinalScaore() 
    {
    
    
    }
}
