using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CursorData",menuName = "MyCustomData/CursorSettingSO")]
public class CursorSettingSO :ScriptableObject
{ 
    [Header("Ĭ�������ͼƬ")]
    public Texture2D customCursor;
    [Header("��꣨�����������ͼƬ")]
    public Texture2D clickCursor;
    [Header("�������ѡ����ͣ���ͼƬ")]
    public Texture2D selectCursor;
}
