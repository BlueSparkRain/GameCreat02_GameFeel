using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    [Header("��ҵ÷��ı�")]
    public TMP_Text playerScoreText;
    [Header("��������ı�")]
    public TMP_Text playerLevelText;

    [Header("���ʧ���ı�")]
    public TMP_Text lostText;

    bool canTrigger = true;

    public Transform IsHistoryHightestScaler;

    [Header("���水ť")]
    public Button RePlayButton;
    [Header("���㰴ť")]
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
        SceneLoadManager.Instance.TransToLoadScene(GameLevelCheckManager.Instance.currentLevelIndex+2,E_SceneTranType.��������);
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
        SceneLoadManager.Instance.TransToLoadScene(2,E_SceneTranType.��������);
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
        yield return uiTweener.UIEaseInFrom(E_Dir.��, transform, UIRoot, transTime);
        canTrigger = true;
    }

    public override void HidePanel()
    {
        base.HidePanel();
        canTrigger = false;
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.��, transform, UIRoot, transTime);
    }

    public void GetPlayerData(E_LevelLevel level, int playerFinalScore)
    {
        playerScoreText.transform.parent.gameObject.SetActive(true);
        playerLevelText.transform.parent.gameObject.SetActive(true);
        playerScoreText.text = playerFinalScore.ToString();
        playerLevelText.text = level.ToString();
    }
}
