using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CursorData",menuName = "MyCustomData/CursorSettingSO")]
public class CursorSettingSO :ScriptableObject
{ 
    [Header("默认鼠标光标图片")]
    public Texture2D customCursor;
    [Header("鼠标（左键）点击光标图片")]
    public Texture2D clickCursor;
    [Header("鼠标特殊选中悬停光标图片")]
    public Texture2D selectCursor;
}
