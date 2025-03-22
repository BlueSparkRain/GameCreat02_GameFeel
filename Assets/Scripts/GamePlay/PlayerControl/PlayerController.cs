using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum E_ControlMode
{
    Keyboard, Gamepad, XBox
}
public enum E_TargetDir
{
    上, 下, 左, 右,
}

public class PlayerController : MonoBehaviour
{
    public AnimationCurve moveCurve;
    ColorSquare square;

    public E_ControlMode controlMode;

    [Header("执行操作的目标方块")]
    public GameObject targetSquare;

    public LayerMask whatIsSquare;
    float timer = 0;
    bool canAct = true;
    [Header("输入检测间隔")]
    public float actInterval = 0.25f;

    Rigidbody2D rb;

    public bool isSwaping=false;

    bool canswap = true;

    Transform VCam;
    void Start()
    {
        square = GetComponent<ColorSquare>();
        rb = GetComponent<Rigidbody2D>();
        VCam = FindAnyObjectByType<CinemachineVirtualCamera>().transform;
    }

    void Update()
    {
        rb.isKinematic = true;

        if (timer >= 0)
            timer -= Time.deltaTime;
        else
            canAct = true;

        if (canAct)
        {
            MoveOnce();
            ColorationOnce();
        }
    }

    GameObject CheckTarget(E_TargetDir targetDir)
    {
        switch (targetDir)
        {
            case E_TargetDir.上:
                return (Physics2D.Raycast(transform.position, Vector2.up, 2, whatIsSquare).collider?.gameObject);
            case E_TargetDir.下:
                return (Physics2D.Raycast(transform.position, Vector2.down, 2, whatIsSquare).collider?.gameObject);
            case E_TargetDir.左:
                return (Physics2D.Raycast(transform.position, Vector2.left, 2, whatIsSquare).collider?.gameObject);
            case E_TargetDir.右:
                return (Physics2D.Raycast(transform.position, Vector2.right, 2, whatIsSquare).collider?.gameObject);
            default:
                return null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 2);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * 2);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 2);
    }

    public void MoveOnce()
    {
        //if (transform.parent == null && canswap)
        if (transform.parent == null)
            return;

        if (PlayerInputManager.Instance.MoveUp)
        {
            //if (transform.parent == null|| transform.parent.parent.GetComponent<SquareColumn>().isRemoving)
            if (transform.parent == null)
                return;

            //Debug.Log("上移");
            targetSquare = CheckTarget(E_TargetDir.上);
            if (targetSquare && targetSquare.transform.parent?.GetComponent<Slot>())
                StartCoroutine(Swap(targetSquare?.GetComponent<Square>()));
        }
        else if (PlayerInputManager.Instance.MoveDown)
        {
                //if (transform.parent == null ||transform.parent.parent.GetComponent<SquareColumn>().isRemoving)
                if (transform.parent == null)
                return;
            //Debug.Log("下移");
            targetSquare = CheckTarget(E_TargetDir.下);
            if (targetSquare && targetSquare.transform.parent?.GetComponent<Slot>())
                StartCoroutine(Swap(targetSquare?.GetComponent<Square>()));
        }
        else if (PlayerInputManager.Instance.MoveLeft )
        {
                //if (transform.parent == null || transform.parent.parent.GetComponent<SquareRow>().isRemoving)
                if (transform.parent == null)
                    return;
            //Debug.Log("左移");
            targetSquare = CheckTarget(E_TargetDir.左);
            if (targetSquare && targetSquare.transform.parent?.GetComponent<Slot>())
                StartCoroutine(Swap(targetSquare?.GetComponent<Square>()));
        }
        else if (PlayerInputManager.Instance.MoveRight)
        {
                //if (transform.parent == null || transform.parent.parent.GetComponent<SquareRow>().isRemoving)
                if (transform.parent == null)
                return;
            //Debug.Log("右移");
            targetSquare = CheckTarget(E_TargetDir.右);
            if (targetSquare && targetSquare.transform.parent?.GetComponent<Slot>())
                StartCoroutine(Swap(targetSquare?.GetComponent<Square>()));
        }
    }

    public void ColorationOnce()
    {
        if (transform.parent == null)
            return;

        if (PlayerInputManager.Instance.ColorationUp)
        {
                if (transform.parent == null)
                return;
            //Debug.Log("上吸");
            targetSquare = CheckTarget(E_TargetDir.上);
            if (targetSquare?.GetComponent<ColorSquare>())
                Coloration(targetSquare?.GetComponent<ColorSquare>());
        }
        else if (PlayerInputManager.Instance.ColorationDown)
        {
                if (transform.parent == null)
                return;
            //Debug.Log("下吸");
            targetSquare = CheckTarget(E_TargetDir.下);
            if (targetSquare?.GetComponent<ColorSquare>())
                Coloration(targetSquare?.GetComponent<ColorSquare>());
        }
        else if (PlayerInputManager.Instance.ColorationLeft)
        {
                if (transform.parent == null)
                return;
            //Debug.Log("左吸");
            targetSquare = CheckTarget(E_TargetDir.左);
            if (targetSquare?.GetComponent<ColorSquare>())
                Coloration(targetSquare?.GetComponent<ColorSquare>());
        }
        else if (PlayerInputManager.Instance.ColorationRight)
        {
                if (transform.parent == null)
                return;
            //Debug.Log("右吸");
            targetSquare = CheckTarget(E_TargetDir.右);
            if (targetSquare?.GetComponent<ColorSquare>())
                Coloration(targetSquare?.GetComponent<ColorSquare>());
        }
    }


    void Coloration(ColorSquare otherSquare)
    {

        if (VCam)
            StartCoroutine(Shake());

        canAct = false;
        square.myData = otherSquare.myData;
        square.MoveToSlot(transform.parent.position);
        MusicManager.Instance.PlaySound("coloration");

        if (transform.parent.GetComponent<Slot>())
        {
            Slot slot = transform.parent.GetComponent<Slot>();
            FindAnyObjectByType<SquareGroup>().UpdateRowSquares(transform.GetComponentInChildren<Square>(), slot.transform.parent.GetSiblingIndex(), slot.transform.GetSiblingIndex());
            transform.parent.parent.GetComponent<SquareColumn>().UpdateColumnSquares(square, transform.parent.GetSiblingIndex());
        }

        square.ColorSelf();
        StartCoroutine(PlayerColorationScaleAnim());
        timer = actInterval;
    }


    public IEnumerator Swap(Square otherSquare)
    {
        if (!canswap || !otherSquare.transform.parent || otherSquare.transform.parent.childCount >1 || !otherSquare.transform.parent.GetComponent<Slot>().isFull || !otherSquare.GetComponent<Square>().canMove )
        {
            Debug.Log("无法交换！");
            yield break;
        }


        //if (transform.parent == null || !transform.parent.GetComponent<Slot>().isFull ||isSwaping)
        if (transform.parent == null || !transform.parent.GetComponent<Slot>().isFull)
            yield break;

        if (!transform.parent || !otherSquare.transform.parent || !otherSquare.transform.parent.GetComponent<Slot>() || !transform.parent.GetComponent<Slot>() )
            yield break;

        //isSwaping =true;
        StartCoroutine(IsSwapingCheck());

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
            yield return square.AnimMoveScale();
        }

    }

    float interval=0.2f;

    IEnumerator IsSwapingCheck()
    {
        if (isSwaping)
        {
            interval += 0.2f;
            yield break;
        }
            //    StopAllCoroutines();
            isSwaping = true;
        //yield return new WaitForSeconds(0.3f);
        //yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(interval);
        isSwaping = false;
    }

    IEnumerator PlayerMove(Vector3 startPos, Vector3 targetPos, float duration)
    {

        if (VCam) 
            StartCoroutine(Shake());
            //Debug.Log("手柄震动！");
        

        float timer = 0;
        while (timer <= duration)
        {
            timer += Time.unscaledDeltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, moveCurve.Evaluate(timer / duration));
            yield return null;
        }
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
