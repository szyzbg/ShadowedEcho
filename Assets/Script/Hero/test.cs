using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    AudioSource s;
    public playerAnimation pa = null;
    // Start is called before the first frame update
    void Start()
    {
        s = GetComponent<AudioSource>();
        s.PlayOneShot(Resources.Load<AudioClip>("Sounds/劈石头"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
