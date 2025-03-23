using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MylevelSelectButton : MonoBehaviour
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


    /// <summary>
    /// 本关卡正被锁定
    /// </summary>
    public bool isLocked;


    void Start()
    {
        material = new Material(shader);
        GetComponent<Image>().material = material;
        material.SetTexture("_MainTex", GetComponent<Image>().sprite.texture);
        material.SetTexture("_alpha", alpha);

        GetComponentInChildren<TMP_Text>().text = transform.parent.GetSiblingIndex().ToString();//关卡编号

        this.GetComponent<Button>().onClick.AddListener(OnClickUnLockButton);

        ChangeLeveState(-1);
        isLocked = true;
    }


    void Update()
    {
        if (material != null)
        {
            Vector4 mousePosition = Input.mousePosition;
            material.SetVector("_MousePos", mousePosition);
        }
    }

    /// <summary>
    /// 解锁本关卡
    /// </summary>
    public void UnLockSelf()
    {
        isLocked = false;
        Debug.Log("解锁关卡" + transform.parent.GetSiblingIndex());
        ChangeLeveState(0);
    }

    /// <summary>
    /// 进入以解锁的关卡
    /// </summary>
    void OnClickUnLockButton()
    {
        if (isLocked)
        {
            StartCoroutine(LevelSelectManager.Instance.LockRemindShow());

            return;
        }
        SceneLoadManager.Instance.LoadNewLevel(transform.parent.GetSiblingIndex() + 2);
        //UIManager.Instance.ShowPanel<SceneTransPanel>(panel => panel.SceneLoadingTrans(transform.parent.GetSiblingIndex() + 2));
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
