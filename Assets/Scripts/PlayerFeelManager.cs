using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFeelManager : MonoSingleton<PlayerFeelManager>
{
    [Header("�ƶ�-��������ٶ�")]
    public float MoveLowSpeed = 1;
    [Header("�ƶ�-��������ٶ�")]
    public float MoveHighSpeed = 0;
    [Header("�ƶ�-�𶯳���ʱ��")]
    public float MoveDuration = 0.2f;

    [Header("Ⱦɫ-��������ٶ�")]
    public float ColorationLowSpeed = 0.5f;
    [Header("Ⱦɫ-��������ٶ�")]
    public float ColorationHighSpeed = 0.5f;
    [Header("Ⱦɫ-��ʱ��")]
    public float ColorationDuration = 0.3f;

    void Start()
    {
        // ���������¼�
        PlayerInputManager.Instance.playerInput.GamePlay.LeftMove.started += ctx => GamepadVibrate(MoveLowSpeed,MoveHighSpeed, MoveDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.RightMove.started += ctx => GamepadVibrate(MoveLowSpeed,MoveHighSpeed, MoveDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.UpMove.started += ctx => GamepadVibrate(MoveLowSpeed,MoveHighSpeed, MoveDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.DownMove.started += ctx => GamepadVibrate(MoveLowSpeed,MoveHighSpeed, MoveDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.LeftMove.started += ctx => ExcuteCameraShake("ShakeLeftM");
        PlayerInputManager.Instance.playerInput.GamePlay.RightMove.started += ctx => ExcuteCameraShake("ShakeRightM");
        PlayerInputManager.Instance.playerInput.GamePlay.UpMove.started += ctx => ExcuteCameraShake("ShakeUpM");
        PlayerInputManager.Instance.playerInput.GamePlay.DownMove.started += ctx => ExcuteCameraShake("ShakeDownM");

        PlayerInputManager.Instance.playerInput.GamePlay.LeftColoration.started += ctx => GamepadVibrate(ColorationLowSpeed,ColorationHighSpeed, ColorationDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.RightColoration.started += ctx => GamepadVibrate(ColorationLowSpeed,ColorationHighSpeed, ColorationDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.UpColoration.started += ctx => GamepadVibrate(ColorationLowSpeed,ColorationHighSpeed, ColorationDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.DownColoration.started += ctx => GamepadVibrate(ColorationLowSpeed,ColorationHighSpeed, ColorationDuration);

        PlayerInputManager.Instance.playerInput.GamePlay.LeftColoration.started += ctx => ExcuteCameraShake("ShakeLeft");
        PlayerInputManager.Instance.playerInput.GamePlay.RightColoration.started += ctx => ExcuteCameraShake("ShakeRight");
        PlayerInputManager.Instance.playerInput.GamePlay.UpColoration.started += ctx => ExcuteCameraShake("ShakeUp");
        PlayerInputManager.Instance.playerInput.GamePlay.DownColoration.started += ctx => ExcuteCameraShake("ShakeDown");
    }
    public void GamepadVibrate(float low, float high, float time) => StartCoroutine(IEGamepadVibrate(low, high, time));

    public IEnumerator IEGamepadVibrate(float low, float high, float time)
    {

        if (Gamepad.current == null)
            yield break;

        //�����ֱ��� ���ٶ� �Լ� �ָ��� , ��ʱ����֮����ͣ��
        Gamepad.current.SetMotorSpeeds(low, high);
        Gamepad.current.ResumeHaptics();
        var endTime = Time.time + time;

        while (Time.time < endTime)
        {
            Gamepad.current?.ResumeHaptics();
            yield return null;
        }
        if (Gamepad.current == null)
            yield break;
        Gamepad.current.PauseHaptics();
    }

    void ExcuteCameraShake(string resName) 
    {
        CameraShakeManager.Instance.ExcuteCameraShake(resName, GetComponentInChildren<CinemachineImpulseSource>());
        //Debug.Log("�ֱ��𶯣�");
    }

    void OnDisable()
    {
        // ȡ�����������¼�
        PlayerInputManager.Instance.playerInput.GamePlay.LeftMove.started -= ctx => GamepadVibrate(MoveLowSpeed, MoveHighSpeed, MoveDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.RightMove.started -= ctx => GamepadVibrate(MoveLowSpeed, MoveHighSpeed, MoveDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.UpMove.started -= ctx => GamepadVibrate(MoveLowSpeed, MoveHighSpeed, MoveDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.DownMove.started -= ctx => GamepadVibrate(MoveLowSpeed, MoveHighSpeed, MoveDuration);

        PlayerInputManager.Instance.playerInput.GamePlay.LeftMove.started -= ctx => ExcuteCameraShake("ShakeLeftM");
        PlayerInputManager.Instance.playerInput.GamePlay.RightMove.started -= ctx => ExcuteCameraShake("ShakeRightM");
        PlayerInputManager.Instance.playerInput.GamePlay.UpMove.started -= ctx => ExcuteCameraShake("ShakeUpM");
        PlayerInputManager.Instance.playerInput.GamePlay.DownMove.started -= ctx => ExcuteCameraShake("ShakeDownM");

        PlayerInputManager.Instance.playerInput.GamePlay.LeftColoration.started -= ctx => GamepadVibrate(ColorationLowSpeed, ColorationHighSpeed, ColorationDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.RightColoration.started -= ctx => GamepadVibrate(ColorationLowSpeed, ColorationHighSpeed, ColorationDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.UpColoration.started -= ctx => GamepadVibrate(ColorationLowSpeed, ColorationHighSpeed, ColorationDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.DownColoration.started -= ctx => GamepadVibrate(ColorationLowSpeed, ColorationHighSpeed, ColorationDuration);

        PlayerInputManager.Instance.playerInput.GamePlay.LeftColoration.started -= ctx => ExcuteCameraShake("ShakeLeft");
        PlayerInputManager.Instance.playerInput.GamePlay.RightColoration.started -= ctx => ExcuteCameraShake("ShakeRight");
        PlayerInputManager.Instance.playerInput.GamePlay.UpColoration.started -= ctx => ExcuteCameraShake("ShakeUp");
        PlayerInputManager.Instance.playerInput.GamePlay.DownColoration.started -= ctx => ExcuteCameraShake("ShakeDown");
    }

    private void Update()
    {
    
    }

}
