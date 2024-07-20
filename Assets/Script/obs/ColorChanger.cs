using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Redgreen spriteMonitor; // 引用Redgreen脚本的GameObject

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is missing.");
            return;
        }

        if (spriteMonitor == null)
        {
            Debug.LogError("SpriteMonitor (Redgreen) is not assigned.");
            return;
        }

        // 订阅Redgreen脚本的事件
        spriteMonitor.OnSpriteSwitch += ChangeColorBasedOnSprite;
    }

    void OnDestroy()
    {
        // 取消订阅事件以防止内存泄漏
        if (spriteMonitor != null)
        {
            spriteMonitor.OnSpriteSwitch -= ChangeColorBasedOnSprite;
        }
    }

    private void ChangeColorBasedOnSprite(string spriteName)
    {
        if (spriteName == "red")
        {
            spriteRenderer.color = new Color(150f / 255f, 150f / 255f, 150f / 255f);
        }
        else if (spriteName == "green")
        {
            spriteRenderer.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        }
    }
}
