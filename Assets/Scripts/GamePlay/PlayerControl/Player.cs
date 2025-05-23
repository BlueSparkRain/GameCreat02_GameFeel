using Cinemachine;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("移动动画曲线")]
    public AnimationCurve moveCurve;
    [Header("控制色块")]
    public ColorSquare square;
    SquareController playerSquareController;

    [Header("执行操作的目标方块")]
    public GameObject targetSquare;

    [Header("方块层")]
    public LayerMask whatIsSquare;
    [Header("敌人层")]
    public LayerMask whatIsEnemy;

    #region 可操作 Info
    bool canAct = true;
    public bool CanAct=> canAct;
    float actTimer = 0;
    [Header("输入检测间隔")]
    public float actInterval = 0.25f;
    #endregion

    #region 交换 Info
    public bool isSwaping = false;
    bool canswap = true;
    float swapTimer;
    float swapInterval = 0.3f;
    #endregion

    [Header("玩家移动射线检测距离")]
    public float targetSquareChecckDistance=1.4f;

    Transform VCam;

    bool sleep;
    [HideInInspector]
    public E_CustomDir targetDir;


    public ParticleSystem P;
    public ParticleSystem N;
    public ParticleSystem G;

    private void Start()
    {
        square = GetComponent<ColorSquare>();
        playerSquareController= GetComponent<SquareController>();
        VCam = Camera.main.transform;
        //VCam = FindAnyObjectByType<CinemachineVirtualCamera>().transform;
        StartCoroutine(SleepWake());
    }
    IEnumerator SleepWake()
    {
        yield return new WaitForSeconds(2);
        sleep = false;
    }

    void Update()
    {
        if (sleep)
            return;
        //玩家操作冷却
        if (actTimer >= 0)
            actTimer -= Time.deltaTime;
        else
            canAct = true;
        //交换期间
        if (swapTimer > 0) 
          swapTimer -= Time.deltaTime;
        else 
           isSwaping = false;
        
    }

    public IEnumerator Swap(SquareController otherSquareControl)
    {
        if (!canswap || !otherSquareControl.transform.parent || otherSquareControl.transform.parent.childCount > 1 || !otherSquareControl.transform.parent.GetComponent<WalkableSlot>().isFull || !otherSquareControl.GetComponent<Square>().canMove)
        {
            Debug.Log("无法交换！");
            yield break;
        }
        if (transform.parent == null || !transform.parent.GetComponent<WalkableSlot>().isFull)
            yield break;
        if (!transform.parent || !otherSquareControl.transform.parent || !otherSquareControl.transform.parent.GetComponent<WalkableSlot>() || !transform.parent.GetComponent<WalkableSlot>())
            yield break;

        IsSwapingCheck();

        //交换音效播放
        MusicManager.Instance.PlaySound("swap", 2);


        canswap = false;
        Transform mySlot = transform.parent;
        otherSquareControl.square.HasFather = false;
        square.HasFather = false;
        transform.SetParent(otherSquareControl.transform.parent);
        otherSquareControl.transform.SetParent(mySlot);

        if (transform.parent != null && transform.parent.GetComponent<WalkableSlot>())
            playerSquareController.SquareMoveToSlot(transform.parent.position);

        if (otherSquareControl != null && mySlot != null)
        {
            otherSquareControl.SquareMoveToSlot(mySlot.position);
            StartCoroutine(otherSquareControl.square.SquareMoveAnim());
        }
        if (transform.parent != null)
        {
            StartCoroutine(PlayerMove(transform.position, transform.parent.position, 0.1f));
            if (transform.parent.GetComponent<WalkableSlot>())
            {
                WalkableSlot slot = transform.parent.GetComponent<WalkableSlot>();

                slot.transform.parent.GetComponent<SubCol>().UpdateSubColumnSquares(square, slot.transform.GetSiblingIndex());
                FindAnyObjectByType<GameMap>().UpdateRowSquares(square, slot.transform.parent.parent.GetSiblingIndex(), slot.mapIndex);
            }
            canAct = false;
            actTimer = actInterval;
            canswap = true;
            yield return square.SquareMoveAnim();
        }
    }

    IEnumerator PlayerMove(Vector3 startPos, Vector3 targetPos, float duration)
    {
        if (VCam)
            StartCoroutine(Shake());

        float timer = 0;
        while (timer <= duration)
        {
            timer += Time.unscaledDeltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, moveCurve.Evaluate(timer / duration));
            yield return null;
        }
    }
    void IsSwapingCheck()
    {
        if (isSwaping)
            swapTimer = swapInterval;
        else
        {
            swapTimer = swapInterval;  
            isSwaping = true;
        }
    }

    public void Coloration(SquareController otherSquare)
    {
        if (VCam)
            StartCoroutine(Shake());

        if (!transform.parent)
            return;
        canAct = false;

        square.myData = (otherSquare.square as ColorSquare).myData;
        playerSquareController.SquareMoveToSlot(transform.parent.position);
        MusicManager.Instance.PlaySound("coloration");

        if (transform.parent.GetComponent<WalkableSlot>())
        {
            WalkableSlot slot = transform.parent.GetComponent<WalkableSlot>();
            slot.transform.parent.GetComponent<SubCol>().UpdateSubColumnSquares(square, slot.transform.GetSiblingIndex());
            FindAnyObjectByType<GameMap>().UpdateRowSquares(square, slot.transform.parent.parent.GetSiblingIndex(), slot.mapIndex);
        }
        square.ColorSelf();
        StartCoroutine(PlayerColorationScaleAnim());
        actTimer = actInterval;
    }
    IEnumerator PlayerColorationScaleAnim()
    {
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 2.5f, 0.05f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.6f, 0.05f, val => transform.localScale = val);
    }
    IEnumerator Shake()
    {
        yield return TweenHelper.MakeLerp(Vector3.zero, new Vector3(0, 0, 0.8f), 0.08f, val => VCam.eulerAngles = val);
        yield return TweenHelper.MakeLerp(new Vector3(0, 0, 0.8f), new Vector3(0, 0, -0.8f), 0.08f, val => VCam.eulerAngles = val);
        yield return TweenHelper.MakeLerp(new Vector3(0, 0, -0.8f), Vector3.zero, 0.08f, val => VCam.eulerAngles = val);
    }
}


