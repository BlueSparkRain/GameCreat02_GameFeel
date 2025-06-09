using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using UnityEngine;

[Serializable]
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
    public int lastestLevel=0;



    /// <summary>
    /// 关卡进度
    /// </summary>
    public List<LevelSaveData> levelDatas=new List<LevelSaveData>();

    /// <summary>
    /// 更新关卡数据
    /// </summary>
    /// <param name="levelIndex"></param>
    /// <param name="level"></param>
    /// <param name="levelHighestScore"></param>
    public void UpdateLevelData(int levelIndex,E_LevelLevel level, int levelHighestScore) 
    {
        levelDatas[levelIndex].GetLevel(level);
        levelDatas[levelIndex].GetHightScore(levelHighestScore);
    }

    /// <summary>
    /// 成就序号列表
    /// </summary>
    public List<int> achievementsIDList = new List<int>(50) { 0 };

    /// <summary>
    /// 成就达成状态列表
    /// </summary>
    public List<bool> achievementsStateList = new List<bool>(50) { false };

    /// <summary>
    /// 修改存档名称
    /// </summary>
    /// <param name="name"></param>
    public void GetProfileName(string name)
    {
        profileName = name;
    }

    public void Init() 
    {
        UnityEngine.Debug.Log("毋庸置疑");
        for (int i = 1; i < 5; i++)
        {
            levelDatas.Add(new LevelSaveData(i));
        }
        UnityEngine.Debug.Log(levelDatas.Count);
    }
    public void GetTime() 
    {

        // 获取当前日期和时间
        DateTime currentDateTime = DateTime.Now;


        // 获取当前日期（年-月-日）
        string date = currentDateTime.ToString("yyyy/MM/dd");

        // 获取当前时间（时:分:秒）
        string time = currentDateTime.ToString("HH:mm");

        saveDate = date;
        saveTime = time;
    }


    /// <summary>
    /// 完成关卡，保存过关数据
    /// </summary>
    /// <param name="index"></param>
    /// <param name="level"></param>
    /// <param name="highestScore"></param>
    public void GetLeveLComplete(int index,E_LevelLevel level,int highestScore) 
    {
        int trueIndex = index - 1;
        UnityEngine.Debug.Log(trueIndex);
        UnityEngine.Debug.Log(level+"-" + levelDatas[trueIndex].levelLevel);
        UnityEngine.Debug.Log(highestScore + "-" + levelDatas[trueIndex].levelHighestScore);
        
        //评级更高才覆盖
        if(level> levelDatas[trueIndex].levelLevel) 
        {
            levelDatas[trueIndex].levelLevel = level;
        }
        //分数更高才覆盖
        if(highestScore> levelDatas[trueIndex].levelHighestScore) 
        {
            levelDatas[trueIndex].levelHighestScore = highestScore;
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

[Serializable]
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
