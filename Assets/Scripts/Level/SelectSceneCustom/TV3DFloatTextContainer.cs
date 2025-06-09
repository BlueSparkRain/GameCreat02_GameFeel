using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TV3DFloatTextContainer : MonoBehaviour
{
     public List<Transform>  text3Dboards=new List<Transform>();


    public  IEnumerator ChangeTextsVislble(bool show) 
    {
        for (int i = 0; i < text3Dboards.Count; i++)
        {
            if(show)
            StartCoroutine(UITween.Instance.UIDoFade(text3Dboards[i], 0, 1, 0.5f));
            else
            StartCoroutine(UITween.Instance.UIDoFade(text3Dboards[i], 1, 0, 0.5f));

            yield return new WaitForSeconds(0.2f);
        }
    }
}
