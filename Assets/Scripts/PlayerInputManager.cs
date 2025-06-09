using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PlayerInputManager : MonoSingleton<PlayerInputManager>
{
    public PlayerInput playerInput;

    public bool PlayerAnyAct { get; private set; }
    public bool MoveUp { get; private set; }
    public bool MoveDown { get; private set; }
    public bool MoveLeft { get; private set; }
    public bool MoveRight { get; private set; }
    public bool ColorationUp { get; private set; }
    public bool ColorationDown { get; private set; }
    public bool ColorationLeft { get; private set; }
    public bool ColorationRight { get; private set; }


    public bool MouseClick { get; private set; }

    bool settingUIPanelIsOpen;

    public GameObject currentUISelectGameObj;

    public Stack<GameObject> selectUIObjs = new Stack<GameObject>();
    public BasePanel currentPanel;

    public bool UISelect;
    public bool UIClose { get; private set; }

    public void ClearObjStack()
    {
        selectUIObjs.Clear();
        SetCurrentSelectGameObj(null);
    }

    public void SetCurrentSelectGameObj(GameObject obj)
    {

        if (selectUIObjs.Contains(obj))
        {
            return;
        }
        EventSystem.current.SetSelectedGameObject(obj);

        currentUISelectGameObj = null;
        selectUIObjs.Push(obj);//新的面板上的按钮
    }

    public void LostCurrentSelectGameObj()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject);
        if (selectUIObjs.Count == 0)
            return;
        selectUIObjs.Pop();
        Debug.Log("弹出后剩余" + selectUIObjs.Count);

        if (selectUIObjs.Count > 0 && selectUIObjs.Peek())
        {
            EventSystem.current.SetSelectedGameObject(selectUIObjs.Peek());
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        Debug.Log(EventSystem.current.currentSelectedGameObject);
    }


    public Vector2 SliderValue;

    public void GamepadUI(GameObject firstButton)
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
        Debug.Log(EventSystem.current.currentSelectedGameObject);
    }

    protected override void InitSelf()
    {
        base.InitSelf();
        if (playerInput == null)
        {
            playerInput = new PlayerInput();
            playerInput.Enable();
        }
    }
    void Update()
    {
        //移动操作输入检测
        MoveUp = playerInput.GamePlay.UpMove.WasPressedThisFrame();
        MoveDown = playerInput.GamePlay.DownMove.WasPressedThisFrame();
        MoveLeft = playerInput.GamePlay.LeftMove.WasPressedThisFrame();
        MoveRight = playerInput.GamePlay.RightMove.WasPressedThisFrame();

        //染色操作输入检测
        ColorationUp = playerInput.GamePlay.UpColoration.WasPressedThisFrame();
        ColorationDown = playerInput.GamePlay.DownColoration.WasPressedThisFrame();
        ColorationLeft = playerInput.GamePlay.LeftColoration.WasPressedThisFrame();
        ColorationRight = playerInput.GamePlay.RightColoration.WasPressedThisFrame();

        //玩家输入检测
        MouseClick = playerInput.GamePlay.MouseClick.WasPressedThisFrame();

        //玩家输入检测
        PlayerAnyAct = playerInput.GamePlay.PlayerAnyAct.WasPressedThisFrame();


        SettingPanelOpenCheck();

    }

    bool isSettingOpen=false;
    /// <summary>
    /// 检测设置面板的开闭
    /// </summary>
    void SettingPanelOpenCheck()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.ShowPanel<SettingPanel>(null);
        }
    }

    public void SettingMenuChange()
    {
        settingUIPanelIsOpen = !settingUIPanelIsOpen;
    }


}
