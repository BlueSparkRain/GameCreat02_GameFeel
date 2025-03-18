
using UnityEngine;

[CreateAssetMenu(fileName = "CameraShackData", menuName = "MyCustomData/CameraShakeSO")]
public class CameraShakeSO : ScriptableObject
{
    [Header("震动时间")]
    public float impactTime = 0.5f;
    [Header("震动力度")]
    public float imapctForce = 1;
    [Header("震动（力）方向")]
    public Vector3 impactVelocity;
    [Header("震动曲线")]
    public AnimationCurve impactCurve;
    [Header("震动幅度")]
    public float listenerAmplitude = 1;
    [Header("震动频率")]
    public float listenerFrequency = 1;
    [Header("震动持续")]
    public float listenerDuration = 1;


}
