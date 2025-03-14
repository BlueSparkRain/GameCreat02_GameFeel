using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class InputManager : MonoSingleton<InputManager>
{    
    private PlayerInput playerInput;
    public bool MoveUp {  get; private set; }
    public bool MoveDown {  get; private set; }
    public bool MoveLeft {  get; private set; }
    public bool MoveRight {  get; private set; }
    public bool ColorationUp {  get; private set; }
    public bool ColorationDown {  get; private set; }
    public bool ColorationLeft {  get; private set; }
    public bool ColorationRight {  get; private set; }

    public void GamepadUI(GameObject firstButton) 
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
        Debug.Log(EventSystem.current.currentSelectedGameObject);
    }

    protected override void InitSelf()
    {
        base.InitSelf();
        if(playerInput == null) 
        {
            playerInput = new PlayerInput();
            playerInput.Enable();
        }
    }

    void Update()
    {
       MoveUp=playerInput.GamePlay.UpMove.WasPressedThisFrame();
       MoveDown = playerInput.GamePlay.DownMove.WasPressedThisFrame();
       MoveLeft=playerInput.GamePlay.LeftMove.WasPressedThisFrame();
       MoveRight=playerInput.GamePlay.RightMove.WasPressedThisFrame();
        
       ColorationUp = playerInput.GamePlay.UpColoration.WasPressedThisFrame();
       ColorationDown = playerInput.GamePlay.DownColoration.WasPressedThisFrame();
       ColorationLeft = playerInput.GamePlay.LeftColoration.WasPressedThisFrame();
       ColorationRight = playerInput.GamePlay.RightColoration.WasPressedThisFrame();
    }


    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

}
