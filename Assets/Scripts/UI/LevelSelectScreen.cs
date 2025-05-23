using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScreen : MonoBehaviour
{
    [Header("关卡序号")]
    public int levelNumber = 1;
    public Shader shader;
    private Material material;

    [Header("关卡名字")]
    public string levelName;
    [Header("关卡名字文本")]
    public TMP_Text levelNameText;

    public int levelStar = -1;

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

    void Start()
    {
        material = new Material(shader);
        GetComponent<MeshRenderer>().material = material;
        material.SetTexture("_MainTex", _locked);
        material.SetTexture("_alpha", alpha);

        //显示关卡名字
        levelNameText.text = levelName;
        levelNameTrans= levelNameText.transform;
        ChangeLeveState(-1);
    }

    Vector3 lastFramePos;

    void Update()
    {
        if (Input.mousePosition!= lastFramePos && material != null)
        {
            lastFramePos = Input.mousePosition;
            Vector4 mousePosition = Input.mousePosition;
            material.SetVector("_MousePos", mousePosition);
        }
    }
    float a;
    /// <summary>
    /// 解锁本关卡
    /// </summary>
    public IEnumerator UnLockSelf()
    {
        //解锁动画
        yield return null;
        Debug.Log("解锁关卡" + transform.parent.GetSiblingIndex());
        ChangeLeveState(0);
    }

    /// <summary>
    /// 进入以解锁的关卡
    /// </summary>
    void OnClickUnLockButton()
    {
        if (isUnLock)
        {
            Debug.Log("未解锁");
            //StartCoroutine(LevelSelectManager.Instance.LockRemindShow());
            return;
        }
        Debug.Log("未解锁");
        SceneLoadManager.Instance.TransToLoadScene(2+levelNumber,E_SceneTranType.黑屏过渡);
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
                material.SetTexture("_LevelStatusTex", _locked);
                material.SetInt("_levelStar", -1);
                break;
            case 0:
                material.SetTexture("_LevelStatusTex", _0Star);
                material.SetInt("_levelStar", 0);
                break;
            case 1:
                material.SetTexture("_LevelStatusTex", _1Star);
                material.SetInt("_levelStar", 1);
                break;
            case 2:
                material.SetTexture("_LevelStatusTex", _2Star);
                material.SetInt("_levelStar", 2);
                break;
            case 3:
                material.SetTexture("_LevelStatusTex", _3Star);
                material.SetInt("_levelStar", 3);
                break;
        }
    }
}
