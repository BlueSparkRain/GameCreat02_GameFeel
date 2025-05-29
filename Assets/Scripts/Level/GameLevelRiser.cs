using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameLevelRiser : MonoBehaviour
{
    [Header("关卡序号")]
    public int LevelIndex;
    [Header("关卡序号文本")]
    public TMP_Text LevelIndexText;

    [Header("基础消除得分")]
    public int LevelBaseScore=1000;
    [Header("卡点消除得分")]
    public int LevelChartScore=2000;

    [Header("相机初始位置")]
    public Transform InitCamPos;
    [Header("相机聚焦屏幕位置")]
    public Transform FocusCamPos;
    [Header("相机聚焦前准备")]
    public float CamWaitTime = 3;
    [Header("相机聚焦时长")]
    public float CameraTransDuration=2;

    [Header("关卡序号")]
    public GameObject LevelIndexUIObj;
    [Header("评级UI")]
    public GameObject ProgressUIObj;
    [Header("分数")]
    public GameObject ScoreUIObj;
    [Header("音乐时长UI")]
    public GameObject MusicTimerUIObj;
    Camera mainCam;

    [Header("本关卡乐曲谱面资源名【Assets/streamingAssets】")]
    public string chartTimeResName;
    [Header("本关卡乐曲资源名")]
    public string musicResName;

    [Header("本关卡音乐时长")]
    public float musicTime;
    ScoreController scoreController;
    GameMap map;
    notes chartTimes;



    private void OnEnable()
    {
        EventCenter.Instance.AddEventListener(E_EventType.E_GetALevel,SetUIObjAnim);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener(E_EventType.E_GetALevel,SetUIObjAnim);
    }

    void InitChartSample() 
    {
        MusicManager.Instance.PlayBKMusic(musicResName);
        scoreController.SelfInit(musicTime);
        //scoreController.SelfInit(MusicManager.Instance.PlayBKMusic(musicResName,false));
        StartCoroutine(ChartCheckManager.Instance.SetUpChartsSample(chartTimes.MusicNotes));
    }

    private void Awake()
    {
        EventCenter.Instance.EventTrigger(E_EventType.E_NewLevel);
        chartTimes = new notes(chartTimeResName);
        mainCam = Camera.main;
        scoreController = FindAnyObjectByType<ScoreController>();
        map = FindAnyObjectByType<GameMap>();

        //设置关卡序号
        LevelIndexText.text = LevelIndex.ToString();

        WholeObjPoolManager.Instance.LoadNewPool();

        StartCoroutine(CamFocusOnScreen());
    }


    IEnumerator CamFocusOnScreen() 
    {
      yield return  new WaitForSeconds(CamWaitTime);
      StartCoroutine(TweenHelper.MakeLerp(InitCamPos.localPosition, FocusCamPos.localPosition, CameraTransDuration,val=>mainCam.transform.localPosition=val));
      StartCoroutine(TweenHelper.MakeLerp(InitCamPos.localEulerAngles, FocusCamPos.localEulerAngles, CameraTransDuration,val=>mainCam.transform.localEulerAngles=val));
      yield return  new WaitForSeconds(1);
        ////显示分数面板入场动画
        StartCoroutine(UIObjScale(ScoreUIObj, Vector3.one * 0.25f));
        yield return  new WaitForSeconds(0.6f);
        //显示进度入场动画
        StartCoroutine(UIObjScale(LevelIndexUIObj, Vector3.one * 0.3f));
        ProgressUIObj.GetComponent<Animator>().SetTrigger("Appear");
        StartCoroutine(UIObjScale(MusicTimerUIObj, Vector3.one));
       //加载地图
       InitGameMap();
       yield return new WaitForSeconds(2);
      //开启采样
      InitChartSample();

    }

    void SetUIObjAnim()
    {
        ProgressUIObj.GetComponent<Animator>().SetTrigger("Idle");
    }

    IEnumerator UIObjScale(GameObject UIObj,Vector3 endValue) 
    {
       yield return TweenHelper.MakeLerp(UIObj.transform.localScale, new Vector3(0.3f, 1, 0.3f), 0.08f , val=> UIObj.transform.localScale = val);
       yield  return TweenHelper.MakeLerp(UIObj.transform.localScale, new Vector3(0.6f, 0.4f, 0.6f), 0.06f , val=> UIObj.transform.localScale = val);
       yield  return TweenHelper.MakeLerp(UIObj.transform.localScale, endValue, 0.06f , val=> UIObj.transform.localScale = val);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    void InitGameMap() 
    {
        map.LoadWholeMap();
    }
}
