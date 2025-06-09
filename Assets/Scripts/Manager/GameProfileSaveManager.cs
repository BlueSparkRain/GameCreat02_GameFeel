using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class GameProfileSaveManager : MonoSingleton<GameProfileSaveManager>
{
    EventCenter eventCenter;

   

    [Header("存储数据")]
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
    /// 当前存档不再是新存档
    /// </summary>
    public void SetOldProfileData()
    {
        currentProfileData.isNewProfile = false;
    }

    private void Update()
    {
        //测试代码
        currentPanel=UIManager.Instance.currentPanel;
    }

    /// <summary>
    /// 选择一个存档或替换新存档
    /// </summary>
    public void SelectNewProfile(ProfileSaveData data)
    {
        currentProfileData = data;
        Debug.Log("当前选中存档！！" + data.ProfileID);
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
    /// 为当前存档设置存档名称
    /// </summary>
    /// <param name="profileName"></param>
    public void SetProfileNameData(string profileName)
    {
        currentProfileData.GetProfileName(profileName);
        DataSaver.SaveByJson(currentProfileData.ProfileID, currentProfileData);
        Debug.Log("存档名称设置成功：" + profileName);
    }

    /// <summary>
    /// 为当前存档更新游戏进度
    /// </summary>
    /// <param name="levelindex"></param>
    /// <param name="level"></param>
    /// <param name="hightScore"></param>
    public void SetProfileLevelData(int levelindex,E_LevelLevel level,int hightScore) 
    {
        Debug.Log("过关"+levelindex+"-"+ level);
        Debug.Log(currentProfileData);
        currentProfileData.GetLeveLComplete(levelindex,level,hightScore);

        DataSaver.SaveByJson(currentProfileData.ProfileID, currentProfileData);
    }

    /// <summary>
    /// 获得新成就
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
    /// 发出通知，所有对象发送数据
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
