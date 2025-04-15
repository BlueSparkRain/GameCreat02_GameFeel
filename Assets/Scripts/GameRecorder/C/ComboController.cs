using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    /// <summary>
    /// ����ģ��
    /// </summary>
    PlayerComboMode comboMode;

    public  float CurrentMulti=>comboMode.currentMulti;

    [Header("������ͼ")]
    public PlayerComboViewer comboViewer;

    [Header("���ļ��")]
    public float comboInterval = 1;

    [Header("���ĳ���ʱ��")]
    public float comboDuration = 0.5f;

    [Header("��������������ʱ��")]
    public float disturbCheckDuration = 0.3f;


    [Header("��������")]
    public Transform ComboUIFather;

    [Header("������")]
    public GameObject BigPrefab;
    [Header("С����")]
    public GameObject LittlePrefab;

    [Header("С��������ʼ��")]
    public Transform LeftBorn;
    [Header("С��������ʼ��")]
    public Transform RightBorn;

    [Header("��������X")]
    public Transform MultiText;

    WaitForSeconds delay = new WaitForSeconds(2);

    //���ڽ�����������
    bool isCheckingDisturb;

    //���Կ�����һ�������ж�
    bool canReadNextCombo = true;

    bool canDistrub;

    //�������ʱ��
    float disturbTimer;

    //���������ʱ��
    float comboIntervalTimer;
    //������ȡʱ����ʱ��
    float comboDurationTimer;

    //�����һ������
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
    /// ÿ�������󣬿�������������ʱ,�ж����Ƿ������
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
    /// ����������������
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

                canDistrub = false;//������
            }
            yield return null;
        }
        isCheckingDisturb = false;
    }

    /// <summary>
    /// ���ʸ��¶���Ч��
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

