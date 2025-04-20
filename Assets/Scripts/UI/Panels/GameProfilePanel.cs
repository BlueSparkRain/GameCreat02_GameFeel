using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProfilePanel : BasePanel
{
    List<GameProfileElement> profileElements = new List<GameProfileElement>();
    List<ProfileSaveData> profileDatas = new List<ProfileSaveData>();
    [Header("�浵Ԫ�ظ�")]
    [SerializeField] private Transform elementsFather;

    GameProfileElement currentProfileElement;

    /// <summary>
    /// �������д浵Ԫ��
    /// </summary>
    public void LockAllElemnets() 
    {
        for (int i = 0; i < profileElements.Count; i++) 
        {
            StartCoroutine(profileElements[i].LockSelf());
        }
    }

    /// <summary>
    /// �ⶳ���д浵Ԫ��
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
            //Ԥ����
            for (int i = 0; i < elementsFather.childCount; i++)
            {
                profileElements.Add(
                    elementsFather.GetChild(i).GetComponent<GameProfileElement>());
            }
        }
        //��忪��ʱ,Ϊ�����浵Ԫ�ؼ��ش浵����
        LoadAllProfiles();
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return uiTweener.UIEaseInFrom(E_Dir.��, transform, UIRoot, transTime);
    }

    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.��, transform, UIRoot, transTime);
    }


    /// <summary>
    /// Ϊ�浵UI����浵����
    /// </summary>
    void LoadAllProfiles()
    {
        PrepareProfileDatas();
        for (int i = 0; i < profileElements.Count; i++)
            profileElements[i].GetProfileData(profileDatas[i],i+1);
    }

    /// <summary>
    /// �����ļ�·�������еõ������浵������
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
