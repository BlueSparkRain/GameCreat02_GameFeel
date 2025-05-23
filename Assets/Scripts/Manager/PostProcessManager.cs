using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoSingleton<PostProcessManager>
{
    [Header("References")]
    [SerializeField] private Volume urpVolume;

    //[Header("Bloom Control")]
    //[Range(0, 5)] public float bloomIntensity = 1f;
    //[Header("Vignette Control")]
    //[Range(0, 1)] public float vignetteIntensity;

    private Bloom _bloom;
    private Vignette _vignette;
    private LensDistortion _lensDistortion;
    protected override void InitSelf()
    {
        base.InitSelf();
        GetCurrentVolume(FindAnyObjectByType<Volume>());
    }

    void GetCurrentVolume(Volume _urpVolume) 
    {
      urpVolume = _urpVolume;
      urpVolume.profile.TryGet(out _bloom);
      urpVolume.profile.TryGet(out _vignette);
      urpVolume.profile.TryGet(out _lensDistortion);
    }

    public void LenDistortionFlash(float startPoint=0, float endPoint=0.4f, float transTime=0.1f, float pingpongDelayTime = 0) 
    {
        if (nextLenDistortion) 
        {
            nextLenDistortion=false;
            if(_lensDistortion)
            StartCoroutine(LensDistortionFlash(startPoint,  endPoint, transTime, pingpongDelayTime));
        }
    }
    IEnumerator LensDistortionFlash(float startPoint, float endPoint, float transTime, float pingpongDelayTime) 
    {
      yield return FadeInAndOut(_lensDistortion.intensity, startPoint, endPoint,transTime,pingpongDelayTime);
      nextLenDistortion = true;
    }

    IEnumerator VignetteFadeInOut(float startPoint, float endPoint, float transTime, float pingpongDelayTime) 
    {
      yield return FadeInAndOut(_vignette.intensity, startPoint, endPoint,transTime,pingpongDelayTime);
      nextVignetteIdleEffect = true;
    }

    IEnumerator FadeInAndOut(ClampedFloatParameter floatAttri,float startPoint,float endPoint,float transTime,float pingpongDelayTime) 
    {
        yield return TweenHelper.MakeLerp(startPoint, endPoint, transTime, val => floatAttri.value = val);
        yield return new WaitForSeconds(pingpongDelayTime);
        yield return TweenHelper.MakeLerp(endPoint, startPoint,transTime, val => floatAttri.value = val);
    }

    bool VignetteIdle;

    bool nextVignetteIdleEffect=true;

    bool nextLenDistortion=true;

    public void OpenVignetteIdle() 
    {
        VignetteIdle = true;
    }
    public void CloseVignetteIdle() 
    {
       VignetteIdle = false; 
    }

    void Update()
    {
        if (VignetteIdle && nextVignetteIdleEffect)
        {
            nextVignetteIdleEffect = false;
            if(_vignette!=null)
            StartCoroutine( VignetteFadeInOut(0.7f, 0.3f, 2, 1));
        }



        //if (_bloom != null)
        //{
        //    _bloom.intensity.overrideState = true;
        //    _bloom.intensity.value = bloomIntensity;
        //}

        //if (_vignette != null)
        //{
        //    _vignette.intensity.overrideState = true;
        //    _vignette.intensity.value = vignetteIntensity;
        //}
    }
}
