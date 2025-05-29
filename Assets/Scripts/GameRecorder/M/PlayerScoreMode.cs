using System;
using UnityEngine;

[Serializable]
public class PlayerScoreMode
{
    /// <summary>
    /// 消除得分
    /// </summary>
    public int currentRemoveScore;

    /// <summary>
    /// 卡拍得分
    /// </summary>
    public int currentChartHitScore;
    public void GetRemoveScore(float multi, int score)
    {
        currentRemoveScore += (int)(multi * score);
    } 
    public void GetChartScore(int score)
    {
        currentRemoveScore += score;
    }
}
