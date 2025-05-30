using System.Collections;
using UnityEngine;
public enum E_CustomDir 
{
 ��,��,��,��,
}
public class Square : MonoBehaviour, ICanEffect
{
    [HideInInspector]
    protected SimpleRigibody rb;
    public bool HasFather;
    public SpriteRenderer spRender {  get; private set; }

    [Header("�����÷�")]
    public int BaseScore = 50;

    [Header("���ؿ�����ƶ�")]
    public bool canMove = true;

    [Header("���ؿ�ɴݻ�")]
    public bool canRemoved=true;

    public SquareController controller;
    public WalkableSlot slot;

    public bool isRemoving;

    [Header("��Ҫ�����Ĵ���")]
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
    /// ɫ�������������
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

    /// <summary>
    /// �ı������״̬
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
        Debug.Log(canRemoved+name + "���鱻����");
        controller.RemoveDecoratorTrigger();
        controller.ReSetDecorator();
    }

    /// <summary>
    /// ִ�з��鼼���߼����ӷ�or������
    /// </summary>
    public virtual void RemoveSelfEffect()
    {
        //���������ӷ�
        EventCenter.Instance.EventTrigger(E_EventType.E_GetSquareRemoveScore, BaseScore);

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
    /// ���ռ�������Χ������⣬�ɽ��վ���3��2.9��Ϊ�ٽ����ֵ
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
        Debug.Log("ʣ��Ѫ��"+removeTime);
        PostProcessManager.Instance.LenDistortionFlash(0,0.8f,0.06f,0.05f);
        yield return new WaitForSeconds(0.4f);
        canminusHealth = true;
    }
}

