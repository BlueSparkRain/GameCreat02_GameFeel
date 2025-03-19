using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboRecorder : MonoBehaviour
{
    [Header("节拍间隔")]
    public float comboInterval=1;
    [Header("节拍持续时间")]
    public float comboDuration=0.5f;

    [Header("当前连击数")]
    public int combo;

    [Header("连击后误操作检测时长")]
    public float disturbCheckDuration = 0.3f;

    [Header("连击UI文本")]
    public TMP_Text ComboNumText;

    //可以开启下一次连击判定
    bool canReadNextCombo=true;

    bool canDistrub;
    
    //误操作计时器
    float disturbTimer;

    //连击间隔计时器
    float comboIntervalTimer;
    //连击读取时长计时器
    float comboDurationTimer;

    //可添加一次连击
    bool addCombo;


    void Start()
    {
         comboIntervalTimer= comboInterval;
         ComboNumText.text ="0";
    }

    void Update()
    {
        if (comboIntervalTimer > 0)
        {
           comboIntervalTimer -= Time.deltaTime;
        }
        else
        {
            canReadNextCombo = true;
        }


        if (canReadNextCombo) 
        {
            comboDurationTimer=comboDuration;
            StartCoroutine(ComboSetUp());
            canReadNextCombo = false;
            //canTriggerNextCombo=false;
            comboIntervalTimer = comboInterval;
        }

        if (!canReadNextCombo && PlayerInputManager.Instance.AnyAct)
        {
            addCombo = true;
        }
    }

    IEnumerator ComboSetUp()
    {
        while (comboDurationTimer >= 0) 
        {
            comboDurationTimer -= Time.deltaTime;
            if (addCombo)
            {
                Debug.Log("连击+1");
                combo++;
                ComboNumText.text = combo.ToString();
                addCombo = false;
                //开启误操作检测
                disturbTimer = disturbCheckDuration;
                canDistrub = true;
                StartCoroutine(DisturbCheck());
                yield break;
            }
            yield return null;
        }
        //Debug.Log("空拍+重置连击"); 
        combo = 0;//超出连击计时时间，重置连击数
        ComboNumText.text = combo.ToString();
    }

    IEnumerator DisturbCheck()
    {
        //Debug.Log("检测误输入");
        while (disturbTimer >= 0)
        {
            disturbTimer -= Time.deltaTime;

            if (canDistrub  && PlayerInputManager.Instance.AnyAct)
            {
                //Debug.Log("误操作+重置连击");
                combo = 0;//超出连击计时时间，重置连击数
                ComboNumText.text = combo.ToString();
                canDistrub = false;//已扰乱
                yield break;
            }

            yield return null;
        }
    }

}
