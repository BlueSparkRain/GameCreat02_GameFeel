using System;
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

    [Header("音符连击")]
    public Transform ComboUIFather;

    [Header("大音符")]
    public GameObject BigPrefab;
    [Header("小音符")]
    public GameObject LittlePrefab;

    [Header("小音符左起始点")]
    public  Transform LeftBorn;
    [Header("小音符右起始点")]
    public  Transform RightBorn;


    [Header("当前得分倍率")]
    public float currentMulti;

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
            //StartCoroutine(ComboSetUp());
            canReadNextCombo = false;
            comboIntervalTimer = comboInterval;
            StartCoroutine(LittleRythmAnim());
        }

        if (!canReadNextCombo && PlayerInputManager.Instance.AnyAct)
        {
            addCombo = true;
        }
    }

    IEnumerator LittleRythmAnim() 
    {
        GameObject leftRythm=Instantiate(LittlePrefab,LeftBorn.position,Quaternion.identity, LeftBorn);
        GameObject rightRythm=Instantiate(LittlePrefab, RightBorn.position, Quaternion.identity, RightBorn);
        StartCoroutine( UITween.Instance.UIDoMove(leftRythm.transform,Vector2.zero, new Vector2(342,0),comboInterval));
        StartCoroutine( UITween.Instance.UIDoMove(rightRythm.transform,Vector2.zero, new  Vector2(-342,0), comboInterval));
        yield return  new WaitForSeconds(comboInterval);
        StartCoroutine(BigRythmAnim());
        Destroy(leftRythm);
        Destroy(rightRythm);
    }
    IEnumerator BigRythmAnim() 
    {
      GameObject bigRythm=Instantiate (BigPrefab,ComboUIFather);
      yield return TweenHelper.MakeLerp(Vector3.one,new Vector3(1.2f,0.8f,1),0.05f,val=> bigRythm.transform.localScale=val);
      StartCoroutine(ComboSetUp());
      yield return new WaitForSeconds(comboDuration);
      //comboIntervalTimer = comboInterval;

        Destroy(bigRythm);
      //yield return TweenHelper.MakeLerp(new Vector3(1.5f,0.8f,1),Vector3.one,0.25f,val=> bigRythm.transform.localScale=val);
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
                GetMultiplier();
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
        GetMultiplier();
        ComboNumText.text = combo.ToString();
    }

    void GetMultiplier() 
    {
        currentMulti = combo>=20 ? 2 + 0.25f * ((combo - 20) / 5) : 1 + 0.2f * (combo / 5);
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
                GetMultiplier();
                ComboNumText.text = combo.ToString();
                canDistrub = false;//已扰乱
                yield break;
            }

            yield return null;
        }
    }

}
