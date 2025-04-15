using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;

/// <summary>
/// �������������ʾ
/// </summary>
public class PlayerComboViewer : MonoBehaviour
{

    [Header("����UI�ı�")]
    public TMP_Text ComboNumText;

    [Header("��ǰ�÷ֱ�����ʾ�ı�")]
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
