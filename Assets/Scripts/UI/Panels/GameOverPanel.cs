using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    [Header("玩家得分文本")]
    public TMP_Text playerScoreText;
    [Header("玩家评级文本")]
    public TMP_Text playerLevelText;

    [Header("玩家失败文本")]
    public TMP_Text lostText;

    bool canTrigger = true;

    public Transform IsHistoryHightestScaler;

    [Header("重玩按钮")]
    public Button RePlayButton;
    [Header("结算按钮")]
    public Button ContinueButton;

    IEnumerator HightStarScale()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(TweenHelper.MakeLerp(Vector3.zero, Vector3.one * 1.2f, 0.1f, val => IsHistoryHightestScaler.localScale = val));
        yield return TweenHelper.MakeLerp(Vector3.zero, new Vector3(0, 0, 5.8f), 0.08f, val => IsHistoryHightestScaler.eulerAngles = val);
        yield return TweenHelper.MakeLerp(new Vector3(0, 0, 5.8f), new Vector3(0, 0, -5.8f), 0.08f, val => IsHistoryHightestScaler.eulerAngles = val);
        yield return TweenHelper.MakeLerp(new Vector3(0, 0, -5.8f), Vector3.zero, 0.08f, val => IsHistoryHightestScaler.eulerAngles = val);
    }

    void OnClickRePlayButton()
    {
        Debug.Log("777");
        EventCenter.Instance.EventTrigger(E_EventType.E_CurrentLevelOver);
        if (!canTrigger)
        {
            return;
        }
        canTrigger = false;
        SceneLoadManager.Instance.TransToLoadScene(GameLevelCheckManager.Instance.currentLevelIndex+2,E_SceneTranType.黑屏过渡);
        uiManager.HidePanel<GameOverPanel>();
    }

    void OnClickContinueButton() 
    {
        EventCenter.Instance.EventTrigger(E_EventType.E_CurrentLevelOver);
        if (!canTrigger)
        {
            return;
        }
        canTrigger = false;
        SceneLoadManager.Instance.TransToLoadScene(2,E_SceneTranType.黑屏过渡);
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
        ContinueButton.onClick.AddListener(OnClickContinueButton);
        RePlayButton.onClick.AddListener(OnClickRePlayButton);
        lostText.transform.parent.gameObject.SetActive(false);
        playerScoreText.transform.parent.gameObject.SetActive(false);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        EventCenter.Instance.EventTrigger(E_EventType.E_CurrentLevelOver);
        StartCoroutine(HightStarScale());
    }
    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return uiTweener.UIEaseInFrom(E_Dir.上, transform, UIRoot, transTime);
        canTrigger = true;
    }

    public override void HidePanel()
    {
        base.HidePanel();
        canTrigger = false;
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.上, transform, UIRoot, transTime);
    }

    public void GetPlayerData(E_LevelLevel level, int playerFinalScore)
    {
        playerScoreText.transform.parent.gameObject.SetActive(true);
        playerLevelText.transform.parent.gameObject.SetActive(true);
        playerScoreText.text = playerFinalScore.ToString();
        playerLevelText.text = level.ToString();
    }
}
