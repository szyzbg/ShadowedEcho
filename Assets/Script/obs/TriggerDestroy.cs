using UnityEngine;

public class TriggerDestroy : MonoBehaviour
{
    // 当另一个碰撞器进入触发器区域时调用
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞到的 GameObject 是否带有 "disappear" 标签
        if (collision.gameObject.CompareTag("disappear"))
        {
            Debug.Log("Disappearing object collided. Destroying the object.");
            // 销毁碰撞到的 GameObject
            Destroy(collision.gameObject);
        }
        else
        {
            Debug.Log("Collided with: " + collision.gameObject.name);
        }
    }
}
