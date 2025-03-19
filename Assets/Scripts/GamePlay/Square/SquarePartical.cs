using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarePartical : MonoBehaviour
{
    private ParticleSystem particle;

    /// <summary>
    /// ÖÆÔì±¬Õ¨Á£×Ó
    /// </summary>
    /// <param name="sprite"></param>
   public void StartPlay(Sprite sprite) 
   {
        particle = transform.GetComponent<ParticleSystem>();
        particle.textureSheetAnimation.SetSprite(0,sprite);
        particle.Play();
        StartCoroutine(DestorySelf());
        particle.transform.SetParent(null);
      
    }

    IEnumerator DestorySelf() 
    {
        yield return new WaitForSeconds(1f);
        DestroyImmediate(transform.gameObject);

    }
}
