using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelecter : MonoBehaviour
{
    public List<LevelSelectScreen> LevelSelectScreens = new List<LevelSelectScreen>();
    public Transform ButtonContainer;
    public Transform Root;

    /// <summary>
    /// 最新的关卡序号
    /// </summary>
    public int lastestLevelIndex;

    [Header("返回主菜单按钮")]
    public Button ReturnButton;

    GameProfileSaveManager profileSaveManager;

    /// <summary>
    /// 设置最新的关卡
    /// </summary>
    public void SetLastetLevelIndex(int index)
    {
        lastestLevelIndex = index;
    }
  
   
    public IEnumerator ShowLevelSelector()
    {
        MusicManager.Instance.PlayBKMusic("BK1");

        yield return UITween.Instance.UIDoFade(transform, 0, 1, 0.5f);
        yield return UITween.Instance.UIDoLocalMove(Root, new Vector2(0, 2000), 0.3f);
    }

    public IEnumerator HideLevelSelector()
    {
        transform.GetComponent<CanvasGroup>().interactable = false;
        yield return UITween.Instance.UIDoLocalMove(Root, new Vector2(0, -2000), 0.3f);
        yield return UITween.Instance.UIDoFade(transform, 1, 0, 0.5f);
    }

    GameProfileSaveManager profileManager;
    LevelSaveData levelData;
    void LoadCurrentProgress()
    {
        //获取存档数据，加载所有关卡进度
        for (int i = 0; i < LevelSelectScreens.Count; i++)
        {
            levelData = profileManager.ProfileSaveData.levelDatas[i];

            LevelSelectScreens[i].InitSelf(
                            levelData.levelLevel,
                            levelData.levelHighestScore,
                            levelData.isUnLock
                            );
        }
    }

    void Start()
    {
        //预缓存
        if (profileSaveManager = null)
            profileManager = GameProfileSaveManager.Instance;

        //注册所有按钮
        for (int i = 0; i < ButtonContainer.childCount; i++)
        {
            LevelSelectScreens.Add(ButtonContainer.GetChild(i).GetComponentInChildren<LevelSelectScreen>());
        }

        //进入此场景，会根据存档数据，显示关卡进度
        LoadCurrentProgress();


        ReturnButton.onClick.AddListener(
        () => SceneLoadManager.Instance.TransToLoadScene(0,E_SceneTranType.过场图过渡));
    }



    //待定，可以直接把下一关解锁的条件放在通关时
    /// <summary>
    /// 解锁新关卡[从关卡场景 过关后 回到场景选择场景，会先解锁新关卡]
    /// </summary>
    /// <param name="Index"></param>
    public IEnumerator UnLockNewLevel()
    {
        lastestLevelIndex++;
        yield return new WaitForSeconds(2);
        StartCoroutine(LevelSelectScreens[lastestLevelIndex].UnLockSelf());
    }
}
