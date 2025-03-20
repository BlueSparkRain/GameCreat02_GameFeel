using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum E_ControlMode
{
    Keyboard, Gamepad, XBox
}
public enum E_TargetDir
{
    ��, ��, ��, ��,
}

public class PlayerController : MonoBehaviour
{
    public AnimationCurve moveCurve;
    ColorSquare square;

    public E_ControlMode controlMode;

    [Header("ִ�в�����Ŀ�귽��")]
    public GameObject targetSquare;

    public LayerMask whatIsSquare;
    float timer = 0;
    bool canAct = true;
    [Header("��������")]
    public float actInterval = 0.25f;

    Rigidbody2D rb;

    public bool isSwaping;


    bool canswap = true;
    void Start()
    {
        square = GetComponent<ColorSquare>();
        rb = GetComponent<Rigidbody2D>();
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
            case E_TargetDir.��:
                return (Physics2D.Raycast(transform.position, Vector2.up, 2, whatIsSquare).collider?.gameObject);
            case E_TargetDir.��:
                return (Physics2D.Raycast(transform.position, Vector2.down, 2, whatIsSquare).collider?.gameObject);
            case E_TargetDir.��:
                return (Physics2D.Raycast(transform.position, Vector2.left, 2, whatIsSquare).collider?.gameObject);
            case E_TargetDir.��:
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
        if (transform.parent == null && canswap)
            return;

        if (PlayerInputManager.Instance.MoveUp)
        {
            //if (transform.parent == null || transform.parent.parent.GetComponent<SquareColumn>().isRemoving)
                if (transform.parent == null)
                return;

            //Debug.Log("����");
            targetSquare = CheckTarget(E_TargetDir.��);
            if (targetSquare)
                StartCoroutine(Swap(targetSquare?.GetComponent<Square>()));
        }
        else if (PlayerInputManager.Instance.MoveDown)
        {
            //if (transform.parent == null || transform.parent.parent.GetComponent<SquareColumn>().isRemoving)
                if (transform.parent == null)
                    return;
            //Debug.Log("����");
            targetSquare = CheckTarget(E_TargetDir.��);
            if (targetSquare)
                StartCoroutine(Swap(targetSquare?.GetComponent<Square>()));
        }
        else if (PlayerInputManager.Instance.MoveLeft)
        {
            //if (transform.parent == null  || transform.parent.parent.GetComponent<SquareRow>().isRemoving)
                if (transform.parent == null)
                    return;
            //Debug.Log("����");
            targetSquare = CheckTarget(E_TargetDir.��);
            if (targetSquare)
                StartCoroutine(Swap(targetSquare?.GetComponent<Square>()));
        }
        else if (PlayerInputManager.Instance.MoveRight)
        {
            //if (transform.parent == null ||transform.parent.parent.GetComponent<SquareRow>().isRemoving)
                if (transform.parent == null)
                return;
            //Debug.Log("����");
            targetSquare = CheckTarget(E_TargetDir.��);
            if (targetSquare)
                StartCoroutine(Swap(targetSquare?.GetComponent<Square>()));
        }
    }

    public void ColorationOnce()
    {
        if (transform.parent == null)
            return;

        if (PlayerInputManager.Instance.ColorationUp)
        {
            //if (transform.parent == null || transform.parent.parent.GetComponent<SquareColumn>().isRemoving || transform.parent.parent.GetComponent<SquareRow>().isRemoving)
                if (transform.parent == null)
                return;
            //Debug.Log("����");
            targetSquare = CheckTarget(E_TargetDir.��);
            if (targetSquare?.GetComponent<ColorSquare>())
                Coloration(targetSquare?.GetComponent<ColorSquare>());
        }
        else if (PlayerInputManager.Instance.ColorationDown)
        {
            //if (transform.parent == null || transform.parent.parent.GetComponent<SquareColumn>().isRemoving || transform.parent.parent.GetComponent<SquareRow>().isRemoving)
                if (transform.parent == null)
                return;
            //Debug.Log("����");
            targetSquare = CheckTarget(E_TargetDir.��);
            if (targetSquare?.GetComponent<ColorSquare>())
                Coloration(targetSquare?.GetComponent<ColorSquare>());
        }
        else if (PlayerInputManager.Instance.ColorationLeft)
        {
            //if (transform.parent == null || transform.parent.parent.GetComponent<SquareColumn>().isRemoving || transform.parent.parent.GetComponent<SquareRow>().isRemoving)
                if (transform.parent == null)
                return;
            //Debug.Log("����");
            targetSquare = CheckTarget(E_TargetDir.��);
            if (targetSquare?.GetComponent<ColorSquare>())
                Coloration(targetSquare?.GetComponent<ColorSquare>());
        }
        else if (PlayerInputManager.Instance.ColorationRight)
        {
            //if (transform.parent == null || transform.parent.parent.GetComponent<SquareColumn>().isRemoving || transform.parent.parent.GetComponent<SquareRow>().isRemoving)
                if (transform.parent == null)
                return;
            //Debug.Log("����");
            targetSquare = CheckTarget(E_TargetDir.��);
            if (targetSquare?.GetComponent<ColorSquare>())
                Coloration(targetSquare?.GetComponent<ColorSquare>());
        }
    }


    void Coloration(ColorSquare otherSquare)
    {

        canAct = false;
        square.myData = otherSquare.myData;
        square.MoveToSlot(transform.parent.position);

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
        if (!canswap || !otherSquare.transform.parent || !otherSquare.transform.parent.GetComponent<Slot>().isFull || !otherSquare.GetComponent<Square>().canMove)
        {
            Debug.Log("�޷�������");
            yield break;
        }
        isSwaping=true;

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
            StartCoroutine(square.AnimMoveScale());
            //yield return new WaitForSeconds(0.1f);
            canAct = false;
            timer = actInterval;
            canswap = true;

        }

        isSwaping = true;
    }

    IEnumerator PlayerMove(Vector3 startPos, Vector3 targetPos, float duration)
    {
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
}
