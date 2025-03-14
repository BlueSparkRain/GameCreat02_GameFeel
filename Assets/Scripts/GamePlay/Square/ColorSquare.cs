using UnityEngine;

public class ColorSquare : Square
{
    public int hitTime = 1;

    protected override void BeRemoved()
    {
        base.BeRemoved();
        Debug.Log("É«¿é±»ÒÆ³ý");
        transform.parent.GetComponent<Slot>()?.ThrowSquare();
    }

    private void OnMouseEnter()
    {
        StartCoroutine(TweenHelper.MakeLerp(transform.localScale, new Vector3(1.2f, 1.2f, 1.2f), 0.1f, val => transform.localScale = val));
    }
    private void OnMouseExit()
    {
        StartCoroutine(TweenHelper.MakeLerp(transform.localScale, Vector3.one, 0.1f, val => transform.localScale = val));
    }
    private void OnMouseDown()
    {
        BeRemoved();
        DestroyImmediate(transform.gameObject);
    }
}

