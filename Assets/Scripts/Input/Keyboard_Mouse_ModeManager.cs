using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard_Mouse_ModeManager : MonoSingleton<Keyboard_Mouse_ModeManager>
{
    Vector2 hotspot = new Vector2(0, 0); // �����ȵ�λ�ã�����ڹ��ͼ��
    CursorSettingSO cursorData;
    void Start()
    {
        cursorData = Resources.Load<CursorSettingSO>("SOData/CursorSettingSO/cursor-s");
        // �����Զ�����
        Cursor.SetCursor(cursorData.customCursor, hotspot, CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �����Ҫ��ĳЩ����»ָ�Ĭ�Ϲ��
            //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            // ������굽��Ļ���Ĳ��������ָ��
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


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
        Cursor.SetCursor(cursorData. clickCursor, hotspot, CursorMode.Auto);
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}



