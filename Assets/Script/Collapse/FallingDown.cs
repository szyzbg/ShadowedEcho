using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDown : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private bool mFall = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mFall)
        {
            updateFall();
            
        }
    }

    public void Fall()
    {
        mFall = true;
    }

    void updateFall()
    {
        rigidbody.gravityScale = 1;
    }
}
