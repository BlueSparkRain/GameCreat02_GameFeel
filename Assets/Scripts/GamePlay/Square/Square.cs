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


    public SquareController controller;
    public WalkableSlot slot ;

    public bool isRemoving;

    public void SetSlot(WalkableSlot slot) 
    {
       this.slot = slot;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<SimpleRigibody>();
        controller=GetComponent<SquareController>();
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
    public  IEnumerator SquareRemoveAnim()
    {
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(2.5f, 0.7f, 1.6f), 0.10f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(0.8f, 2.5f, 1.6f), 0.06f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.6f, 0.05f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 2.2f, 0.03f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 0.45f, 0.02f, val => transform.localScale = val);
    }

    public virtual IEnumerator BeRemoved()
    { 
        yield return null;
        RemoveSelfEffect();
        Debug.Log("���鱻����");
        controller.RemoveDecoratorTrigger();
        controller.ReSetDecorator();
    }

    /// <summary>
    /// ִ�з��鼼���߼����ӷ�or������
    /// </summary>
    public virtual void RemoveSelfEffect()
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

