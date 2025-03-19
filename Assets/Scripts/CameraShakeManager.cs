using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理与所有相机晃动相关逻辑
/// </summary>
public class CameraShakeManager : MonoSingleton<CameraShakeManager>
{
    private CameraShakeSO shakeProfile;
    [Header("一般是当前激活的虚拟相机上挂载震动监听")]
    private CinemachineImpulseListener impulseListener;
    private CinemachineImpulseDefinition impulseDefinition;
    //每个source对应一种震动文件
    private Dictionary<string, CameraShakeSO> sourecDic = new Dictionary<string, CameraShakeSO>();


    protected override void InitSelf()
    {
        base.InitSelf();
        impulseListener=FindAnyObjectByType<CinemachineImpulseListener>();
    }

    /// <summary>
    /// 在游戏开始后，所有具有ShakeSource的对象都会初始化先调用此方法自动向CameraShakeManager登记自身的profile
    /// </summary>
    public void AddNewShakeSourceListender(string sourceName, CameraShakeSO source)
    {
        if (!sourecDic.ContainsKey(sourceName))
        {
            sourecDic.Add(sourceName, source);
            //Debug.Log("添加" + source.name);
        }
    }

    /// <summary>
    /// 由ShakeSource调用此方法发出震动，虚拟相机上的ShakeListener获取震动监听
    /// </summary>
    /// <param name="profile"></param>
    /// <param name="source"></param>
    public void ExcuteCameraShake(string sourceName, CinemachineImpulseSource source)
    {
        //先判断目标是否存在，不存在先加载资源
        AddNewShakeSourceListender(sourceName, Resources.Load<CameraShakeSO>("SOData/CameraShakeSO/" + sourceName));
        CameraShakeSO profile = ScriptableObject.CreateInstance(typeof(CameraShakeSO)) as CameraShakeSO;
        if (!sourecDic.ContainsKey(sourceName))
            return;
        profile = sourecDic[sourceName];
        SetShakeProfile(profile, source);
        source.GenerateImpulseWithForce(profile.imapctForce);
    }

    void SetShakeProfile(CameraShakeSO profile, CinemachineImpulseSource source)
    {
        impulseDefinition = source.m_ImpulseDefinition;
        //设置震动Source属性
        impulseDefinition.m_ImpulseDuration = profile.impactTime;
        source.m_DefaultVelocity = profile.impactVelocity;
        impulseDefinition.m_CustomImpulseShape = profile.impactCurve;
        //设置震动Listener属性
        if (impulseListener != null)
        {
            impulseListener.m_ReactionSettings.m_AmplitudeGain = profile.listenerAmplitude;
            impulseListener.m_ReactionSettings.m_FrequencyGain = profile.listenerFrequency;
            impulseListener.m_ReactionSettings.m_Duration = profile.listenerDuration;
        }
    }
}
