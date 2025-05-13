using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameProfilePanel : BasePanel
{
    List<GameProfileElement> profileElements = new List<GameProfileElement>();
    List<ProfileSaveData> profileDatas = new List<ProfileSaveData>();
    [Header("存档元素父")]
    [SerializeField] private Transform elementsFather;

    GameProfileElement currentProfileElement;

    public Button ReturnButton;

    /// <summary>
    /// 冻结所有存档元素
    /// </summary>
    public void LockAllElemnets() 
    {
        for (int i = 0; i < profileElements.Count; i++) 
        {
            StartCoroutine(profileElements[i].LockSelf());
        }
    }

    /// <summary>
    /// 解冻所有存档元素
    /// </summary>
    public void UnLockAllElements() 
    {
        for (int i = 0; i < profileElements.Count; i++)
        {
            StartCoroutine(profileElements[i].UnLockSelf());
        }
    }


    public void SetElementName(GameProfileElement element)
    {
        currentProfileElement = element;
    }

    public void UpdateCurrentProfileUIView()
    {
        currentProfileElement.UpdateView();
    }

    bool init;
    protected override void Init()
    {
        base.Init();
        if (!init)
        {
            ReturnButton.onClick.AddListener(() =>
            {
                uiManager.HidePanel<GameProfilePanel>();
            });
        //预缓存
            for (int i = 0; i < elementsFather.childCount; i++)
            {
                profileElements.Add(
                    elementsFather.GetChild(i).GetComponent<GameProfileElement>());
            }
        }
        //面板开启时,为三个存档元素加载存档数据
        LoadAllProfiles();
    }

    public override void ShowPanel()
    {
        if(canClosePanel)
            return;
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        if (canClosePanel)
            yield break;

        yield return uiTweener.UIEaseInFrom(E_Dir.下, transform, UIRoot, transTime);
        canClosePanel = true;
    }

    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.下, transform, UIRoot, transTime);
        canClosePanel=false;    
    }


    /// <summary>
    /// 为存档UI分配存档数据
    /// </summary>
    void LoadAllProfiles()
    {
        PrepareProfileDatas();
        for (int i = 0; i < profileElements.Count; i++)
            profileElements[i].GetProfileData(profileDatas[i],i+1);
    }

    /// <summary>
    /// 根据文件路径反序列得到各个存档的数据
    /// </summary>
    void PrepareProfileDatas()
    {
        ProfileSaveData data1 = DataSaver.LoadFromJson<ProfileSaveData>(JsonFileName.Profile1);
        ProfileSaveData data2 = DataSaver.LoadFromJson<ProfileSaveData>(JsonFileName.Profile2);
        ProfileSaveData data3 = DataSaver.LoadFromJson<ProfileSaveData>(JsonFileName.Profile3);

        profileDatas.Add(data1);
        profileDatas.Add(data2);
        profileDatas.Add(data3);
    }
}
