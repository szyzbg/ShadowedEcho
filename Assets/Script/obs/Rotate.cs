using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 10f; // 旋转速度，单位为度每秒
    public Transform targetObject; // 目标GameObject
    public float activationDistance = 5f; // 触发旋转的距离

    void Update()
    {
        // 检查目标对象是否存在
        if (targetObject != null)
        {
            // 计算两者之间的距离
            float distance = Vector3.Distance(transform.position, targetObject.position);

            // 如果距离小于设定值，则进行旋转
            if (distance < activationDistance)
            {
                // 每帧旋转
                transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
