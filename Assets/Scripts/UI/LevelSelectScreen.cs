using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScreen : MonoBehaviour
{
    [Header("�ؿ����")]
    public int levelNumber = 1;
    public Material MyMaterial;
    private Material runtimeMat;
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

    bool canOpen=true;

    private void OnMouseEnter()
    {
        StartCoroutine(ShakeLevelNameRotAnim());
        
    }



    private void OnMouseUpAsButton()
    {
        //StartCoroutine(ShakeLevelNameRotAnim());
        if (canOpen)
        {
            canOpen = false;
            OnClickUnLockButton();
        }
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

    private void Awake()
    {


        SetMat();

        //��ʾ�ؿ�����
        levelNameText.text = levelName;
        levelNameTrans = levelNameText.transform;

        ChangeLeveState(-1);//<<<<======================����ǿ�������ǿ��Ըı�sahder��

    }
   

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
    /// �������ؿ�
    /// </summary>
    public void UnLockSelf()
    {
        //��������
        isUnLock = true;
        //Debug.Log("*************************************UnLock******************************* level : " + transform.parent.GetSiblingIndex());
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
    /// �����Խ����Ĺؿ�
    /// </summary>
    /// 
    void SetMat()
    {
        runtimeMat = new Material(MyMaterial);
        GetComponent<MeshRenderer>().material = runtimeMat;
        runtimeMat.SetTexture("_MainTex", _locked);
        runtimeMat.SetTexture("_alpha", alpha);
        return;
    }
    void OnClickUnLockButton()
    {
        if (!isUnLock)
        {
            Debug.Log("��ǰ�ؿ�δ����");
            LevelSelectManager.Instance.LockRemindShow();
            return;
        }
        UIManager.Instance.ShowPanel<Pop_Confirm_WindowPanel>(panel=>panel.ToConfirm("������Ϸ",InsideLevel, DisposeConfirm));


        //Debug.Log("��ǰ�ؿ��ѽ���");
        //SceneLoadManager.Instance.TransToLoadScene(2+levelNumber,E_SceneTranType.��������);
        //GameLevelCheckManager.Instance.SetCurrentLevel(levelNumber);
    }
    void DisposeConfirm() 
    {
        canOpen=true;
    }
    void InsideLevel()
    {
        canOpen = true;
        Debug.Log("��ǰ�ؿ��ѽ���");
        SceneLoadManager.Instance.TransToLoadScene(2 + levelNumber, E_SceneTranType.��������);
        GameLevelCheckManager.Instance.SetCurrentLevel(levelNumber);
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

                SetMat();

                runtimeMat.SetTexture("_LevelStatusTex", _locked);
                runtimeMat.SetFloat("_levelStar", -1);
               
                break;
            case 0:

                SetMat();

                runtimeMat.SetTexture("_LevelStatusTex", _0Star);
                runtimeMat.SetFloat("_levelStar", 0);
                
                break;
            case 2:

                SetMat();

                runtimeMat.SetTexture("_LevelStatusTex", _1Star);
                runtimeMat.SetFloat("_levelStar", 1);
               
                break;
            case 3:

                SetMat();

                runtimeMat.SetTexture("_LevelStatusTex", _2Star);
                runtimeMat.SetFloat("_levelStar", 2);
              
                break;
            case 4:

                SetMat();

                runtimeMat.SetTexture("_LevelStatusTex", _3Star);
                runtimeMat.SetFloat("_levelStar", 3);
               
                break;
            default:
                break;
        }
    }
}
