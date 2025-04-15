using System;
using System.Collections;
using TMPro;
using UnityEngine;

[Serializable]
public class PlayerComboMode
{

    /// <summary>
    /// 当前连击数
    /// </summary>
    public int combo;

    [Header("当前得分倍率")]
    public float currentMulti;


    /// <summary>
    /// 增加连击数
    /// </summary>
    public void GetCombo() 
    {
        combo++;
    }

    /// <summary>
    /// 连击数归零1
    /// </summary>
    public void ReSetCombo() 
    {
      combo=0;
    }


    public void GetMultiplier() 
    {
        currentMulti = combo >= 20 ? 2 + 0.25f * ((combo - 20) / 5) : 1 + 0.2f * (combo / 5);
    }


 

}
