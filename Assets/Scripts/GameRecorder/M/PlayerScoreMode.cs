using System;
using UnityEngine;


[Serializable]
public class PlayerScoreMode
{
    /// <summary>
    /// ��ҵ�ǰ�÷�
    /// </summary>
    public int playerCurrentScore;

    public void GetScore( float multi ,int score) 
    {
        playerCurrentScore += (int)(multi*score);
    }

}
