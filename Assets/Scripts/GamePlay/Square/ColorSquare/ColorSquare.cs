using System.Collections;
using UnityEngine;

public class ColorSquare : Square
{
    //public int hitTime = 1;
    public ColorSquareSO myData;

    public void ColorSelf()
    {
        GetComponent<SpriteRenderer>().sprite = myData.ColorSquareSprite;
        if (transform.GetComponentInChildren<TrailRenderer>())
        transform.GetComponentInChildren<TrailRenderer>().startColor = myData.SquareColor;
    }

    protected override void Awake()
    {
        base.Awake();
    }
    
    public override IEnumerator BeRemoved()
    {
        yield return base.BeRemoved();
        EventCenter.Instance.EventTrigger(E_EventType.E_ColorSquareRemove, transform);
        if (transform.GetComponent<PlayerController>())
            yield break;

        DoSelfEffect();
        yield return AnimReMoveScale();
        //Debug.Log("色块被消除");

        if (transform.parent != null && transform.parent.GetComponent<Slot>() != null)
        {
            if (transform.parent.GetSiblingIndex() - 1 >= 0 && transform.parent.GetComponent<Slot>())
            {
                Transform upSlot = transform.parent.parent.GetChild(transform.parent.GetSiblingIndex() - 1);

                if (upSlot != null && upSlot.GetSiblingIndex() > 1 && upSlot.GetSiblingIndex() != 0)
                {
                    if (upSlot.childCount != 0)
                    {
                        yield return upSlot.GetComponent<Slot>().ThrowSquare();//上方块先松掉
                    }
                }
            }
        }

        if (transform.parent != null && transform.parent.GetComponent<Slot>())
            yield return transform.parent.GetComponent<Slot>().ThrowSquare();

        if (transform.GetComponent<PlayerController>())
            yield break;
        FindAnyObjectByType<SquareObjPool>().ReturnPool(this);
    }

  
    private void OnMouseEnter()
    {
        StartCoroutine(TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.8f, 0.1f, val => transform.localScale = val));
    }
    private void OnMouseExit()
    {
        StartCoroutine(TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.6f, 0.1f, val => transform.localScale = val));
    }

}

