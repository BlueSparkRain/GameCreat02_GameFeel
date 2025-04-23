using Cinemachine;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public AnimationCurve moveCurve;
    public ColorSquare square;

    [Header("执行操作的目标方块")]
    public GameObject targetSquare;

    public LayerMask whatIsSquare;
    float timer = 0;
    bool canAct = true;

    public bool CanAct=> canAct;
    [Header("输入检测间隔")]
    public float actInterval = 0.25f;

    [Header("玩家检测距离")]
    public float targetSquareChecckDistance=1.2f;

    public bool isSwaping = false;

    bool canswap = true;

    float interval = 0.2f;

    Transform VCam;

    bool sleep;
    [HideInInspector]
    public E_TargetDir targetDir;


    private void Start()
    {
        square = GetComponent<ColorSquare>();
        VCam = FindAnyObjectByType<CinemachineVirtualCamera>().transform;
        StartCoroutine(SleepWake());
    }
    IEnumerator SleepWake()
    {
        yield return new WaitForSeconds(2);
        sleep = false;
    }

    float swapTimer;
    float swapInterval=0.3f;

    void Update()
    {
        if (sleep)
            return;
        if (timer >= 0)
            timer -= Time.deltaTime;
        else
            canAct = true;

        if (swapTimer > 0) 
        {
          swapTimer -= Time.deltaTime;
        }
        else 
        {
           isSwaping = false;
        }

    }

    /// <summary>
    /// 获取目标方向的方块对象
    /// </summary>
    /// <param name="targetDir"></param>
    /// <returns></returns>
    public GameObject CheckTarget(E_TargetDir targetDir)
    {
        switch (targetDir)
        {
            case E_TargetDir.上:
                return (Physics2D.Raycast(transform.position, Vector2.up, targetSquareChecckDistance, whatIsSquare).collider?.gameObject);
            case E_TargetDir.下:
                return (Physics2D.Raycast(transform.position, Vector2.down, targetSquareChecckDistance, whatIsSquare).collider?.gameObject);
            case E_TargetDir.左:
                return (Physics2D.Raycast(transform.position, Vector2.left, targetSquareChecckDistance, whatIsSquare).collider?.gameObject);
            case E_TargetDir.右:
                return (Physics2D.Raycast(transform.position, Vector2.right, targetSquareChecckDistance, whatIsSquare).collider?.gameObject);
            default:
                return null;
        }
    }

    public IEnumerator Swap(Square otherSquare)
    {
        if (!canswap || !otherSquare.transform.parent || otherSquare.transform.parent.childCount > 1 || !otherSquare.transform.parent.GetComponent<Slot>().isFull || !otherSquare.GetComponent<Square>().canMove)
        {
            Debug.Log("无法交换！");
            yield break;
        }
        if (transform.parent == null || !transform.parent.GetComponent<Slot>().isFull)
            yield break;

        if (!transform.parent || !otherSquare.transform.parent || !otherSquare.transform.parent.GetComponent<Slot>() || !transform.parent.GetComponent<Slot>())
            yield break;

        IsSwapingCheck();

        //交换音效播放
        MusicManager.Instance.PlaySound("swap", 2);


        canswap = false;
        Transform mySlot = transform.parent;
        otherSquare.HasFather = false;
        square.HasFather = false;
        transform.SetParent(otherSquare.transform.parent);
        otherSquare.transform.SetParent(mySlot);

        if (transform.parent != null && transform.parent.GetComponent<Slot>())
            square.MoveToSlot(transform.parent.position);

        if (otherSquare != null && mySlot != null)
        {
            otherSquare.MoveToSlot(mySlot.position);
        }
        if (transform.parent != null)
        {
            StartCoroutine(PlayerMove(transform.position, transform.parent.position, 0.1f));
            if (transform.parent.GetComponent<Slot>())
            {
                Slot slot = transform.parent.GetComponent<Slot>();
                slot.transform.parent.GetComponent<SquareColumn>().UpdateColumnSquares(square, slot.transform.GetSiblingIndex());
                FindAnyObjectByType<SquareGroup>().UpdateRowSquares(transform.GetComponentInChildren<Square>(), slot.transform.parent.GetSiblingIndex(), slot.transform.GetSiblingIndex());
            }
            canAct = false;
            timer = actInterval;
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
        {
            swapTimer = swapInterval;
        }
        else
        {
            swapTimer = swapInterval;  
            isSwaping = true;
        }
    }


    public void Coloration(ColorSquare otherSquare)
    {
        if (VCam)
            StartCoroutine(Shake());

        if (!transform.parent)
            return;
        canAct = false;

        square.myData = otherSquare.myData;
        square.MoveToSlot(transform.parent.position);
        MusicManager.Instance.PlaySound("coloration");

        if (transform.parent.GetComponent<Slot>())
        {
            Slot slot = transform.parent.GetComponent<Slot>();
            slot.transform.parent.GetComponent<SquareColumn>().UpdateColumnSquares(square, slot.transform.GetSiblingIndex());
            FindAnyObjectByType<SquareGroup>().UpdateRowSquares(transform.GetComponentInChildren<Square>(), slot.transform.parent.GetSiblingIndex(), slot.transform.GetSiblingIndex());
        }
        square.ColorSelf();
        StartCoroutine(PlayerColorationScaleAnim());
        timer = actInterval;
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

public enum E_TargetDir
{
    上, 下, 左, 右,
}
