using UnityEngine;

public class LeftRightMovement : MonoBehaviour
{
    public float amplitude = 1f;  // 移动的振幅（上下移动的距离）
    public float frequency = 1f;  // 移动的频率（速度）

    private Vector3 startPosition;  // 初始位置

    void Start()
    {
        // 记录GameObject的初始位置
        startPosition = transform.position;
    }

    void Update()
    {
        // 计算新的Y位置
        float newX = startPosition.x + Mathf.Sin(Time.time * frequency) * amplitude;

        // 设置GameObject的新位置
        transform.position = new Vector3( newX,startPosition.y, startPosition.z);
    }
}
