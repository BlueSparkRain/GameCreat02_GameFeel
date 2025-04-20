using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeSettingPanel : BasePanel
{
    #region 手柄震动配置
    //手柄震动
    [Header("手柄震动开关")]
    public Toggle gamePadShakeTog;
    [Header("手柄震动强度")]
    public Slider gaemPadIntensity;
    [Header("手柄震动频率")]
    public Slider gaemPadFrequency;
    #endregion

    #region 屏幕震动配置
    //屏幕震动
    [Header("屏幕震动开关")]
    public Toggle screenShakeTog;
    [Header("屏幕震动强度")]
    public Slider screenIntensity;
    [Header("屏幕震动频率")]
    public Slider screenFrequency;
    [Header("屏幕震动时长")]
    public Slider screenDuration;
    #endregion

    CameraShakeManager  camShakeManager;
    GamepadShakeManager gamepadShakeManager;


    protected override void Init()
    {
        base.Init();
        if (!initSelf)
        {
            initSelf = true;
            camShakeManager=CameraShakeManager.Instance;
            gamepadShakeManager=GamepadShakeManager.Instance;

            screenShakeTog.onValueChanged.AddListener( bol => camShakeManager.SwitchShake());
            gamePadShakeTog.onValueChanged.AddListener( bol => gamepadShakeManager.SwitchShake());

            //gamePadShakeTog.onValueChanged.AddListener(val=>   );


        }
    }


    public override void HidePanel()
    {
        base.HidePanel();
    }

    public override IEnumerator HidePanelTweenEffect()
    {
        yield return uiTweener.UIEaseOutTo(E_Dir.下, transform, UIRoot, transTime);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    public override IEnumerator ShowPanelTweenEffect()
    {
        yield return uiTweener.UIEaseInFrom(E_Dir.下, transform, UIRoot, transTime);
    }

 
}
