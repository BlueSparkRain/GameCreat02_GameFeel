
using UnityEngine;

[CreateAssetMenu(fileName = "CameraShackData", menuName = "MyCustomData/CameraShakeSO")]
public class CameraShakeSO : ScriptableObject
{
    [Header("��ʱ��")]
    public float impactTime = 0.5f;
    [Header("������")]
    public float imapctForce = 1;
    [Header("�𶯣���������")]
    public Vector3 impactVelocity;
    [Header("������")]
    public AnimationCurve impactCurve;
    [Header("�𶯷���")]
    public float listenerAmplitude = 1;
    [Header("��Ƶ��")]
    public float listenerFrequency = 1;
    [Header("�𶯳���")]
    public float listenerDuration = 1;


}
