using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������������ζ�����߼�
/// </summary>
public class CameraShakeManager : MonoSingleton<CameraShakeManager>
{
    private CameraShakeSO shakeProfile;
    [Header("һ���ǵ�ǰ�������������Ϲ����𶯼���")]
    private CinemachineImpulseListener impulseListener;
    private CinemachineImpulseDefinition impulseDefinition;
    //ÿ��source��Ӧһ�����ļ�
    private Dictionary<string, CameraShakeSO> sourecDic = new Dictionary<string, CameraShakeSO>();


    protected override void InitSelf()
    {
        base.InitSelf();
        impulseListener=FindAnyObjectByType<CinemachineImpulseListener>();
    }

    /// <summary>
    /// ����Ϸ��ʼ�����о���ShakeSource�Ķ��󶼻��ʼ���ȵ��ô˷����Զ���CameraShakeManager�Ǽ������profile
    /// </summary>
    public void AddNewShakeSourceListender(string sourceName, CameraShakeSO source)
    {
        if (!sourecDic.ContainsKey(sourceName))
        {
            sourecDic.Add(sourceName, source);
            //Debug.Log("���" + source.name);
        }
    }

    /// <summary>
    /// ��ShakeSource���ô˷��������𶯣���������ϵ�ShakeListener��ȡ�𶯼���
    /// </summary>
    /// <param name="profile"></param>
    /// <param name="source"></param>
    public void ExcuteCameraShake(string sourceName, CinemachineImpulseSource source)
    {
        //���ж�Ŀ���Ƿ���ڣ��������ȼ�����Դ
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
        //������Source����
        impulseDefinition.m_ImpulseDuration = profile.impactTime;
        source.m_DefaultVelocity = profile.impactVelocity;
        impulseDefinition.m_CustomImpulseShape = profile.impactCurve;
        //������Listener����
        if (impulseListener != null)
        {
            impulseListener.m_ReactionSettings.m_AmplitudeGain = profile.listenerAmplitude;
            impulseListener.m_ReactionSettings.m_FrequencyGain = profile.listenerFrequency;
            impulseListener.m_ReactionSettings.m_Duration = profile.listenerDuration;
        }
    }
}
