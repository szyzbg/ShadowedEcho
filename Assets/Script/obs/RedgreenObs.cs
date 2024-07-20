using UnityEngine;
using UnityEngine.Tilemaps;

public class RedgreenObs : MonoBehaviour
{
    public Redgreen redgreen;  // Redgreen脚本的引用
    private TilemapCollider2D tilemapCollider;
    private Tilemap tilemap;

    void Start()
    {
        if (redgreen == null)
        {
            Debug.LogError("Redgreen script reference is missing.");
            return;
        }

        // 获取TilemapCollider2D和Tilemap组件
        tilemapCollider = GetComponent<TilemapCollider2D>();
        tilemap = GetComponent<Tilemap>();

        if (tilemapCollider == null)
        {
            Debug.LogError("TilemapCollider2D component is missing.");
            return;
        }

        if (tilemap == null)
        {
            Debug.LogError("Tilemap component is missing.");
            return;
        }

        // 订阅Redgreen的OnSpriteSwitch事件
        redgreen.OnSpriteSwitch += HandleSpriteSwitch;
    }

    void HandleSpriteSwitch(string spriteName)
    {
        if (spriteName == "red")
        {
            // 设置TilemapCollider2D为禁用，透明度改为0
            tilemapCollider.enabled = false;
            SetTilemapAlpha(0);
        }
        else if (spriteName == "green")
        {
            // 设置TilemapCollider2D为启用，透明度改为255
            tilemapCollider.enabled = true;
            SetTilemapAlpha(1); // 透明度范围从0到1
        }
    }

    void SetTilemapAlpha(float alpha)
    {
        Color color = tilemap.color;
        color.a = alpha;
        tilemap.color = color;
    }

    void OnDestroy()
    {
        // 取消订阅事件，防止内存泄漏
        if (redgreen != null)
        {
            redgreen.OnSpriteSwitch -= HandleSpriteSwitch;
        }
    }
}
