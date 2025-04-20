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
    /// ���µĹؿ����
    /// </summary>
    public int lastestLevelIndex;

    [Header("�������˵���ť")]
    public Button ReturnButton;

    GameProfileSaveManager profileSaveManager;

    /// <summary>
    /// �������µĹؿ�
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
        //��ȡ�浵���ݣ��������йؿ�����
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
        //Ԥ����
        if (profileSaveManager = null)
            profileManager = GameProfileSaveManager.Instance;

        //ע�����а�ť
        for (int i = 0; i < ButtonContainer.childCount; i++)
        {
            LevelSelectScreens.Add(ButtonContainer.GetChild(i).GetComponentInChildren<LevelSelectScreen>());
        }

        //����˳���������ݴ浵���ݣ���ʾ�ؿ�����
        LoadCurrentProgress();


        ReturnButton.onClick.AddListener(
        () => SceneLoadManager.Instance.TransToLoadScene(0,E_SceneTranType.����ͼ����));
    }



    //����������ֱ�Ӱ���һ�ؽ�������������ͨ��ʱ
    /// <summary>
    /// �����¹ؿ�[�ӹؿ����� ���غ� �ص�����ѡ�񳡾������Ƚ����¹ؿ�]
    /// </summary>
    /// <param name="Index"></param>
    public IEnumerator UnLockNewLevel()
    {
        lastestLevelIndex++;
        yield return new WaitForSeconds(2);
        StartCoroutine(LevelSelectScreens[lastestLevelIndex].UnLockSelf());
    }
}
