using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuCustomButtonElement : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{

    Vector3 readyPos;
    Vector3 startPos;
    //UITween uITween;
    [SerializeField] float transTime; 
    Button button;
    Animator anima;
    [SerializeField]TMP_Text text;

    void Start()
    {
        readyPos = transform.localPosition;
        startPos= readyPos+new Vector3(-1000, 0,0);
        transform.localPosition = startPos;
        button = GetComponent<Button>();
        anima = GetComponent<Animator>();   
    }

    public void SelfAppear() 
    {
       StartCoroutine(TweenHelper.MakeLerp(startPos, readyPos, transTime, val => transform.localPosition = val));       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(TweenHelper.MakeLerp(Vector3.one, Vector3.one * 1.2f, 0.1f, val => transform.localScale = val));
        if(text != null) 
        text.color = Color.yellow;
        //transform.localScale = Vector3.one * 1.2f;
        if (anima != null)
        {
            anima.SetBool("OnSelect",true);
            anima.SetBool("DisSelect",false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(TweenHelper.MakeLerp(Vector3.one * 1.2f, Vector3.one, 0.1f, val => transform.localScale = val));
        if(text != null) 
        text.color = Color.white;
        if (anima != null)
        {
            anima.SetBool("DisSelect",true);
            anima.SetBool("OnSelect",false);
            //transform.localScale = Vector3.one;
        }
    }
}
