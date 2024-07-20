using UnityEngine;
using System.Collections;

public class FadeObjectsToBlack : MonoBehaviour
{
    public GameObject player; // 玩家对象
    public float fadeDuration = 5f; // 渐变持续时间
    public float triggerXPosition = 125f; // 触发背景渐变的玩家x坐标

    private bool hasStartedFading = false;
    private Renderer objectRenderer;
    private Color initialColor;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer.material.HasProperty("_Color"))
        {
            initialColor = objectRenderer.material.color;
        }
    }

    void Update()
    {
        if (!hasStartedFading && player.transform.position.x > triggerXPosition)
        {
            hasStartedFading = true;
            StartCoroutine(FadeToBlackCoroutine());
        }
    }

    IEnumerator FadeToBlackCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            if (objectRenderer.material.HasProperty("_Color"))
            {
                Color currentColor = Color.Lerp(initialColor, Color.black, elapsedTime / fadeDuration);
                objectRenderer.material.color = currentColor;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保颜色设置为黑色
        if (objectRenderer.material.HasProperty("_Color"))
        {
            objectRenderer.material.color = Color.black;
        }
    }
}
