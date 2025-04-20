using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioPanel : BasePanel
{
    [Header("��Ч����������")]
    public Slider SFXSlder;
    [Header("������������������")]
    public Slider BGMSlder;

    [Header("���ذ�ť")]
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

    /// <summary>
    /// ����������������
    /// </summary>
    /// <param name="v"></param>
    void OnBGMSliderValueChnage(float v) =>
      MusicManager.Instance.ChangeBKMusicValue(v);

    /// <summary>
    /// ��Ч������������
    /// </summary>
    /// <param name="v"></param>
    void OnSFXSliderValueChnage(float v) =>
        MusicManager.Instance.ChangeSoundValue(v);
    void OnClickReturnButton() =>
      UIManager.Instance.HidePanel<AudioPanel>();

    protected override void Init()
    {
        base.Init();
        SFXSlder.onValueChanged.AddListener(OnSFXSliderValueChnage);
        BGMSlder.onValueChanged.AddListener(OnBGMSliderValueChnage);
        ReturnButton.onClick.AddListener(OnClickReturnButton);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return uiTweener.UIEaseInFrom(E_Dir.��, transform, UIRoot, transTime);
    }


    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.��, transform, UIRoot, transTime);
    }
}
