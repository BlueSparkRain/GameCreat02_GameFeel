using System.Collections;
using UnityEngine;

public class ColorSquare : Square
{
    public int hitTime = 1;
    public ColorSquareSO myData;

    public void ColorSelf()
    {
        GetComponent<SpriteRenderer>().sprite = myData.ColorSquareSprite;
        if (transform.GetComponentInChildren<TrailRenderer>())
        transform.GetComponentInChildren<TrailRenderer>().startColor = myData.SquareColor;
    }

    public override IEnumerator BeRemoved()
    {
        yield return base.BeRemoved();
        if (transform.GetComponent<PlayerController>())
            yield break;

        yield return AnimScaleReMove();
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

        FindAnyObjectByType<SquareObjPool>().ReturnPool(this);
    }


    public IEnumerator AnimScaleReMove()
    {
        yield return TweenHelper.MakeLerp(transform.localScale, new Vector3(2.0f, 1.0f, 1.6f), 0.05f, val => transform.localScale = val);
        yield return TweenHelper.MakeLerp(transform.localScale, Vector3.one * 1.6f, 0.05f, val => transform.localScale = val);
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

