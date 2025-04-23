using UnityEngine;

/// <summary>
/// ���շ����������������
/// </summary>
public class GameLevelCheckManager :MonoSingleton<GameLevelCheckManager>
{
    public float baseScoreMulti=0.3f;
    public float hitScoreMulti=0.7f;

    [Header("���������÷�")]
    public int currentBaseRemoveScore=>scoreController.PlayerCurrentScore;
    
    [Header("���Ĳ����÷�")]
    public int currentHitScore;

    [Header("���յ÷�")]
    public float finalScore;
    
    ScoreController scoreController;

    /// <summary>
    /// ScoreController��ÿ�ض��Ƶ�
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
    /// ���յ÷�=0.3*���������÷�+0.7*���ĵ÷�
    /// </summary>
    /// <param name="baseRemoveScore">���������÷�</param>
    /// <param name="hitScore">���ĵ÷�</param>
    void GetFinalScore(int baseRemoveScore,int hitScore) 
    {
        finalScore = baseRemoveScore* baseScoreMulti + 
                     hitScore*hitScoreMulti ;
    }

    void GetFinalScaore() 
    {
    
    
    }
}
