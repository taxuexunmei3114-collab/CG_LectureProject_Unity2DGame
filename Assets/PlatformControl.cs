using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    private PlatformEffector2D effector;
    private float activationDelay = 0.3f; // 长按激活延迟
    private float pressTimer; // 按键按下计时器
    private bool isActive; // 是否处于可穿过状态

    private void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        if (effector == null)
        {
            Debug.LogError("PlatformEffector2D component not found on " + gameObject.name);
        }

    }
    private void Update()
    {
        if (effector == null) return;

        // 检测S键长按
        if (Input.GetKey(KeyCode.S))
        {
            pressTimer += Time.deltaTime;

            // 长按达到延迟时间后激活穿过状态
            if (pressTimer >= activationDelay && !isActive)
            {
                effector.rotationalOffset = 180f;
                isActive = true;
            }
        }
        // 松开S键时取消穿过状态
        else if (Input.GetKeyUp(KeyCode.S))
        {
            ResetPlatformState();
        }
    }

    // 重置平台状态为不可穿过
    private void ResetPlatformState()
    {
        pressTimer = 0f;
        if (isActive)
        {
            effector.rotationalOffset = 0f;
            isActive = false;
        }
    }
}
