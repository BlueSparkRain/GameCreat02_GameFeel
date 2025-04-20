using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeSettingPanel : BasePanel
{
    #region �ֱ�������
    //�ֱ���
    [Header("�ֱ��𶯿���")]
    public Toggle gamePadShakeTog;
    [Header("�ֱ���ǿ��")]
    public Slider gaemPadIntensity;
    [Header("�ֱ���Ƶ��")]
    public Slider gaemPadFrequency;
    #endregion

    #region ��Ļ������
    //��Ļ��
    [Header("��Ļ�𶯿���")]
    public Toggle screenShakeTog;
    [Header("��Ļ��ǿ��")]
    public Slider screenIntensity;
    [Header("��Ļ��Ƶ��")]
    public Slider screenFrequency;
    [Header("��Ļ��ʱ��")]
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

 
}
