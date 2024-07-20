using UnityEngine;

public class IsFall : MonoBehaviour
{
    // 变量控制 Rigidbody2D 的状态，默认为 False
    private bool hasRigidbody2D = false;

    private void Update()
    {
        // 如果当前没有 Rigidbody2D 组件，则检查条件是否满足以添加它
        if (!hasRigidbody2D)
        {
            CheckForRigidbodyAddition();
        }
    }

    private void CheckForRigidbodyAddition()
    {
        // 获取 "Falling Wall" 的 GameObject
        GameObject fallingWall = GameObject.Find("Falling Wall");
        // 如果找到了 "Falling Wall" 并且当前没有 Rigidbody2D 组件
        if (fallingWall != null)
        {
            // 获取当前 GameObject 的 BoxCollider2D 组件
            BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
            // 确保 BoxCollider2D 组件存在
            if (boxCollider2D != null)
            {
                // 获取 BoxCollider2D 的偏移的横坐标
                float boxOffsetX = boxCollider2D.offset.x;

                // 检查 "Falling Wall" 的横坐标是否大于 BoxCollider2D 的偏移横坐标
                if (fallingWall.transform.position.x > boxOffsetX)
                {
                    // 在添加 Rigidbody2D 组件之前记录日志
                    Debug.Log("Adding Rigidbody2D to the GameObject: " + gameObject.name);
                    
                    // 添加 Rigidbody2D 组件
                    gameObject.AddComponent<Rigidbody2D>();
                    // 更新状态为 True
                    hasRigidbody2D = true;
                }
            }
        }
    }
}