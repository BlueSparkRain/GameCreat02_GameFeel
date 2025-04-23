using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChartOffsetPanel : BasePanel
{

    [Header("��ǰƫ��")]
    public float currentChartOffset;//+40ms,-20ms

    [Header("ƫ�Ƶ�λ[Ĭ��10ms]")]
    public float chartOffsetUnitValue=10;//10ms

    [Header("��ƫ�ư�ť")]
    public Button LeftCartOffsetButton;
    [Header("��ƫ�ư�ť")]
    public Button RightCartOffsetButton;

    [Header("���ÿ�����Ч����")]
    public Toggle EnableHitSoundTog;

    [Header("��������")]
    public Slider BackGroundDimSlider;
    [Header("���ִ�С")]
    public Slider MusicVolumeSlider;

    ChartCheckManager chartCheckManager;

    public void OnClickChartOffsetButton(bool isLeftButton) 
    {
        //��ƫ��
        if (isLeftButton)
            currentChartOffset -= chartOffsetUnitValue;
        //��ƫ��
        else 
            currentChartOffset += chartOffsetUnitValue;

        //��UI��������ͬ����ȫ��
        chartCheckManager.SetChartOffsetValue(currentChartOffset);
    }

    void BackgroundDimOnValueChange(float val) {
   
    } 

    void MusicVolumeOnValueChange(float val) {
    
    }

    void OnHitSoundTogChange(bool enable) { 
    
    }
 
    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.��, transform, UIRoot, transTime);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return uiTweener.UIEaseInFrom(E_Dir.��, transform, UIRoot, transTime);
    }

    protected override void Init()
    {
        base.Init();
        if (!initSelf) 
        {
            chartCheckManager= ChartCheckManager.Instance;
            EnableHitSoundTog.onValueChanged.AddListener(OnHitSoundTogChange);
            BackGroundDimSlider.onValueChanged.AddListener(BackgroundDimOnValueChange);
            MusicVolumeSlider.onValueChanged.AddListener(MusicVolumeOnValueChange);
        }
    }
}
