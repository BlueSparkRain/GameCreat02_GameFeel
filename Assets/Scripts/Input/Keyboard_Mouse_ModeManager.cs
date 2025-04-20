using System.Collections;
using UnityEngine;

public class Keyboard_Mouse_ModeManager : MonoSingleton<Keyboard_Mouse_ModeManager>
{
    Vector2 hotspot = new Vector2(0, 0); // 光标的热点位置（相对于光标图像）
    CursorSettingSO cursorData;

    void Start()
    {
        // 设置自定义光标
        cursorData = Resources.Load<CursorSettingSO>("SOData/CursorSettingSO/cursor-s");
        Cursor.SetCursor(cursorData.customCursor, hotspot, CursorMode.Auto);
    }

    void Update()
    {
        if (PlayerInputManager.Instance.MouseClick)
        {
            StartCoroutine(WaitToNormalCursor());
        }
    }

    /// <summary>
    /// 鼠标点击光标切换
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitToNormalCursor()
    {
        Cursor.SetCursor(cursorData.clickCursor, hotspot, CursorMode.Auto);
        //播放鼠标点击音效
        yield return new WaitForSeconds(0.2f);
        Cursor.SetCursor(cursorData.customCursor, hotspot, CursorMode.Auto);
    }


    /// <summary>
    /// 转换输入模式，退出键鼠模式
    /// </summary>
    public void ExitMouseInputMode()
    {
        // 锁定鼠标到屏幕中心并隐藏鼠标指针
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// 转换输入模式，进入键鼠模式
    /// </summary>
    public void FreezeCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}



