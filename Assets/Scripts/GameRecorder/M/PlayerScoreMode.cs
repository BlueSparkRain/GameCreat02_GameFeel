using System;
using UnityEngine;

[Serializable]
public class PlayerScoreMode
{
    /// <summary>
    /// �����÷�
    /// </summary>
    public int currentRemoveScore;

    /// <summary>
    /// ���ĵ÷�
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
