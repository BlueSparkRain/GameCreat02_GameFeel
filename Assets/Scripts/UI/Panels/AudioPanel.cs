using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioPanel : BasePanel
{
    [Header("音效音量滑动条")]
    public Slider SFXSlder;
    [Header("背景音乐音量滑动条")]
    public Slider BGMSlder;

    [Header("面板根")]
    public Transform Root;

    [Header("返回按钮")]
    public Button ReturnButton;

    float changeTimer;
    float cahngeInterval = 0.5f;
    bool canChange;

    private void Update()
    {
        if (changeTimer >= 0)
            changeTimer -= Time.deltaTime;
        else
            canChange = true;

        if (canChange && PlayerInputManager.Instance.currentUISelectGameObj && PlayerInputManager.Instance.currentUISelectGameObj.GetComponent<Slider>()) 
        {
            Debug.Log("65e211eegdsa");
            if (PlayerInputManager.Instance.SliderValue.x != 0 && canChange)
            {
                canChange = false;
                changeTimer = cahngeInterval;
                PlayerInputManager.Instance.currentUISelectGameObj.GetComponent<Slider>().value += PlayerInputManager.Instance.SliderValue.x * 0.05f;
            }
        }
    }

    public override void HidePanel()
    {
        base.HidePanel();
    }

    /// <summary>
    /// 背景音乐音量控制
    /// </summary>
    /// <param name="v"></param>
    void OnBGMSliderValueChnage(float v) =>
      MusicManager.Instance.ChangeBKMusicValue(v);

    /// <summary>
    /// 音效音乐音量控制
    /// </summary>
    /// <param name="v"></param>
    void OnSFXSliderValueChnage(float v) =>
        MusicManager.Instance.ChangeSoundValue(v);
    void OnClickReturnButton() =>
      UIManager.Instance.HidePanel<AudioPanel>();

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return UITween.Instance.UIDoMove(Root, Vector2.zero, new Vector2(2000, 0), transTime/3);
        yield return UITween.Instance.UIDoFade(transform, 1, 0, transTime/3);
        yield return base.HidePanelTweenEffect();

    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        StartCoroutine(UITween.Instance.UIDoFade(transform, 0, 1, transTime / 2));
        yield return UITween.Instance.UIDoMove(Root, new Vector2(-2000, 0 ),Vector2.zero,transTime / 2);
    }

    protected override void Init()
    {
        base.Init();
        SFXSlder.onValueChanged.AddListener(OnSFXSliderValueChnage);
        BGMSlder.onValueChanged.AddListener(OnBGMSliderValueChnage);
        ReturnButton.onClick.AddListener(OnClickReturnButton);
        //EventSystem.current.SetSelectedGameObject(BGMSlder.gameObject);
    }


    public override void GamePadClose()
    {
        base.GamePadClose();
       
        UIManager.Instance.HidePanel<AudioPanel>();
    }
}
