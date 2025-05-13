using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class ColorSquare : Square
{
    public ColorSquareSO myData;
    SquareObjPool pool;
    SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        pool=FindAnyObjectByType<SquareObjPool>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetColorData(ColorSquareSO so) 
    {
      myData= so;
      ColorSelf();
    }

    public void ColorSelf()
    {
        spriteRenderer.sprite = myData.ColorSquareSprite;
        if (transform.GetComponentInChildren<TrailRenderer>())
            transform.GetComponentInChildren<TrailRenderer>().startColor = myData.SquareColor;
    }
    public override IEnumerator BeRemoved()
    {
        yield return base.BeRemoved();
        EventCenter.Instance.EventTrigger(E_EventType.E_ColorSquareRemove, transform);

        if (transform.GetComponent<PlayerController>())
        {
            //yield return new WaitForSeconds(0.4f);
            //StartCoroutine(slot.WaitLoose(0.6f));
            yield break;
        }
        DoSelfEffect();
        yield return SquareReMoveAnim();

        if (transform.parent != null && slot)
        {
            transform.SetParent(null);
            slot.ThrowSquare();
        }

        if (transform.GetComponent<PlayerController>())
            yield break;

        pool.ReturnPool(this);
    }  
}

