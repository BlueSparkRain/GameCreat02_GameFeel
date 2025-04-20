using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    [Header("玩家用时文本")]
    public TMP_Text playerUsedTimeText;
    [Header("玩家得分文本")]
    public TMP_Text playerScoreText;
    [Header("玩家评级文本")]
    public TMP_Text playerLevelText;

    [Header("玩家失败文本")]
    public TMP_Text lostText;

    bool canVer=true;

    int highestStarNum;

    public Transform HightScaleStar;

    IEnumerator HightStarScale() 
    {
        yield return new WaitForSeconds(1);
        StartCoroutine( TweenHelper.MakeLerp(Vector3.zero, Vector3.one*1.2f, 0.1f, val => HightScaleStar.localScale = val));
        yield return TweenHelper.MakeLerp(Vector3.zero, new Vector3(0, 0, 5.8f), 0.08f, val => HightScaleStar.eulerAngles = val);
        yield return TweenHelper.MakeLerp(new Vector3(0, 0, 5.8f), new Vector3(0, 0, -5.8f), 0.08f, val => HightScaleStar.eulerAngles = val);
        yield return TweenHelper.MakeLerp(new Vector3(0, 0, -5.8f), Vector3.zero, 0.08f, val => HightScaleStar.eulerAngles = val);
    }

    void OnClickRePlayButton()
    {
        if (!canVer)
        {
            return;
        }
        canVer = false;
        MusicManager.Instance.StopBKMusic();

        highestStarNum = 0;
        uiManager.HidePanel<GameOverPanel>();
    }

  

    public void LostGame(int playerFinalScore)
    {
        playerScoreText.transform.parent.gameObject.SetActive(true);
        playerScoreText.text = playerFinalScore.ToString();
        lostText.transform.parent.gameObject.SetActive(true);
        lostText.text = "Next Time!";
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

    public override void ShowPanel()
    {
        base.ShowPanel();
        StartCoroutine(HightStarScale());
    }
    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return uiTweener.UIEaseInFrom(E_Dir.上, transform, UIRoot, transTime);
        canVer = true;
    }

    public override void HidePanel()
    {
        base.HidePanel();
        canVer = false;
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseInFrom(E_Dir.上, transform, UIRoot, transTime);
        canVer = true;
    }

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
}
