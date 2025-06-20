using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuCustomButtonElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 readyPos;
    Vector3 startPos;
    [SerializeField] float transTime;
    Animator anima;
    [SerializeField] TMP_Text text;
   

    private void Awake()
    {
        readyPos = transform.localPosition;
        startPos = readyPos + new Vector3(-1000, 0, 0);
        transform.localPosition = startPos;
        anima = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    public void InitSelfPos() 
    {
    transform.localPosition = startPos;
    }
    public void SelfAppear()
    {

        StartCoroutine(TweenHelper.MakeLerp(startPos, readyPos, transTime, val => transform.localPosition = val));
    }

    public void SelfHide()
    {

        StartCoroutine(TweenHelper.MakeLerp(readyPos, startPos, transTime, val => transform.localPosition = val));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(TweenHelper.MakeLerp(Vector3.one, Vector3.one * 1.2f, 0.1f, val => transform.localScale = val));
        if (text != null)
            text.color = Color.yellow;
        if (anima != null)
        {
            anima.SetBool("OnSelect", true);
            anima.SetBool("DisSelect", false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(TweenHelper.MakeLerp(Vector3.one * 1.2f, Vector3.one, 0.1f, val => transform.localScale = val));
        if (text != null)
            text.color = Color.white;
        if (anima != null)
        {
            anima.SetBool("DisSelect", true);
            anima.SetBool("OnSelect", false);
        }
    }
}
