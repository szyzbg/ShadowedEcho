using UnityEngine;

public class MoveLeftRepeat : MonoBehaviour
{
    public float speed = 5f; // 控制移动速度
    private Camera mainCamera; // 引用主相机
    private Vector3 startPosition; // 起始位置
    private Vector3 endPosition; // 结束位置

    void Start()
    {
        mainCamera = Camera.main; // 获取主相机的引用
        startPosition = new Vector3(mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x, transform.position.y, transform.position.z);
        endPosition = new Vector3(mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        // 移动到左侧
        if (transform.position.x <= endPosition.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
        }
        // 当到达左侧时，移动到右侧
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
        }
    }
}