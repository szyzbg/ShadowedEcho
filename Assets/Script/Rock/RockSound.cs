using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSound : MonoBehaviour
{
    public RockBehavior rb1 = null;
    public RockBehavior rb2 = null;
    private AudioClip rockD;
    // Start is called before the first frame update
    void Start()
    {
        rockD = Resources.Load<AudioClip>("Sounds/劈石头");
    }

    // Update is called once per frame
    void Update()
    {
        if (rb1.soundTmp) {
            GetComponent<AudioSource>().PlayOneShot(rockD);
            rb1.soundTmp = false;
        }
        if (rb2.soundTmp) {
            GetComponent<AudioSource>().PlayOneShot(rockD);
            rb2.soundTmp = false;
        }
    }
}
