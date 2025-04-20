using System.Collections;
using UnityEngine;

public class Keyboard_Mouse_ModeManager : MonoSingleton<Keyboard_Mouse_ModeManager>
{
    Vector2 hotspot = new Vector2(0, 0); // �����ȵ�λ�ã�����ڹ��ͼ��
    CursorSettingSO cursorData;

    void Start()
    {
        // �����Զ�����
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
    /// ���������л�
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitToNormalCursor()
    {
        Cursor.SetCursor(cursorData.clickCursor, hotspot, CursorMode.Auto);
        //�����������Ч
        yield return new WaitForSeconds(0.2f);
        Cursor.SetCursor(cursorData.customCursor, hotspot, CursorMode.Auto);
    }


    /// <summary>
    /// ת������ģʽ���˳�����ģʽ
    /// </summary>
    public void ExitMouseInputMode()
    {
        // ������굽��Ļ���Ĳ��������ָ��
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// ת������ģʽ���������ģʽ
    /// </summary>
    public void FreezeCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}



