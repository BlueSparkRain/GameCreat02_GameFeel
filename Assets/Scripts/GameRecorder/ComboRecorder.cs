using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboRecorder : MonoBehaviour
{
    [Header("���ļ��")]
    public float comboInterval=1;
    [Header("���ĳ���ʱ��")]
    public float comboDuration=0.5f;

    [Header("��ǰ������")]
    public int combo;

    [Header("��������������ʱ��")]
    public float disturbCheckDuration = 0.3f;

    [Header("����UI�ı�")]
    public TMP_Text ComboNumText;

    //���Կ�����һ�������ж�
    bool canReadNextCombo=true;

    bool canDistrub;
    
    //�������ʱ��
    float disturbTimer;

    //���������ʱ��
    float comboIntervalTimer;
    //������ȡʱ����ʱ��
    float comboDurationTimer;

    //�����һ������
    bool addCombo;

    [Header("��������")]
    public Transform ComboUIFather;

    [Header("������")]
    public GameObject BigPrefab;
    [Header("С����")]
    public GameObject LittlePrefab;

    [Header("С��������ʼ��")]
    public  Transform LeftBorn;
    [Header("С��������ʼ��")]
    public  Transform RightBorn;


    [Header("��ǰ�÷ֱ���")]
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
                Debug.Log("����+1");
                combo++;
                GetMultiplier();
                ComboNumText.text = combo.ToString();
                addCombo = false;
                //������������
                disturbTimer = disturbCheckDuration;
                canDistrub = true;
                StartCoroutine(DisturbCheck());
                yield break;
            }
            yield return null;
        }
        //Debug.Log("����+��������"); 
        combo = 0;//����������ʱʱ�䣬����������
        GetMultiplier();
        ComboNumText.text = combo.ToString();
    }

    void GetMultiplier() 
    {
        currentMulti = combo>=20 ? 2 + 0.25f * ((combo - 20) / 5) : 1 + 0.2f * (combo / 5);
    }

    IEnumerator DisturbCheck()
    {
        //Debug.Log("���������");
        while (disturbTimer >= 0)
        {
            disturbTimer -= Time.deltaTime;

            if (canDistrub  && PlayerInputManager.Instance.AnyAct)
            {
                //Debug.Log("�����+��������");
                combo = 0;//����������ʱʱ�䣬����������
                GetMultiplier();
                ComboNumText.text = combo.ToString();
                canDistrub = false;//������
                yield break;
            }

            yield return null;
        }
    }

}
