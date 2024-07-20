using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    public float amplitude = 0.5f; // 振幅，控制浮动的高度
    public float frequency = 1f; // 频率，控制浮动的速度

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // 记录初始位置
    }

    void Update()
    {
        // 计算新的 Y 坐标
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        // 更新 GameObject 的位置
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
