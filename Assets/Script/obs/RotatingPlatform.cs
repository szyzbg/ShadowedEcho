using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public Transform pivotPoint;
    public float rotationSpeed = 10f; // 旋转速度
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        // 检查碰撞对象是否有刚体
        Rigidbody otherRb = collision.collider.GetComponent<Rigidbody>();
        if (otherRb != null)
        {
            // 计算施加在平台上的力
            Vector3 force = collision.relativeVelocity * otherRb.mass;
            Vector3 torque = Vector3.Cross(force, pivotPoint.position - collision.contacts[0].point);

            // 应用力矩以旋转平台
            rb.AddTorque(torque * rotationSpeed);
        }
    }
}
