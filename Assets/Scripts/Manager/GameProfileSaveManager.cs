using System;
using UnityEngine;

public class GameProfileSaveManager : MonoSingleton<GameProfileSaveManager>
{
    EventCenter eventCenter;

    [Header("存储数据")]
    [SerializeField]private ProfileSaveData currentProfile;

    public ProfileSaveData ProfileSaveData => currentProfile;

    public  BasePanel currentPanel;

    /// <summary>
    /// 当前存档不再是新存档
    /// </summary>
    public void SetOldProfileData()
    {
        currentProfile.isNewProfile = false;
    }

    private void Update()
    {
        currentPanel=UIManager.Instance.currentPanel;
    }

    /// <summary>
    /// 为当前存档设置存档名称
    /// </summary>
    /// <param name="profileName"></param>
    public void SetProfileNameData(string profileName)
    {
        currentProfile.GetProfileName(profileName);
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
        currentProfile.GetLeveLComplete(levelindex,level,hightScore);
    }

    /// <summary>
    /// 获得新成就
    /// </summary>
    public void SetAchievementsData(int achievementID)
    {
        currentProfile.GetNewAchievement(achievementID);
    }

  
    protected override void InitSelf()
    {
        base.InitSelf();

        if(eventCenter!=null)
        eventCenter = EventCenter.Instance;
    }


    /// <summary>
    /// 替换新存档
    /// </summary>
    public void SelectNewProfile(ProfileSaveData data) 
    {
        currentProfile = data;
        Debug.Log( "当前选中存档！！"+data.ProfileID);
    }


    /// <summary>
    /// 发出通知，所有对象发送数据
    /// </summary>
    public void SaveData() 
    {
        eventCenter.EventTrigger(E_EventType.E_DataSave);
    }
  
    ///// <summary>
    ///// 程序关闭，自动写入存档文件
    ///// </summary>
    //private void OnApplicationQuit()
    //{
    //    if (currentProfile != null)
    //    {
    //        currentProfile.GetTime();
    //        DataSaver.SaveByJson(currentProfile.ProfileID, currentProfile);
    //    }
    //}
}
