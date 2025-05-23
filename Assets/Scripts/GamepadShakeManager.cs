using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadShakeManager : MonoSingleton<GamepadShakeManager>
{

    bool canGamepadShake;

    public void SwitchShake() 
    {
      canGamepadShake = !canGamepadShake;
    }

    [Header("移动-低震动马达速度")]
    public float MoveLowSpeed = 0;
    [Header("移动-高震动马达速度")]
    public float MoveHighSpeed = 0.2f;
    [Header("移动-震动持续时长")]
    public float MoveDuration = 0.1f;

    [Header("染色-低震动马达速度")]
    public float ColorationLowSpeed = 0.3f;
    [Header("染色-高震动马达速度")]
    public float ColorationHighSpeed = 0.3f;
    [Header("染色-震动时长")]
    public float ColorationDuration = 0.2f;

    public void IMInit()
    {
        InitSelf();
    }

    protected override void InitSelf()
    {
        base.InitSelf();
        PlayerInputManager.Instance.playerInput.GamePlay.LeftMove.started += ctx => GamepadVibrate(MoveLowSpeed, MoveHighSpeed, MoveDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.RightMove.started += ctx => GamepadVibrate(MoveLowSpeed, MoveHighSpeed, MoveDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.UpMove.started += ctx => GamepadVibrate(MoveLowSpeed, MoveHighSpeed, MoveDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.DownMove.started += ctx => GamepadVibrate(MoveLowSpeed, MoveHighSpeed, MoveDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.LeftMove.started += ctx => ExcuteCameraShake("ShakeLeftM");
        PlayerInputManager.Instance.playerInput.GamePlay.RightMove.started += ctx => ExcuteCameraShake("ShakeRightM");
        PlayerInputManager.Instance.playerInput.GamePlay.UpMove.started += ctx => ExcuteCameraShake("ShakeUpM");
        PlayerInputManager.Instance.playerInput.GamePlay.DownMove.started += ctx => ExcuteCameraShake("ShakeDownM");

        PlayerInputManager.Instance.playerInput.GamePlay.LeftColoration.started += ctx => GamepadVibrate(ColorationLowSpeed, ColorationHighSpeed, ColorationDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.RightColoration.started += ctx => GamepadVibrate(ColorationLowSpeed, ColorationHighSpeed, ColorationDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.UpColoration.started += ctx => GamepadVibrate(ColorationLowSpeed, ColorationHighSpeed, ColorationDuration);
        PlayerInputManager.Instance.playerInput.GamePlay.DownColoration.started += ctx => GamepadVibrate(ColorationLowSpeed, ColorationHighSpeed, ColorationDuration);

        PlayerInputManager.Instance.playerInput.GamePlay.LeftColoration.started += ctx => ExcuteCameraShake("ShakeLeft");
        PlayerInputManager.Instance.playerInput.GamePlay.RightColoration.started += ctx => ExcuteCameraShake("ShakeRight");
        PlayerInputManager.Instance.playerInput.GamePlay.UpColoration.started += ctx => ExcuteCameraShake("ShakeUp");
        PlayerInputManager.Instance.playerInput.GamePlay.DownColoration.started += ctx => ExcuteCameraShake("ShakeDown");
    }

    void Start()
    {
        InitSelf();
    }
    public void GamepadVibrate(float low, float high, float time) => StartCoroutine(IEGamepadVibrate(low, high, time));

    public IEnumerator IEGamepadVibrate(float low, float high, float time)
    {
        if (PlayerInputManager.Instance.currentUISelectGameObj != null)
            yield break;
        if (Gamepad.current == null)
            yield break;

        //设置手柄的 震动速度 以及 恢复震动 , 计时到达之后暂停震动
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
        return;
        CameraShakeManager.Instance.ExcuteCameraShake(resName, GetComponentInChildren<CinemachineImpulseSource>());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        // 取消订阅输入事件
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

    bool sleep = true;
    private void Update()
    {
        if (sleep && PlayerInputManager.Instance.AnyAct)
        {
            Debug.Log("启动");
            sleep = false;
        }
    }
}
