using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    /// <summary>
    /// 连击模型
    /// </summary>
    PlayerComboMode comboMode;

    public  float CurrentMulti=>comboMode.currentMulti;

    [Header("连击视图")]
    public PlayerComboViewer comboViewer;

    [Header("节拍间隔")]
    public float comboInterval = 1;

    [Header("节拍持续时间")]
    public float comboDuration = 0.5f;

    [Header("连击后误操作检测时长")]
    public float disturbCheckDuration = 0.3f;


    [Header("音符连击")]
    public Transform ComboUIFather;

    [Header("大音符")]
    public GameObject BigPrefab;
    [Header("小音符")]
    public GameObject LittlePrefab;

    [Header("小音符左起始点")]
    public Transform LeftBorn;
    [Header("小音符右起始点")]
    public Transform RightBorn;

    [Header("倍率物体X")]
    public Transform MultiText;

    WaitForSeconds delay = new WaitForSeconds(2);

    //正在进行误操作检测
    bool isCheckingDisturb;

    //可以开启下一次连击判定
    bool canReadNextCombo = true;

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
        comboMode=new PlayerComboMode();

        comboIntervalTimer = comboInterval;
        comboViewer.ComboNumText.text = "0";
        comboDurationTimer = comboDuration;
        StartCoroutine(ComboSetUp());
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
            StartCoroutine(LittleRythmAnim());
        }

        if (canReadNextCombo)
        {
            canReadNextCombo = false;
            comboIntervalTimer = comboInterval;
        }

        if (!canReadNextCombo && PlayerInputManager.Instance.AnyAct)
        {
            addCombo = true;
        }
    }

    IEnumerator LittleRythmAnim()
    {
        GameObject leftRythm = Instantiate(LittlePrefab, LeftBorn.position, Quaternion.identity, LeftBorn);
        GameObject rightRythm = Instantiate(LittlePrefab, RightBorn.position, Quaternion.identity, RightBorn);
        StartCoroutine(UITween.Instance.UIDoMove(leftRythm.transform, Vector2.zero, new Vector2(342, 0), comboInterval));
        StartCoroutine(UITween.Instance.UIDoMove(rightRythm.transform, Vector2.zero, new Vector2(-342, 0), comboInterval));
        StartCoroutine(BigRythmAnim());
        yield return new WaitForSeconds(comboInterval);
        Destroy(leftRythm);
        Destroy(rightRythm);
    }
    IEnumerator BigRythmAnim()
    {
        GameObject bigRythm = Instantiate(BigPrefab, ComboUIFather);
        yield return TweenHelper.MakeLerp(Vector3.one, new Vector3(1.2f, 0.8f, 1), 0.05f, val => bigRythm.transform.localScale = val);
        comboDurationTimer = comboDuration;
        StartCoroutine(ComboSetUp());
        yield return new WaitForSeconds(comboDuration);

        Destroy(bigRythm);
    }

    IEnumerator ComboSetUp()
    {
        while (comboDurationTimer >= 0)
        {
            comboDurationTimer -= Time.deltaTime;
            if (addCombo)
            {
                GetCombo();
                StartCoroutine(NextEmptyComboCheck());
                GetMultiplier();
                StartCoroutine(MultiTextShakeAnim());
                addCombo = false;
            }
            yield return null;
        }
        StartCoroutine(DisturbCheck());
    }

    /// <summary>
    /// 每次连击后，开启连续连击计时,判定看是否空连击
    /// </summary>
    /// <returns></returns>
    IEnumerator NextEmptyComboCheck() 
    {
        int nextTarget = comboMode.combo + 1;
        yield return  delay;
        if (comboMode.combo < nextTarget)
        {
            ResetCombo();
        }
    }

    /// <summary>
    /// 得连击后误操作检测
    /// </summary>
    /// <returns></returns>
    IEnumerator DisturbCheck()
    {
        if (isCheckingDisturb)
            yield break;
        isCheckingDisturb = true;
        disturbTimer = disturbCheckDuration;
        canDistrub = true;
        while (disturbTimer >= 0)
        {
            disturbTimer -= Time.deltaTime;

            if (canDistrub && PlayerInputManager.Instance.AnyAct)
            {
                ResetCombo();
            
                GetMultiplier();

                canDistrub = false;//已扰乱
            }
            yield return null;
        }
        isCheckingDisturb = false;
    }

    /// <summary>
    /// 倍率更新动画效果
    /// </summary>
    /// <returns></returns>
    IEnumerator MultiTextShakeAnim()
    {
        yield return TweenHelper.MakeLerp(Vector3.zero, new Vector3(0, 0, 7.8f), 0.03f, val => MultiText.eulerAngles = val);
        StartCoroutine(TweenHelper.MakeLerp(Vector3.one, Vector3.one * comboMode.currentMulti, 0.04f, val => MultiText.localScale = val));
        yield return TweenHelper.MakeLerp(new Vector3(0, 0, -7.8f), new Vector3(0, 0, 7.8f), 0.04f, val => MultiText.eulerAngles = val);
        yield return TweenHelper.MakeLerp(Vector3.one * comboMode.currentMulti, Vector3.one, 0.06f, val => MultiText.localScale = val);
        yield return TweenHelper.MakeLerp(new Vector3(0, 0, 7.8f), Vector3.zero, 0.03f, val => MultiText.eulerAngles = val);

    }

    void GetMultiplier()
    {
        comboMode.GetMultiplier();
        comboViewer.SetCurrentMulti(comboMode.currentMulti);
    }

    void GetCombo()
    {
        comboMode.GetCombo();
        comboViewer.SetComboNUm(comboMode.combo);
    }

    void ResetCombo()
    {
        comboMode.ReSetCombo();
        comboViewer.SetComboNUm(comboMode.combo);
    }
}

