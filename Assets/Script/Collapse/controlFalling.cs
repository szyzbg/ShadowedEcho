using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlFalling : MonoBehaviour
{
    public GameObject ground;
    public float speed = 0.0f; // 移动速度
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 确保GameObject有一个Rigidbody2D组件，并向右施加力
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.M)) {
            FallingDown c = ground.GetComponent<FallingDown>();
            c.Fall();
        }
    }
}
