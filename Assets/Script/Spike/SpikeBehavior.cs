using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpikeBehavior : MonoBehaviour
{
    private AudioClip spikeSound;
    public GameObject hero = null;
    private Rigidbody2D rigidbody = null;

    // Start is called before the first frame update
    void Start()
    {
        spikeSound = Resources.Load<AudioClip>("Sounds/踩到地刺");
        Debug.Assert(hero != null);
        rigidbody = hero.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        checkTouched();
    }

    void checkTouched()
    {
        bool bool1 = HeroBehavior.GetIsHurt();
        bool1 = bool1 && HeroBehavior.GetInHurt();
        if (!bool1 && rigidbody.IsTouching(GetComponent<TilemapCollider2D>()))
        {
            Debug.Log("Touch");
            GetComponent<AudioSource>().PlayOneShot(spikeSound);
            HeroBehavior.SetHurt();
            HealthSystem1.Instance.TakeDamage(2f);
            
        }
    }


}
