using Unity.VisualScripting;
using UnityEngine;

public class TaxiMovement : MonoBehaviour
{
    private AudioClip ac;
    private bool soundPlayed = false;
    public GameObject circle; // 圆形物体的引用
    public float speed = 5f; // 出租车的初始速度
    public float acceleration = 10f; // 出租车的加速度

    void Start() {
        ac = Resources.Load<AudioClip>("Sounds/出租车喇叭");
    }
    private void Update()
    {
        // 确保circle已经被赋值
        if (circle == null)
        {
            Debug.LogError("Circle object is not assigned.");
            return;
        }

        // 计算出租车与圆形物体之间的水平距离
        float horizontalDistance = Mathf.Abs(transform.position.x - circle.transform.position.x);

        // 如果水平距离小于10个单位
        if (horizontalDistance < 10f)
        {
            if (!soundPlayed) {
                soundPlayed = true;
                AudioSource asc = GetComponent<AudioSource>();
                asc.PlayOneShot(ac);
            }
            
                
            
            // 向左加速运动
            // 这里我们通过增加速度来实现加速效果
            speed += acceleration * Time.deltaTime;

            // 更新出租车的位置
            // 我们使用负值来表示向左移动
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
        }
        else
        {
            // 如果距离大于10个单位，恢复初始速度
            speed = 5f;
        }
    }
}