using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScreen : MonoBehaviour
{
    [Header("�ؿ����")]
    public int levelNumber = 1;
    public Shader shader;
    private Material material;

    [Header("�ؿ�����")]
    public string levelName;
    [Header("�ؿ������ı�")]
    public TMP_Text levelNameText;

    public int levelStar = -1;

    public Texture _locked;
    public Texture _0Star; 
    public Texture _1Star;
    public Texture _2Star;
    public Texture _3Star;
    public Texture alpha;

    [Header("��ʷ��߷���")]
    public int historyScore;



    [Header("��ʷ�������")]
    public E_LevelLevel level=E_LevelLevel.None;

    /// <summary>
    /// ���ؿ���������
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
    /// ���ݴ浵�е�������Ϣ��ѡ��չʾ��ʽ
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

        //��ʾ�ؿ�����
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
    /// �������ؿ�
    /// </summary>
    public IEnumerator UnLockSelf()
    {
        //��������
        yield return null;
        Debug.Log("�����ؿ�" + transform.parent.GetSiblingIndex());
        ChangeLeveState(0);
    }

    /// <summary>
    /// �����Խ����Ĺؿ�
    /// </summary>
    void OnClickUnLockButton()
    {
        if (isUnLock)
        {
            Debug.Log("δ����");
            //StartCoroutine(LevelSelectManager.Instance.LockRemindShow());
            return;
        }
        Debug.Log("δ����");
        SceneLoadManager.Instance.TransToLoadScene(2+levelNumber,E_SceneTranType.��������);
    }

    public void ChangeLeveState(int levelStarNum)
    {
        if (levelStarNum < levelStar)
        {
            Debug.Log("�ⲻ����߷�");
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
