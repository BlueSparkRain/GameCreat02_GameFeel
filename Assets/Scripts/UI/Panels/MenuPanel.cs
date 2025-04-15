using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : BasePanel
{
    [Header("���水ť")]
    public Button PlayButton;
    [Header("ͼ����ť")]
    public Button BookButton;
    [Header("���ð�ť")]
    public Button SettingButton;
    [Header("��л��ť")]
    public Button ThankYouButton;
    [Header("�˳���ť")]
    public Button QuitButton;

    [Header("����")]
    public Transform root;

    void OnClickPlayButton()
    {
        UIManager.Instance.ShowPanel<SceneTransPanel>(panel => panel.SceneLoadingTrans(1));
        UIManager.Instance.HidePanel<MenuPanel>();
    }

    void OnClickQuitButton()
    {
        Application.Quit();
    }

    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return UITween.Instance.UIDoMove(root, Vector2.zero, new Vector2(0, -2000), transTime / 3);
        yield return UITween.Instance.UIDoFade(transform, 1, 0, transTime / 2);
        GetComponent<CanvasGroup>().interactable = false;
    }


    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 0, 1, transTime / 3);
        yield return UITween.Instance.UIDoMove(root, new Vector2(0, -2000), Vector2.zero, transTime / 2);
    }

    protected override void Init()
    {
        base.Init();
        //PlayButton.onClick.AddListener(OnClickPlayButton);
        //QuitButton.onClick.AddListener(OnClickQuitButton);


        PlayButton.onClick.AddListener(() =>  {

            Debug.Log("What going on");
            uiManager.ShowPanel<TestPanel>(null,true); });


        StartCoroutine(WaitUnLock());
    }

    bool  Ilock;

    IEnumerator WaitUnLock()
    {
        Ilock=true;
        yield return new WaitForSeconds(1);
        Ilock=false;
        GetComponent<CanvasGroup>().interactable = true;
    }
    private void Update()
    {
        //if(uiManager!=null)
        //Debug.Log(uiManager.currentPanel);

        if (Ilock) 
        {
            GetComponent<CanvasGroup>().interactable = false;
        }
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
        
        }
    }
    public override void GamePadClose()
    {
        base.GamePadClose();
    }
}
