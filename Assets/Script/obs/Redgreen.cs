using System;
using System.Collections;
using UnityEngine;

public class Redgreen : MonoBehaviour
{
    public Texture redTexture;    // 红色 Texture
    public Texture greenTexture; // 绿色 Texture
    public float switchInterval = 1.0f; // 切换 Sprite 的时间间隔（秒）

    private SpriteRenderer spriteRenderer;
    private Sprite redSprite;
    private Sprite greenSprite;
    private bool isRed = true; // 用于跟踪当前的 Sprite 状态

    // 事件，用于通知其他脚本
    public event Action<string> OnSpriteSwitch;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing.");
            return;
        }

        if (redTexture == null || greenTexture == null)
        {
            Debug.LogError("RedTexture or GreenTexture is not assigned.");
            return;
        }

        // 从 Texture 创建 Sprite
        redSprite = TextureToSprite(redTexture);
        greenSprite = TextureToSprite(greenTexture);

        if (redSprite == null || greenSprite == null)
        {
            Debug.LogError("Failed to create sprites from textures.");
            return;
        }

        // 设置初始 Sprite
        spriteRenderer.sprite = redSprite;

        // 开始切换 Sprite 的协程
        StartCoroutine(SwitchSprite());
    }

    IEnumerator SwitchSprite()
    {
        while (true)
        {
            // 等待指定的时间间隔
            yield return new WaitForSeconds(switchInterval);

            // 切换 Sprite
            if (isRed)
            {
                spriteRenderer.sprite = greenSprite;
                OnSpriteSwitch?.Invoke("green");
            }
            else
            {
                spriteRenderer.sprite = redSprite;
                OnSpriteSwitch?.Invoke("red");
            }

            // 更新当前的 Sprite 状态
            isRed = !isRed;
        }
    }

    // 将 Texture 转换为 Sprite
    private Sprite TextureToSprite(Texture texture)
    {
        if (texture == null)
        {
            Debug.LogError("Texture is null.");
            return null;
        }

        // 将 Texture 转换为 Sprite
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        return Sprite.Create((Texture2D)texture, rect, pivot);
    }
}
