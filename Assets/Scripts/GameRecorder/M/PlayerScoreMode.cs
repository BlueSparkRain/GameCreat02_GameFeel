using System;
using UnityEngine;


[Serializable]
public class PlayerScoreMode
{
    /// <summary>
    /// 玩家当前得分
    /// </summary>
    public int playerCurrentScore;

    public void GetScore( float multi ,int score) 
    {
        playerCurrentScore += (int)(multi*score);
    }

}
