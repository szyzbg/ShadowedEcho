using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjAScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Vector3.right * 0.1f;
    }
}
