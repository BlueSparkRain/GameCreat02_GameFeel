using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public Transform root;

    [Header("玩家用时文本")]
    public TMP_Text playerUsedTimeText;
    [Header("玩家得分文本")]
    public TMP_Text playerScoreText;
    [Header("玩家评级文本")]
    public TMP_Text playerLevelText;

    [Header("玩家失败文本")]
    public TMP_Text lostText;

    bool canVer=true;

    void OnClickRePlayButton()
    {
        if (!canVer)
        {
            return;
        }
        canVer = false;
        Debug.Log("重新加载游戏");
        SceneLoadManager.Instance.EndOneLevel(highestStarNum);
        highestStarNum = 0;
        UIManager.Instance.HidePanel<GameOverPanel>();
    }

    public override void HidePanel()
    {
        base.HidePanel();
        canVer=false;
    }

    public void LostGame(int playerFinalScore)
    {
        playerScoreText.transform.parent.gameObject.SetActive(true);
        playerScoreText.text = playerFinalScore.ToString();
        lostText.transform.parent.gameObject.SetActive(true);
        lostText.text = "Next Time!";
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return UITween.Instance.UIDoMove(root, Vector2.zero, new Vector2(0, -1500), transTime);
        yield return UITween.Instance.UIDoFade(transform, 1, 0, transTime);
        canVer = true;

    }

    int highestStarNum;

 

    public void GetPlayerData(float playerUsedTime, int playerFinalScore,int highestStarNum)
    {
        playerUsedTimeText.transform.parent.gameObject.SetActive(true);
        playerScoreText.transform.parent.gameObject.SetActive(true);
        playerLevelText.transform.parent.gameObject.SetActive(true);
        playerUsedTimeText.text = playerUsedTime.ToString();
        playerScoreText.text = playerFinalScore.ToString();
        playerLevelText.text = "A";
        this.highestStarNum = highestStarNum;
    }


    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return UITween.Instance.UIDoMove(root, new Vector2(0, -1500), Vector2.zero, transTime);
        yield return UITween.Instance.UIDoFade(transform, 0, 1, transTime);
        canVer = true;
    }

    protected override void Init()
    {
        base.Init();
        FirstSelectButton.GetComponent<Button>().onClick.AddListener(OnClickRePlayButton);
        lostText.transform.parent.gameObject.SetActive(false);
        playerUsedTimeText.transform.parent.gameObject.SetActive(false);
        playerScoreText.transform.parent.gameObject.SetActive(false);
        playerUsedTimeText.transform.parent.gameObject.SetActive(false);
    }

    public override void GamePadClose()
    {
        base.GamePadClose();
    }
}
