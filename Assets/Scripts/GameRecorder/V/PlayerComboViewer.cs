using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;

/// <summary>
/// 控制连击相关显示
/// </summary>
public class PlayerComboViewer : MonoBehaviour
{

    [Header("连击UI文本")]
    public TMP_Text ComboNumText;

    [Header("当前得分倍率显示文本")]
    public TMP_Text currentMultiText;


    public void SetComboNUm(int value) 
    {
        ComboNumText.text = value.ToString();
    }

    public void SetCurrentMulti(float value) 
    {
        currentMultiText.text = value.ToString();
    }

}
