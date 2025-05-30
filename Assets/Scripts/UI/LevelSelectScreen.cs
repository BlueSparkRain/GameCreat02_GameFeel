using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScreen : MonoBehaviour
{
    [Header("关卡序号")]
    public int levelNumber = 1;
    public Material MyMaterial;
    private Material runtimeMat;
    [Header("关卡名字")]
    public string levelName;
    [Header("关卡名字文本")]
    public TMP_Text levelNameText;

    public float levelStar = -1;

    public Texture _locked;
    public Texture _0Star; 
    public Texture _1Star;
    public Texture _2Star;
    public Texture _3Star;
    public Texture alpha;

    [Header("历史最高分数")]
    public int historyScore;



    [Header("历史最高评级")]
    public E_LevelLevel level=E_LevelLevel.None;

    /// <summary>
    /// 本关卡正被锁定
    /// </summary>
    public bool isUnLock;

    private void OnMouseEnter()
    {
        StartCoroutine(ShakeLevelNameRotAnim());
        
    }

    private void OnMouseExit()
    {
        

    }

    private void OnMouseUpAsButton()
    {
        //StartCoroutine(ShakeLevelNameRotAnim());
        OnClickUnLockButton();
    }


    /// <summary>
    /// 根据存档中的评级信息来选择展示方式
    /// </summary>
    public void InitSelf(E_LevelLevel _level,int _historyScore,bool isUnLock) 
    {
        level = _level;
        historyScore = _historyScore;
        this.isUnLock = isUnLock;
        if (isUnLock) 
        {
           UnLockSelf();
        }
    }

    Transform levelNameTrans;
    IEnumerator ShakeLevelNameRotAnim()
    {
        yield return TweenHelper.MakeLerp(Vector3.zero, new Vector3(0, 90, 5.8f), 0.08f, val => levelNameTrans.localEulerAngles = val);
        StartCoroutine(TweenHelper.MakeLerp(levelNameTrans.localScale,new Vector3(2,1.2f,2),0.05f,val=> levelNameTrans.localScale=val));
        yield return TweenHelper.MakeLerp(new Vector3(90, 90, -5.8f), new Vector3(90, 0, 5.8f), 0.08f, val => levelNameTrans.localEulerAngles= val);
        StartCoroutine(TweenHelper.MakeLerp(levelNameTrans.localScale,Vector3.one*1,0.05f,val=> levelNameTrans.localScale=val));
        yield return TweenHelper.MakeLerp(new Vector3(0, 90, 5.8f), Vector3.zero, 0.08f, val => levelNameTrans.localEulerAngles = val);
    }

    private void Awake()
    {


         runtimeMat= new Material(MyMaterial);
        GetComponent<MeshRenderer>().material = runtimeMat;
        runtimeMat.SetTexture("_MainTex", _locked);
        runtimeMat.SetTexture("_alpha", alpha);

        //显示关卡名字
        levelNameText.text = levelName;
        levelNameTrans = levelNameText.transform;

        ChangeLeveState(-1);//<<<<======================这里强行设置是可以改变sahder的

    }
    //void Start()
    //{
    //    material = new Material(shader);
    //    GetComponent<MeshRenderer>().material = material;
    //    material.SetTexture("_MainTex", _locked);
    //    material.SetTexture("_alpha", alpha);

    //    //显示关卡名字
    //    levelNameText.text = levelName;
    //    levelNameTrans= levelNameText.transform;
    //    ChangeLeveState(-1);
    //}

    Vector3 lastFramePos;

    void Update()
    {
        if (Input.mousePosition!= lastFramePos && GetComponent<MeshRenderer>().material != null)
        {
            lastFramePos = Input.mousePosition;
            Vector4 mousePosition = Input.mousePosition;
            GetComponent<MeshRenderer>().material.SetVector("_MousePos", mousePosition);
        }
    }
    float a;
    /// <summary>
    /// 解锁本关卡
    /// </summary>
    public void UnLockSelf()
    {
        //解锁动画
        isUnLock = true;
        Debug.Log("解锁关卡" + transform.parent.GetSiblingIndex());
        ChangeLeveState(0);
    }

    /// <summary>
    /// 0:C 1:B 2:A 3:S
    /// </summary>
    /// <param name="levelIndex"></param>
    public void UpdateLevelAppearence(int levelIndex) 
    {
        ChangeLeveState(levelIndex);
    }

    /// <summary>
    /// 进入以解锁的关卡
    /// </summary>
    void OnClickUnLockButton()
    {
        if (!isUnLock)
        {
            Debug.Log("当前关卡未解锁");
            LevelSelectManager.Instance.LockRemindShow();
            return;
        }

        Debug.Log("当前关卡已解锁");
        SceneLoadManager.Instance.TransToLoadScene(2+levelNumber,E_SceneTranType.黑屏过渡);
        GameLevelCheckManager.Instance.SetCurrentLevel(levelNumber);
    }

    public void ChangeLeveState(int levelStarNum)
    {
        if (levelStarNum < levelStar)
        {
            Debug.Log("这不是最高分");
            return;
        }

        levelStar = levelStarNum;

        switch (levelStar)
        {
            case -1:
                runtimeMat.SetTexture("_LevelStatusTex", _locked);
                runtimeMat.SetFloat("_levelStar", -1);
                Debug.Log("levelStar: " + runtimeMat.GetFloat("_levelStar"));
                break;
            case 0:
                runtimeMat.SetTexture("_LevelStatusTex", _0Star);
                runtimeMat.SetFloat("_levelStar", 0);
                Debug.Log("levelStar: " + runtimeMat.GetFloat("_levelStar"));
                break;
            case 2:
                runtimeMat.SetTexture("_LevelStatusTex", _1Star);
                runtimeMat.SetFloat("_levelStar", 1);
                Debug.Log("levelStar: " + runtimeMat.GetFloat("_levelStar"));
                break;
            case 3:
                runtimeMat.SetTexture("_LevelStatusTex", _2Star);
                runtimeMat.SetFloat("_levelStar", 2);
                Debug.Log("levelStar: " + runtimeMat.GetFloat("_levelStar"));
                break;
            case 4:
                runtimeMat.SetTexture("_LevelStatusTex", _3Star);
                runtimeMat.SetFloat("_levelStar", 3);
                Debug.Log("levelStar: " + runtimeMat.GetFloat("_levelStar"));
                break;
        }
    }
}
