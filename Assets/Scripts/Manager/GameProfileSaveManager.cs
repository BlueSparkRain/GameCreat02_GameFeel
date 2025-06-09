using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class GameProfileSaveManager : MonoSingleton<GameProfileSaveManager>
{
    EventCenter eventCenter;

   

    [Header("�洢����")]
    [SerializeField]private ProfileSaveData currentProfileData;

    public ProfileSaveData ProfileSaveData => currentProfileData;

    public  BasePanel currentPanel;

    private void OnEnable()
    {

        EventCenter.Instance.AddEventListener(E_EventType.E_CurrentLevelOver, StopCoroutines);
    }

    private void OnDisable()
    {
        EventCenter.Instance.RemoveEventListener(E_EventType.E_CurrentLevelOver, StopCoroutines);
    }

    void StopCoroutines()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// ��ǰ�浵�������´浵
    /// </summary>
    public void SetOldProfileData()
    {
        currentProfileData.isNewProfile = false;
    }

    private void Update()
    {
        //���Դ���
        currentPanel=UIManager.Instance.currentPanel;
    }

    /// <summary>
    /// ѡ��һ���浵���滻�´浵
    /// </summary>
    public void SelectNewProfile(ProfileSaveData data)
    {
        currentProfileData = data;
        Debug.Log("��ǰѡ�д浵����" + data.ProfileID);
    }

    public void UnLockNewLevel(int lastestLevelIndex) 
    {
        if (lastestLevelIndex <= currentProfileData.levelDatas.Count)
        {
            currentProfileData.lastestLevel = lastestLevelIndex;
            currentProfileData.levelDatas[lastestLevelIndex - 1].isUnLock = true;
            DataSaver.SaveByJson(currentProfileData.ProfileID, currentProfileData);
        }
    }

    /// <summary>
    /// Ϊ��ǰ�浵���ô浵����
    /// </summary>
    /// <param name="profileName"></param>
    public void SetProfileNameData(string profileName)
    {
        currentProfileData.GetProfileName(profileName);
        DataSaver.SaveByJson(currentProfileData.ProfileID, currentProfileData);
        Debug.Log("�浵�������óɹ���" + profileName);
    }

    /// <summary>
    /// Ϊ��ǰ�浵������Ϸ����
    /// </summary>
    /// <param name="levelindex"></param>
    /// <param name="level"></param>
    /// <param name="hightScore"></param>
    public void SetProfileLevelData(int levelindex,E_LevelLevel level,int hightScore) 
    {
        Debug.Log("����"+levelindex+"-"+ level);
        Debug.Log(currentProfileData);
        currentProfileData.GetLeveLComplete(levelindex,level,hightScore);

        DataSaver.SaveByJson(currentProfileData.ProfileID, currentProfileData);
    }

    /// <summary>
    /// ����³ɾ�
    /// </summary>
    public void SetAchievementsData(int achievementID)
    {
        currentProfileData.GetNewAchievement(achievementID);
    }

  
    protected override void InitSelf()
    {
        base.InitSelf();

        if(eventCenter!=null)
        eventCenter = EventCenter.Instance;
    }


    /// <summary>
    /// ����֪ͨ�����ж���������
    /// </summary>
    public void SaveData() 
    {
        eventCenter.EventTrigger(E_EventType.E_DataSave);
    }


    public static void CreatEmptyJsonProfile()
    {
        DataSaver.SaveByJson(JsonFileName.Profile1, new ProfileSaveData(JsonFileName.Profile1));
        DataSaver.SaveByJson(JsonFileName.Profile2, new ProfileSaveData(JsonFileName.Profile2));
        DataSaver.SaveByJson(JsonFileName.Profile3, new ProfileSaveData(JsonFileName.Profile3));
    }
}
