using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IDragHandler
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        StartCoroutine(TweenHelper.MakeLerp(transform.localScale, Vector3.one*1.4f, 0.2f, val => transform.localScale = val));
        animator.SetTrigger("Appear");
        animator.ResetTrigger("Hide");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(TweenHelper.MakeLerp(transform.localScale, Vector3.one*1.2f, 0.2f, val => transform.localScale = val));
        animator.SetTrigger("Hide");
        animator.ResetTrigger("Appear");
    }

    public void OnDrag(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition += eventData.delta * 0.5f;
    }
}
