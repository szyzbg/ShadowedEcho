using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AddRigidbodyToTilemap : MonoBehaviour
{
    public Tilemap tilemap;  // Tilemap组件
    public float delay = 0.1f; // 每个tile的延迟时间
    public float startDelay = 3.0f; // 初始延迟时间
    public HeroBehavior heroBehavior; // 引用HeroBehavior组件

    public Vector3 currentTileWorldPosition; // 当前tile的世界坐标

    void Start()
    {
        StartCoroutine(AddRigidbodyToTiles());
    }

    private IEnumerator AddRigidbodyToTiles()
    {
        // 初始延迟
        yield return new WaitForSeconds(startDelay);

        // 获取tilemap的边界
        BoundsInt bounds = tilemap.cellBounds;

        // 遍历所有的tile
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(pos))
                {
                    // 为当前tile创建一个游戏对象
                    GameObject tileObject = new GameObject("Tile_" + x + "_" + y);
                    tileObject.transform.position = tilemap.CellToWorld(pos) + tilemap.tileAnchor;

                    // 设置当前tile的世界坐标
                    currentTileWorldPosition = tileObject.transform.position;

                    // 添加SpriteRenderer组件以显示tile的精灵
                    SpriteRenderer renderer = tileObject.AddComponent<SpriteRenderer>();
                    renderer.sprite = tilemap.GetSprite(pos);

                    // 添加Rigidbody2D组件以实现自由落体
                    Rigidbody2D rb = tileObject.AddComponent<Rigidbody2D>();
                    rb.gravityScale = 1;

                    // 从tilemap中移除当前tile
                    tilemap.SetTile(pos, null);

                    // 调用HeroBehavior的方法
                    if (heroBehavior != null)
                    {
                        heroBehavior.CheckTileProximity(currentTileWorldPosition);
                    }

                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }
}
