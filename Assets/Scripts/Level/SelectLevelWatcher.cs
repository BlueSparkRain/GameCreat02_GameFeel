using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevelWatcher : MonoBehaviour
{
    List<LevelSelectScreen> tvs=new List<LevelSelectScreen>();
    ProfileSaveData profileData;

    private void Awake()
    {
        StartCoroutine(MusicManager.Instance.PLayNewBK("LEVEL-Ë®µÎ"));
        GameProfileSaveManager.Instance.UnLockNewLevel(GameProfileSaveManager.Instance.ProfileSaveData.lastestLevel+1);
        //µ÷ÊÔ´úÂë
        //GameProfileSaveManager.Instance.UnLockNewLevel(1);
        //GameProfileSaveManager.Instance.UnLockNewLevel(2);
        //GameProfileSaveManager.Instance.UnLockNewLevel(3);
        //GameProfileSaveManager.Instance.UnLockNewLevel(4);
        Time.timeScale = 1.0f;
        for (int i = 0; i < transform.childCount; i++) 
        {
            tvs.Add(transform.GetChild(i).GetComponentInChildren<LevelSelectScreen>());
        }
        //UpdateHistoryTV();
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
    yield return null;
        UpdateHistoryTV();
    }

    void UpdateHistoryTV() 
    {
        profileData = GameProfileSaveManager.Instance.ProfileSaveData;
        for (int i = 0; i < profileData.lastestLevel; i++) 
        {
            tvs[i].UnLockSelf();
            tvs[i].UpdateLevelAppearence((int) profileData.levelDatas[i].levelLevel);
        }
    }
}
