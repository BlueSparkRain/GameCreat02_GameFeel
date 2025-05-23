using System.Collections;
using UnityEngine;

public enum E_CustomDir 
{
 上,下,左,右,
}
public class Square : MonoBehaviour, ICanEffect
{
    protected SimpleRigibody rb;
    [HideInInspector]
    public bool HasFather;

    [Header("基础得分")]
    public int BaseScore = 50;

    [Header("本地块可以移动")]
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
    /// 色块放缩出生动画
    /// </summary>
    /// <returns></returns>
    public IEnumerator SquareScaleBornAnim() 
    {
      transform.localScale = Vector3.zero;
      yield return TweenHelper.MakeLerp(Vector3.zero, new Vector3(1.4f,1.8f,1.6f), 0.05f, val => transform.localScale = val);
      yield return TweenHelper.MakeLerp(new Vector3(1.4f,1.8f,1.6f),Vector3.one *1.6f, 0.05f, val => transform.localScale = val);

    }

    /// <summary>
    /// 色块在交换时的动画
    /// </summary>
    /// <returns></returns>
    public IEnumerator SquareMoveAnim()
    {
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(2.0f, 1.0f, 1.6f), 0.05f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.6f, 0.05f, val => transform.localScale = val);
    }

    /// <summary>
    /// 色块在被消除时的动画
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
        Debug.Log("方块被消除");
        controller.RemoveDecoratorTrigger();
        controller.ReSetDecorator();
    }

    /// <summary>
    /// 执行方块技能逻辑（加分or其他）
    /// </summary>
    public virtual void RemoveSelfEffect()
    {
        //基础消除加分
        EventCenter.Instance.EventTrigger(E_EventType.E_GetSquareScore, BaseScore);

        PlayExplodeEffect();
        PlayRemoveSound();
    }

    /// <summary>
    /// 播放消除音效
    /// </summary>
    void PlayRemoveSound() 
    {
        //消除音效
        FindAnyObjectByType<PitchTest>().DingDong();
    }

    /// <summary>
    /// 播放方块自身被去除时的爆炸效果
    /// </summary>
    void PlayExplodeEffect()
    {
        if (!GetComponent<PlayerController>())
        {
            controller.SquareCreateTargetPartical(E_ParticalType.色块消除爆炸);
        }
    }
}

