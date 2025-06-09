using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level03Custom : MonoBehaviour
{
    public Transform noiseScreen;
    public AnimationCurve noiseScreenCurve;

    IEnumerator NoiseScreenFlash() 
   {
        yield return new WaitForSeconds(82);
        yield return TweenHelper.MakeLerp(new Vector3(1, 1, 0), Vector3.one, 0.25f, val => noiseScreen.localScale = val, noiseScreenCurve);
        yield return new WaitForSeconds(2);
        yield return TweenHelper.MakeLerp( Vector3.one, new Vector3(0, 1, 0), 0.15f, val => noiseScreen.localScale = val, noiseScreenCurve);
   }

    private void Start()
    {
         StartCoroutine(NoiseScreenFlash());
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
