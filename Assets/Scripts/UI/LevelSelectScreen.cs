using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScreen : MonoBehaviour
{
    public int levelNumber = 1;
    public Shader shader;
    private Material material;
    public string levelName;
    public int levelStar = -1;

    public Texture _locked;
    public Texture _0Star;
    public Texture _1Star;
    public Texture _2Star;
    public Texture _3Star;
    public Texture alpha;

    [Header("历史最高分数")]
    public int historyScore;

    [Header("历史评级")]
    public E_LevelLevel level=E_LevelLevel.None;

    /// <summary>
    /// 本关卡正被锁定
    /// </summary>
    public bool isUnLock;


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

    void Start()
    {
        material = new Material(shader);
        GetComponent<Image>().material = material;
        material.SetTexture("_MainTex", GetComponent<Image>().sprite.texture);
        material.SetTexture("_alpha", alpha);

        GetComponentInChildren<TMP_Text>().text = transform.parent.GetSiblingIndex().ToString();//关卡编号
        GetComponent<Button>().onClick.AddListener(OnClickUnLockButton);
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
            //StartCoroutine(LevelSelectManager.Instance.LockRemindShow());
            return;
        }
        SceneLoadManager.Instance.LoadNewLevel(transform.parent.GetSiblingIndex() + 2);
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
