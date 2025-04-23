using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleRigibody : MonoBehaviour
{
    // 速度和加速度
    public Vector3 velocity;
    public Vector3 acceleration;
    public bool useGravity = true; 
    private Quaternion rotation;
  
    void Update()
    {
        if (useGravity)
        {
            HandleManualMovement();
        }
    }
    public void GetSlot() 
    {
        velocity = Vector3.zero;
        useGravity = false;
    }

    private void HandleManualMovement()
    {
        // 设置物体的位置、旋转等
        // 例如：根据用户输入或其他方式来设置速度、旋转等
        velocity += acceleration * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;
        //transform.rotation = rotation;
    }

    /// <summary>
    /// 自定义运动加速度
    /// </summary>
    /// <param name="accelerationDir">归一化方向</param>
    /// <param name="accelerationIntensity">加速度绝对值大小</param>
    public void SetCustomAcceleration(Vector3 accelerationDir)
    {
        this.acceleration = accelerationDir * (9.8f);
    }

    /// <summary>
    /// 重置刚体速度
    /// </summary>
    public void SetZeroSpeed()
    {
        velocity = Vector3.zero;
    }


    /// <summary>
    /// 设置刚体速度
    /// </summary>
    /// <param name="newVelocity"></param>
    public void SetLooseVelocity(Vector3 newVelocity)
    {
        useGravity = true;
        velocity = newVelocity;
    }

    /// <summary>
    /// 设置旋转
    /// </summary>
    /// <param name="newRotation"></param>
    public void SetRotation(Quaternion newRotation)
    {
        rotation = newRotation;
    }
}
