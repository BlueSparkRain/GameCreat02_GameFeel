using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelRiser : MonoBehaviour
{
    public Transform InitCamPos;
    public Transform FocusCamPos;
    public float CameTransDuration=2;
    Camera mainCam;

    public GameObject ProgressUIObj;
    public GameObject ScoreUIObj;

    void Start()
    {
        mainCam=Camera.main;

        StartCoroutine(CamFocusOnScreen());
    }

    IEnumerator CamFocusOnScreen() 
    {
      yield return  new WaitForSeconds(2);
      StartCoroutine(TweenHelper.MakeLerp(InitCamPos.localPosition, FocusCamPos.localPosition, CameTransDuration,val=>mainCam.transform.localPosition=val));
      StartCoroutine(TweenHelper.MakeLerp(InitCamPos.localEulerAngles, FocusCamPos.localEulerAngles, CameTransDuration,val=>mainCam.transform.localEulerAngles=val));
      yield return  new WaitForSeconds(2);
      StartCoroutine(UIObjScale(ScoreUIObj, Vector3.one * 0.25f));
      yield return  new WaitForSeconds(1);
      StartCoroutine(UIObjScale(ProgressUIObj,Vector3.one*0.5f));
      LoadGameMap();
    }

    IEnumerator UIObjScale(GameObject UIObj,Vector3 endValue) 
    {
       yield return TweenHelper.MakeLerp(UIObj.transform.localScale, new Vector3(0.3f, 1, 0.3f), 0.08f , val=> UIObj.transform.localScale = val);
       yield  return TweenHelper.MakeLerp(UIObj.transform.localScale, new Vector3(0.6f, 0.4f, 0.6f), 0.06f , val=> UIObj.transform.localScale = val);
       yield  return TweenHelper.MakeLerp(UIObj.transform.localScale, endValue, 0.06f , val=> UIObj.transform.localScale = val);

    }

    /// <summary>
    /// ©╙й╪сно╥
    /// </summary>
    void LoadGameMap() 
    {
    

    }
}
