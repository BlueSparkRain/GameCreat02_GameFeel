using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class ColorSquare : Square
{
    public ColorSquareSO myData;

    SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void InitColorData(ColorSquareSO so) 
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
            yield break;
        }
        if (transform.parent != null && slot)
        {
            yield return SquareRemoveAnim();
            transform.SetParent(null);
            slot.ThrowSquare();
        }

        if (transform.GetComponent<PlayerController>())
            yield break;

        poolManager.ObjReturnPool(E_ObjectPoolType.ÑÕÉ«¿é³Ø,gameObject);
    }  
}

