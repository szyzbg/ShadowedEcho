using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTools
{
    static public float initialCameraSize = 3.2f;
    static public float scaleTime = 1f;
    static private int scaleTmp = 0;
    static private Vector3 toPosition;
    // 抖动的持续时间
    static public float shakeDuration = 1f;
    // 抖动的强度
    static public float shakeMagnitude = 0.05f;
    static private float defaultShakeMagnitude = 0.05f; // 默认的抖动强度
    static public float elapsedTime;
    static public Vector3 cPosition;
    static private Vector3 originalPosition;
    static private bool isMoving = false;
    static private float moveTime = 0f;
    static private float moveDuration = 2f; // 移动持续时间

    static public void CameraFocus(GameObject camera, GameObject hero, float Xoffset, float Yoffset)
    {
        if (isMoving)
        {
            moveTime += Time.smoothDeltaTime;
            cPosition = Vector3.Lerp(originalPosition, toPosition, moveTime / moveDuration);
            if (moveTime >= moveDuration)
            {
                isMoving = false;
                moveTime = 0f;
                cPosition = toPosition;
            }
        }
        else
        {
            cPosition.x += Time.smoothDeltaTime * (hero.transform.localPosition.x - Xoffset - cPosition.x) * 5;
            cPosition.y += Time.smoothDeltaTime * (hero.transform.localPosition.y - Yoffset - cPosition.y) * 5;
        }

        if (elapsedTime < shakeDuration && Time.timeScale > 0.5f)
        {
            // 生成一个随机的抖动偏移
            Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * shakeMagnitude;
            // 应用偏移到GameObject
            camera.transform.position = cPosition + randomOffset;
            // 更新已用时间
            elapsedTime += Time.smoothDeltaTime;
        }
        else
        {
            // 如果抖动时间结束，恢复到原始位置
            camera.transform.position = cPosition;
        }
    }

    static public void CameraScale(GameObject camera, float scaleTo)
    {
        Camera c = camera.GetComponent<Camera>();
        float cInitialSize = c.orthographicSize;
        c.orthographicSize = Mathf.Lerp(cInitialSize, scaleTo, Time.smoothDeltaTime * scaleTmp / scaleTime);
        scaleTmp++;
    }

    static public void initScaleTmp()
    {
        scaleTmp = 0;
    }

    static public void StartShake(float customShakeMagnitude = -1)
    {
        if (customShakeMagnitude >= 0)
        {
            shakeMagnitude = customShakeMagnitude;
        }
        else
        {
            shakeMagnitude = defaultShakeMagnitude;
        }

        elapsedTime = 0f;
    }

    static public void ResetShakeMagnitude()
    {
        shakeMagnitude = defaultShakeMagnitude;
    }

    static public void MoveCameraTo(Vector3 newPosition, float duration)
    {
        originalPosition = cPosition;
        toPosition = newPosition;
        moveDuration = duration;
        isMoving = true;
        moveTime = 0f;
    }

    static public void StopCameraMovement()
    {
        isMoving = false;
    }
}
