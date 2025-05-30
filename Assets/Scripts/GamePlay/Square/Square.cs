using System.Collections;
using UnityEngine;
public enum E_CustomDir 
{
 上,下,左,右,
}
public class Square : MonoBehaviour, ICanEffect
{
    [HideInInspector]
    protected SimpleRigibody rb;
    public bool HasFather;
    public SpriteRenderer spRender {  get; private set; }

    [Header("基础得分")]
    public int BaseScore = 50;

    [Header("本地块可以移动")]
    public bool canMove = true;

    [Header("本地块可摧毁")]
    public bool canRemoved=true;

    public SquareController controller;
    public WalkableSlot slot;

    public bool isRemoving;

    [Header("需要消除的次数")]
    public int removeTime = 1;

    bool isBoss;
    public void SetBoss() 
    {
        isBoss = true;
        removeTime = 5;
        StartCoroutine(TurnBossAnim());
        GetComponent<ColorSquare>().myData = null;
        //spRender.color = Color.white;
    }

    IEnumerator TurnBossAnim() 
    {
        yield return TweenHelper.MakeLerp(transform.localScale,new Vector3(transform.localScale.x,0,transform.localScale.z), 0.05f, val => transform.localScale = val);
        spRender.sprite = Resources.Load<Sprite>("Sprites/Boss");
        yield return TweenHelper.MakeLerp(transform.localScale,Vector3.one*1.56f, 0.05f, val => transform.localScale = val);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    protected WholeObjPoolManager  poolManager;

    public void SetSlot(WalkableSlot slot) 
    {
       this.slot = slot;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<SimpleRigibody>();
        spRender=GetComponent<SpriteRenderer>();
        controller =GetComponent<SquareController>();
        poolManager = WholeObjPoolManager.Instance;
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

    public IEnumerator SquareToEnemyAnim() 
    {
        yield return TweenHelper.MakeLerp(Vector3.one, new Vector3(255, 0, 0), 0.5f, val => spRender.color = new Color(val.x,val.y,val.z,255));
     
        yield return TweenHelper.MakeLerp(new Vector3(255, 0, 0), Vector3.one, 0.4f, val => spRender.color = new Color(val.x,val.y,val.z,255));
        
        yield return TweenHelper.MakeLerp(Vector3.one, new Vector3(255, 0, 0), 0.3f, val => spRender.color = new Color(val.x,val.y,val.z,255));
        
        yield return TweenHelper.MakeLerp(new Vector3(255, 0, 0), Vector3.one, 0.2f, val => spRender.color = new Color(val.x,val.y,val.z,255));
        
        yield return TweenHelper.MakeLerp(Vector3.one, new Vector3(255, 0, 0), 0.1f, val => spRender.color = new Color(val.x,val.y,val.z,255));
       
        if(isBoss)
        yield return TweenHelper.MakeLerp(new Vector3(255, 0, 0), Vector3.one, 0.1f, val => spRender.color = new Color(val.x,val.y,val.z,255));

        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(2.5f, 0.7f, 1.6f), 0.10f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(0.8f, 2.5f, 1.6f), 0.06f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.6f, 0.05f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 2.2f, 0.03f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.56f, 0.02f, val => transform.localScale = val);
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

    /// <summary>
    /// 改变可消除状态
    /// </summary>
    /// <param name="canRemove"></param>
    protected void SetSquareRemovable(bool canRemove) 
    {
         canRemoved = canRemove;
    }

    public virtual IEnumerator BeRemoved()
    { 
        yield return null;
        isBoss = false;
        canminusHealth = true;
        removeTime = 1;
        RemoveSelfEffect();
        Debug.Log(canRemoved+name + "方块被消除");
        controller.RemoveDecoratorTrigger();
        controller.ReSetDecorator();
    }

    /// <summary>
    /// 执行方块技能逻辑（加分or其他）
    /// </summary>
    public virtual void RemoveSelfEffect()
    {
        //基础消除加分
        EventCenter.Instance.EventTrigger(E_EventType.E_GetSquareRemoveScore, BaseScore);

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


    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener<Transform>(E_EventType.E_ColorSquareRemove, CheckHealth);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener<Transform>(E_EventType.E_ColorSquareRemove, CheckHealth);
    }

    public bool canminusHealth = true;

    /// <summary>
    /// 可收集方块周围消除检测，可接收距离3（2.9）为临界最大值
    /// </summary>
    /// <param name="square"></param>
    void CheckHealth(Transform square)
    {
        if (!isBoss)
            return;

        if (canminusHealth &&  Vector2.Distance(square.position, transform.position) <= 1)
        {
            StartCoroutine(AeraCheck());
            if (removeTime <= 0)
            {
                canminusHealth = false;
                StartCoroutine(BeRemoved());
                EventCenter.Instance.EventTrigger(E_EventType.E_KillABoss);
            }
        }
    }

    IEnumerator AeraCheck()
    {
        canminusHealth = false;
        removeTime -= 1;
        Debug.Log("剩余血量"+removeTime);
        PostProcessManager.Instance.LenDistortionFlash(0,0.8f,0.06f,0.05f);
        yield return new WaitForSeconds(0.4f);
        canminusHealth = true;
    }
}

