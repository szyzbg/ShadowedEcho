using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed=5f;
    public Vector3 direction;
    public bool reflected=false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.5f) {
            transform.position+=direction*speed*Time.smoothDeltaTime;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //angle加90度
        angle += 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        //Debug.Log(direction);
        }
        
    }
}
