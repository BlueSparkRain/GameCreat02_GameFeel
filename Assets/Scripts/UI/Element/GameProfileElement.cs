using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameProfileElement : MonoBehaviour
{
    //用来读取历史存档，显示数据信息
    [Header("存档编号文本")]
    public TMP_Text ProfileIDText;
    private int profileID;

    [Header("时间戳文本")]
    public TMP_Text TimeText;

    [Header("当前关卡文本")]
    public TMP_Text CurrentLevelText;

    [Header("当前存档昵称文本")]
    public TMP_Text ProfileNameText;

    [Header("引导文本")]
    public TMP_Text TechText;

    [Header("加载存档按钮")]
    public Button LoadProfileButton;

    [Header("删除存档按钮")]
    public Button ClearProfileButton;

    [Header("存档数据")]
    [SerializeField] private ProfileSaveData profileSaveData;

    GameProfileSaveManager gameProfileSaveManager;
    UIManager uiManager;

    WaitForSeconds delay = new WaitForSeconds(0.3f);
    CanvasGroup canvasGroup;

    private void Start()
    {
        LoadProfileButton.onClick.AddListener(() =>
        {
            //加载存档信息，进入关卡选择界面【不支持关卡内进度存档！！！】

            //if (canOpenWindow)
            //{
            //    canOpenWindow = false;
            //}

            LoadCurrentProfile();
        });

        ClearProfileButton.onClick.AddListener(() =>
        {
            //用户请求删除存档信息
            CallClearConfirm();
        });

        gameProfileSaveManager = GameProfileSaveManager.Instance;
        canvasGroup = GetComponent<CanvasGroup>();
        uiManager = UIManager.Instance;
    }

    /// <summary>
    /// 获取自凹槽的存档数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    public void GetProfileData(ProfileSaveData data, int index)
    {
        profileSaveData = data;
        ProfileIDText.text = index.ToString();
        Debug.Log("获取到存档：" + ProfileIDText.text);
        profileID = index;
        UpdateView();
    }

    /// <summary>
    /// 根据当前存档更新UI数据显示
    /// </summary>
    public void UpdateView()
    {
        if (!profileSaveData.isNewProfile)
        {
            TechText.text = "点击进入游戏";
            //存档名称
            ProfileNameText.text = profileSaveData.profileName;
            //存档日期
            TimeText.text = profileSaveData.saveDate
                            + " " +
                            profileSaveData.saveTime;
            //存档最新关卡
            CurrentLevelText.text = profileSaveData.currentLevel.ToString();
        }
        else
        {
            TechText.text = "请创建新存档";
            ProfileNameText.text = "———新存档———";
            TimeText.text = "———???———";
            CurrentLevelText.text = "0";
        }
    }

    /// <summary>
    ///加载当前存档[如果是新存档，需要先输入存档名称]
    /// </summary>
    void LoadCurrentProfile()
    {
        transform.parent.GetComponentInParent<GameProfilePanel>().SetElementName(this);

        if (profileSaveData.isNewProfile)
        {
            (uiManager.GetPanel<GameProfilePanel>() as GameProfilePanel).LockAllElemnets();

            //打开输入弹窗，待玩家输入存档名称后写入json文件
            uiManager.ShowPanel<Pop_InputField_WindowPanel>(panel => panel.InputToConfirm(OnProfileNameSetValueChange, ProfileNameSetUpAction), true, true);
            //当前存档不为新文档
            profileSaveData.isNewProfile = false;
        }
        else //历史存档
        {
            //点击进入关卡选择
            Debug.Log("加载历史存档");
            gameProfileSaveManager.SelectNewProfile(profileSaveData);
            //确认当前存档为全局
            SceneLoadManager.Instance.TransToLoadScene(1,E_SceneTranType.过场图过渡);
        }
    }

    bool canOpenWindow = true;

    /// <summary>
    /// 呼出确认弹窗防止误操作
    /// </summary>
    void CallClearConfirm()
    {
        if (canOpenWindow)
        {
            canOpenWindow = false;
            (uiManager.GetPanel<GameProfilePanel>() as GameProfilePanel).LockAllElemnets();
            uiManager.ShowPanel<Pop_Confirm_WindowPanel>(panel => panel.ToConfirm("是否删除存档", ClearCurrentProfile, ClearDisposeAction), true, true);
        }
    }
    /// <summary>
    /// 清除当前存档【成空白存档】
    /// </summary>
    void ClearCurrentProfile()
    {
        Debug.Log("注意：" + profileSaveData.ProfileID + "已被清空！");
        ProfileSaveData newData = new ProfileSaveData(profileSaveData.ProfileID);
        DataSaver.SaveByJson(profileSaveData.ProfileID, newData);
        GetProfileData(newData, profileID);
        //解锁其他存档元素
        canOpenWindow = true;

        (uiManager.GetPanel<GameProfilePanel>() as GameProfilePanel).UnLockAllElements();
    }

    /// <summary>
    /// 取消删除存档指令
    /// </summary>
    void ClearDisposeAction()
    {
        (uiManager.GetPanel<GameProfilePanel>() as GameProfilePanel).UnLockAllElements();
    }

    /// <summary>
    /// 用户每次修改存档名称
    /// </summary>
    /// <param name="str">正在输入的存档名称</param>
    void OnProfileNameSetValueChange(String str)
    {
        Debug.Log(profileSaveData.ProfileID + "得到新名字" + str);
        profileSaveData.GetProfileName(str);
        DataSaver.SaveByJson(profileSaveData.ProfileID, profileSaveData);
    }
    /// <summary>
    /// 修改完毕，设置存档名称
    /// </summary>
    void ProfileNameSetUpAction()
    {
        profileSaveData.GetTime();
        DataSaver.SaveByJson(profileSaveData.ProfileID, profileSaveData);
        UpdateView();

        canOpenWindow = true;
        (uiManager.GetPanel<GameProfilePanel>() as GameProfilePanel).UnLockAllElements();
        //StartCoroutine(LockALittle());
    }


    /// <summary>
    /// 冻结自身
    /// </summary>
    /// <returns></returns>
    public IEnumerator LockSelf()
    {
        yield return delay;
        canvasGroup.interactable = false;

    }
    /// <summary>
    /// 解冻自身
    /// </summary>
    /// <returns></returns>
    public IEnumerator UnLockSelf()
    {
        yield return delay;
        canvasGroup.interactable = true;
    }

    /// <summary>
    /// 程序关闭，自动写入存档文件
    /// </summary>
    private void OnApplicationQuit()
    {
        if (profileSaveData != null)
        {
            profileSaveData.GetTime();
            DataSaver.SaveByJson(profileSaveData.ProfileID, profileSaveData);
        }
    }
}
