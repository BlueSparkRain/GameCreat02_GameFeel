using System.Collections;
using UnityEngine;

public enum E_CustomDir 
{
 ��,��,��,��,
}
public class Square : MonoBehaviour, ICanEffect
{
    protected SimpleRigibody rb;
    [HideInInspector]
    public bool HasFather;

    [Header("�����÷�")]
    public int BaseScore = 50;

    [Header("���ؿ�����ƶ�")]
    public bool canMove = true;


    protected SquareController  controller;
    public WalkableSlot slot ;//{  get; protected set; }

    public void SetSlot(WalkableSlot slot) 
    {
       this.slot = slot;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<SimpleRigibody>();
        controller=GetComponent<SquareController>();
    }
    private void Start()
    {
        //controller.PrepareSquareInit();
    }

    /// <summary>
    /// ɫ�������������
    /// </summary>
    /// <returns></returns>
    public IEnumerator SquareScaleBornAnim() 
    {
        transform.localScale = Vector3.zero;
      yield return TweenHelper.MakeLerp(Vector3.zero, new Vector3(1.4f,1.8f,1.6f), 0.05f, val => transform.localScale = val);
      yield return TweenHelper.MakeLerp(new Vector3(1.4f,1.8f,1.6f),Vector3.one *1.6f, 0.05f, val => transform.localScale = val);

  
    }

    /// <summary>
    /// ɫ���ڽ���ʱ�Ķ���
    /// </summary>
    /// <returns></returns>
    public IEnumerator SquareMoveAnim()
    {
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(2.0f, 1.0f, 1.6f), 0.05f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.6f, 0.05f, val => transform.localScale = val);
    }

    /// <summary>
    /// ɫ���ڱ�����ʱ�Ķ���
    /// </summary>
    /// <returns></returns>
    public  IEnumerator SquareReMoveAnim()
    {
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(2.5f, 0.7f, 1.6f), 0.12f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(0.8f, 2.5f, 1.6f), 0.08f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.6f, 0.06f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 2.6f, 0.04f, val => transform.localScale = val);
    }

    public virtual IEnumerator BeRemoved()
    {
        yield return null;
        Debug.Log("���鱻����");
        controller.ReSetDecorator();
    }

    /// <summary>
    /// ִ�з��鼼���߼����ӷ�or������
    /// </summary>
    public virtual void DoSelfEffect()
    {
        //���������ӷ�
        EventCenter.Instance.EventTrigger(E_EventType.E_GetSquareScore, BaseScore);

        PlayExplodeEffect();
        PlayRemoveSound();
    }

    /// <summary>
    /// ����������Ч
    /// </summary>
    void PlayRemoveSound() 
    {
        //������Ч
        FindAnyObjectByType<PitchTest>().DingDong();
    }


    /// <summary>
    /// ���ŷ�������ȥ��ʱ�ı�ըЧ��
    /// </summary>
    void PlayExplodeEffect()
    {
        if (!GetComponent<PlayerController>())
        {
            controller.SquareCreateTargetPartical(E_ParticalType.ɫ��������ը);
        }
    }
}

