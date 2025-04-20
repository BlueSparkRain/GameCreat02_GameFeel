using System;
using UnityEngine;

public class GameProfileSaveManager : MonoSingleton<GameProfileSaveManager>
{
    EventCenter eventCenter;

    [Header("�洢����")]
    [SerializeField]private ProfileSaveData currentProfile;

    public ProfileSaveData ProfileSaveData => currentProfile;

    public  BasePanel currentPanel;

    /// <summary>
    /// ��ǰ�浵�������´浵
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
    /// Ϊ��ǰ�浵���ô浵����
    /// </summary>
    /// <param name="profileName"></param>
    public void SetProfileNameData(string profileName)
    {
        currentProfile.GetProfileName(profileName);
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
        currentProfile.GetLeveLComplete(levelindex,level,hightScore);
    }

    /// <summary>
    /// ����³ɾ�
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
    /// �滻�´浵
    /// </summary>
    public void SelectNewProfile(ProfileSaveData data) 
    {
        currentProfile = data;
        Debug.Log( "��ǰѡ�д浵����"+data.ProfileID);
    }


    /// <summary>
    /// ����֪ͨ�����ж���������
    /// </summary>
    public void SaveData() 
    {
        eventCenter.EventTrigger(E_EventType.E_DataSave);
    }
  
    ///// <summary>
    ///// ����رգ��Զ�д��浵�ļ�
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
