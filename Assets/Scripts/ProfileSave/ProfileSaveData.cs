using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using UnityEngine;

[Serializable]
public class ProfileSaveData
{
    /// <summary>
    /// �浵��ʶ
    /// </summary>
    public string ProfileID;

    /// <summary>
    /// ��������
    /// </summary>
    public string saveDate;
    /// <summary>
    /// ����ʱ��
    /// </summary>
    public string saveTime;
   
    public ProfileSaveData(string ProfileID)
    {
        this.ProfileID = ProfileID;
        
        //SaveTime=DateTime.Now;
    }

    /// <summary>
    /// ���´浵
    /// </summary>
    public bool isNewProfile = true;

    /// <summary>
    /// �浵����
    /// </summary>
    public string profileName;

    /// <summary>
    ///��ǰ�ؿ���
    /// </summary>
    public int lastestLevel=0;



    /// <summary>
    /// �ؿ�����
    /// </summary>
    public List<LevelSaveData> levelDatas=new List<LevelSaveData>();

    /// <summary>
    /// ���¹ؿ�����
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
    /// �ɾ�����б�
    /// </summary>
    public List<int> achievementsIDList = new List<int>(50) { 0 };

    /// <summary>
    /// �ɾʹ��״̬�б�
    /// </summary>
    public List<bool> achievementsStateList = new List<bool>(50) { false };

    /// <summary>
    /// �޸Ĵ浵����
    /// </summary>
    /// <param name="name"></param>
    public void GetProfileName(string name)
    {
        profileName = name;
    }

    public void Init() 
    {
        UnityEngine.Debug.Log("��ӹ����");
        for (int i = 1; i < 5; i++)
        {
            levelDatas.Add(new LevelSaveData(i));
        }
        UnityEngine.Debug.Log(levelDatas.Count);
    }
    public void GetTime() 
    {

        // ��ȡ��ǰ���ں�ʱ��
        DateTime currentDateTime = DateTime.Now;


        // ��ȡ��ǰ���ڣ���-��-�գ�
        string date = currentDateTime.ToString("yyyy/MM/dd");

        // ��ȡ��ǰʱ�䣨ʱ:��:�룩
        string time = currentDateTime.ToString("HH:mm");

        saveDate = date;
        saveTime = time;
    }


    /// <summary>
    /// ��ɹؿ��������������
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
        
        //�������߲Ÿ���
        if(level> levelDatas[trueIndex].levelLevel) 
        {
            levelDatas[trueIndex].levelLevel = level;
        }
        //�������߲Ÿ���
        if(highestScore> levelDatas[trueIndex].levelHighestScore) 
        {
            levelDatas[trueIndex].levelHighestScore = highestScore;
        }

    }

    /// <summary>
    /// ����³ɾ�
    /// </summary>
    public void GetNewAchievement(int achievementID)
    {
       achievementsIDList[achievementID] = achievementID;
       achievementsStateList[achievementID] = true;
    }
}

[Serializable]
/// <summary>
/// ��������-�ؿ�����
/// </summary>
public class LevelSaveData 
{
    /// <summary>
    /// �ؿ�ID
    /// </summary>
    public int levelID;

    /// <summary>
    /// �ؿ�����
    /// </summary>
    public E_LevelLevel levelLevel=E_LevelLevel.None;

    /// <summary>
    /// �ؿ���ʷ��߷�
    /// </summary>
    public int levelHighestScore;
    
    /// <summary>
    /// 
    /// </summary>
    public bool isUnLock=false;

    /// <summary>
    /// �������ؿ�
    /// </summary>
    public void UnLockSelf() 
    {
       isUnLock = true;
    }

    /// <summary>
    /// ��ɱ��ؿ�
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
