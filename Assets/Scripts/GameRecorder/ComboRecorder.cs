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
                Debug.Log("����+1");
                combo++;
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
        ComboNumText.text = combo.ToString();
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
                ComboNumText.text = combo.ToString();
                canDistrub = false;//������
                yield break;
            }

            yield return null;
        }
    }

}
