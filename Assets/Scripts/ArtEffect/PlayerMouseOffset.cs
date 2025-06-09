using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseOffset : MonoBehaviour
{
    public float offsetMultiplyer = 1;
    public float smoothTime = 0.5f;
    Vector2 startPos;
    Vector3 velocity;



    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {

        // 获取鼠标屏幕位置
        Vector3 mousePos = Input.mousePosition;

        // 将鼠标位置从屏幕空间转换为视口坐标
        Vector2 offset = Camera.main.ScreenToViewportPoint(mousePos) * 10;
        //Vector2 offset = Camera.main.ScreenToWorldPoint(mousePos) ;

        // 使用物体当前的 z 坐标，保持物体的深度
        Vector3 targetPosition = startPos + (offset * offsetMultiplyer);
        targetPosition.z = transform.position.z; // 保留当前的 z 坐标

        // 平滑移动
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void SetPosition(Vector3 position)
    {
        startPos = position;
    }
}
