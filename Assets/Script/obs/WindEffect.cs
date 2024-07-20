using UnityEngine;

public class WindEffect : MonoBehaviour
{
    public float windStrength = 10f; // 风力强度

    private void OnTriggerStay(Collider other)
    {
        // 检查碰撞对象是否有刚体
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 向上的力
            Vector3 upwardForce = Vector3.up * windStrength;
            rb.AddForce(upwardForce);
        }
    }
}
