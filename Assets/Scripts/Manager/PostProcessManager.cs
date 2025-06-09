using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoSingleton<PostProcessManager>
{
    [Header("References")]
    [SerializeField] private Volume urpVolume;

    private Bloom _bloom;
    private Vignette _vignette;
    private LensDistortion _lensDistortion;

    private FilmGrain _filmGrain;
    private ColorAdjustments _colorAdjustments;

    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener(E_EventType.E_NewLevel,InitSelf);  
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener(E_EventType.E_NewLevel,InitSelf);  
    }

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
      urpVolume.profile.TryGet(out _filmGrain);
      urpVolume.profile.TryGet(out _colorAdjustments);
    }

    public  bool isGrayWorld;
    public void GrayWorldFlash(float startPoint = 0, float endPoint = -100f, float transTime = 0.2f, float pingpongDelayTime = 4) 
    {
        if (!isGrayWorld)
        {
            isGrayWorld = true;
            StartCoroutine(FilmGrainFlash(startPoint, 1, transTime, pingpongDelayTime));
            StartCoroutine(GrayFlash(startPoint, endPoint, transTime, pingpongDelayTime));
        }
        else 
        {
            float _startPoint= _colorAdjustments.saturation.value;
            //StopAllCoroutines();
            StartCoroutine(FilmGrainFlash(_startPoint, 1, transTime, pingpongDelayTime));
            StartCoroutine(FilmGrainFlash(_startPoint, endPoint, transTime, pingpongDelayTime));
        }
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
        yield return FadeInAndOut(_lensDistortion.intensity, startPoint, endPoint, transTime, pingpongDelayTime);
        nextLenDistortion = true;
    }

    IEnumerator FilmGrainFlash(float startPoint, float endPoint, float transTime, float pingpongDelayTime)
    {
        yield return FadeInAndOut(_filmGrain.intensity, startPoint, endPoint, transTime, pingpongDelayTime);

    }

    IEnumerator GrayFlash(float startPoint, float endPoint, float transTime, float pingpongDelayTime) 
    {
      yield return FadeInAndOut(_colorAdjustments.saturation, startPoint, endPoint,transTime,pingpongDelayTime);
        isGrayWorld = false;
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
            StartCoroutine( VignetteFadeInOut(0.6f, 0.3f, 2, 1));
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
