using System;
using System.Collections;
using TMPro;
using UnityEngine;

[Serializable]
public class PlayerComboMode
{

    /// <summary>
    /// ��ǰ������
    /// </summary>
    public int combo;

    [Header("��ǰ�÷ֱ���")]
    public float currentMulti;


    /// <summary>
    /// ����������
    /// </summary>
    public void GetCombo() 
    {
        combo++;
    }

    /// <summary>
    /// ����������1
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
