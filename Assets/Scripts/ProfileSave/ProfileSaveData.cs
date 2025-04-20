using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public class ProfileSaveData
{
    /// <summary>
    /// 存档标识
    /// </summary>
    public string ProfileID;

    /// <summary>
    /// 保存日期
    /// </summary>
    public string saveDate;
    /// <summary>
    /// 保存时间
    /// </summary>
    public string saveTime;
   
    public ProfileSaveData(string ProfileID)
    {
        this.ProfileID = ProfileID;
        //SaveTime=DateTime.Now;
    }

    /// <summary>
    /// 是新存档
    /// </summary>
    public bool isNewProfile = true;

    /// <summary>
    /// 存档名称
    /// </summary>
    public string profileName;

    /// <summary>
    ///当前关卡数
    /// </summary>
    public int currentLevel;

    /// <summary>
    /// 关卡进度
    /// </summary>
    public List<LevelSaveData> levelDatas=new List<LevelSaveData>(6);

    /// <summary>
    /// 成就序号列表
    /// </summary>
    public List<int> achievementsIDList = new List<int>(50) { 0 };

    /// <summary>
    /// 成就达成状态列表
    /// </summary>
    public List<bool> achievementsStateList = new List<bool>(50) { false };

    public void GetProfileName(string name)
    {
        profileName = name;
    }

    public void GetTime() 
    {

        // 获取当前日期和时间
        DateTime currentDateTime = DateTime.Now;


        // 获取当前日期（年-月-日）
        string date = currentDateTime.ToString("yyyy/MM/dd");

        // 获取当前时间（时:分:秒）
        //string time = currentDateTime.ToString("HH:mm:ss");
        string time = currentDateTime.ToString("HH:mm");

        saveDate = date;
        saveTime = time;
    }


    /// <summary>
    /// 完成关卡，不保存过关数据
    /// </summary>
    /// <param name="index"></param>
    /// <param name="level"></param>
    /// <param name="highestScore"></param>
    public void GetLeveLComplete(int index,E_LevelLevel level,int highestScore) 
    {
        //评级更高才覆盖
        if(level> levelDatas[index].levelLevel) 
        {
            levelDatas[index].levelLevel = level;
        }
        //分数更高才覆盖
        if(highestScore> levelDatas[index].levelHighestScore) 
        {
            levelDatas[index].levelHighestScore = highestScore;
        }
    }

    /// <summary>
    /// 获得新成就
    /// </summary>
    public void GetNewAchievement(int achievementID)
    {
       achievementsIDList[achievementID] = achievementID;
       achievementsStateList[achievementID] = true;
    }
}

/// <summary>
/// 保存数据-关卡数据
/// </summary>
public class LevelSaveData 
{
    /// <summary>
    /// 关卡ID
    /// </summary>
    public int levelID;

    /// <summary>
    /// 关卡评级
    /// </summary>
    public E_LevelLevel levelLevel=E_LevelLevel.None;

    /// <summary>
    /// 关卡历史最高分
    /// </summary>
    public int levelHighestScore;
    
    /// <summary>
    /// 
    /// </summary>
    public bool isUnLock=false;

    /// <summary>
    /// 解锁本关卡
    /// </summary>
    public void UnLockSelf() 
    {
       isUnLock = true;
    }

    /// <summary>
    /// 完成本关卡
    /// </summary>
    public void GetLevel(E_LevelLevel level) 
    {
       levelLevel = level;
    }

    public void GetHightScore(int score) 
    {
       levelHighestScore = score;
    }

    public LevelSaveData(int levelID) 
    {
      this.levelID = levelID;
    }

}
