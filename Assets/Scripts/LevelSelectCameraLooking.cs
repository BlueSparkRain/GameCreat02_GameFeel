using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectCameraLooking : MonoBehaviour
{
    private Vector3 lastMousePosition;
    private float rotationSpeedY;
    private float maxRotationSpeed = 2f;
    private float dampingFactor = 0.9f;
    private float thresholdAngle = 15f;
    private bool isKeyboardRotating = false;
    float keyboardRotateY = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 鼠标左键旋转视角
        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - lastMousePosition;

            rotationSpeedY += mouseDelta.x * 0.05f;

            rotationSpeedY = Mathf.Clamp(rotationSpeedY, -maxRotationSpeed, maxRotationSpeed);

            transform.Rotate(Vector3.up, -rotationSpeedY, Space.World);

            lastMousePosition = currentMousePosition;
        }
        else if(!isKeyboardRotating)
        {
            rotationSpeedY *= dampingFactor;

            transform.Rotate(Vector3.up, -rotationSpeedY, Space.World);

            // 检查当前角度是否接近90度的倍数
            float currentAngleY = transform.eulerAngles.y;
            float nearestMultipleOf90 = Mathf.Round(currentAngleY / 90f) * 90f;
            float angleDifferenceY = Mathf.Abs(currentAngleY - nearestMultipleOf90);

            // 如果接近90度的倍数，则回弹到最近的90度的倍数
            if (angleDifferenceY <= thresholdAngle)
            {
                rotationSpeedY = 0f; 
                float snapBackDeltaY = nearestMultipleOf90 - currentAngleY;
                transform.Rotate(Vector3.up, snapBackDeltaY * 0.2f, Space.World);
            }
            lastMousePosition = Input.mousePosition;
        }
        // 按键控制视角
        
        if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isKeyboardRotating = true;
            keyboardRotateY -= 90f;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            isKeyboardRotating = true;
            keyboardRotateY += 90f;
        }
        if(keyboardRotateY>0.5f)
        {
            transform.Rotate(Vector3.up, 90 * 0.01f, Space.World);
            keyboardRotateY -= 90 * 0.01f;
        }
        else if (keyboardRotateY < -0.5f)
        {
            transform.Rotate(Vector3.up, -90 * 0.01f, Space.World);
            keyboardRotateY += 90 * 0.01f;
        }
        else
        {
            keyboardRotateY = 0;
            isKeyboardRotating = false;
        }

            //
            if ((Mathf.Abs(transform.eulerAngles.y + 1) % 90) > 10f)
        {
            if (transform.GetChild(0).gameObject.activeSelf == true)
            {
                //如果关卡已解锁，则将第二个AudioSource的音频由默认的噪声改为对应音乐
            }
            transform.GetChild(0).gameObject.SetActive(false);

        }
        else
        {
            if (transform.GetChild(0).gameObject.activeSelf == false)
            {
                transform.GetComponent<AudioSource>().Play();
            }

            transform.GetChild(0).gameObject.SetActive(true);

        }

    }
}
