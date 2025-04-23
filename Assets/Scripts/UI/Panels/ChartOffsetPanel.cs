using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChartOffsetPanel : BasePanel
{

    [Header("当前偏移")]
    public float currentChartOffset;//+40ms,-20ms

    [Header("偏移单位[默认10ms]")]
    public float chartOffsetUnitValue=10;//10ms

    [Header("左偏移按钮")]
    public Button LeftCartOffsetButton;
    [Header("右偏移按钮")]
    public Button RightCartOffsetButton;

    [Header("启用卡拍音效开关")]
    public Toggle EnableHitSoundTog;

    [Header("背景亮度")]
    public Slider BackGroundDimSlider;
    [Header("音乐大小")]
    public Slider MusicVolumeSlider;

    ChartCheckManager chartCheckManager;

    public void OnClickChartOffsetButton(bool isLeftButton) 
    {
        //左偏移
        if (isLeftButton)
            currentChartOffset -= chartOffsetUnitValue;
        //右偏移
        else 
            currentChartOffset += chartOffsetUnitValue;

        //将UI调整数据同步至全局
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
        yield return uiTweener.UIEaseOutTo(E_Dir.左, transform, UIRoot, transTime);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return uiTweener.UIEaseInFrom(E_Dir.左, transform, UIRoot, transTime);
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
