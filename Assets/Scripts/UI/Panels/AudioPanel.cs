using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioPanel : BasePanel
{
    [Header("��Ч����������")]
    public Slider SFXSlder;
    [Header("������������������")]
    public Slider BGMSlder;

    [Header("����")]
    public Transform Root;

    [Header("���ذ�ť")]
    public Button ReturnButton;

    public override void HidePanel()
    {
        base.HidePanel();
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

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return UITween.Instance.UIDoFade(transform, 1, 0, transTime / 2);
        yield return UITween.Instance.UIDoLocalMove(Root, new Vector2(-2000, 0), transTime / 2);

    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return UITween.Instance.UIDoLocalMove(Root, new Vector2(2000, 0), transTime / 2);
        StartCoroutine(UITween.Instance.UIDoFade(transform, 0, 1, transTime / 2));
    }



    protected override void Init()
    {
        base.Init();
        SFXSlder.onValueChanged.AddListener(OnSFXSliderValueChnage);
        BGMSlder.onValueChanged.AddListener(OnBGMSliderValueChnage);
        ReturnButton.onClick.AddListener(OnClickReturnButton);
    }
}
