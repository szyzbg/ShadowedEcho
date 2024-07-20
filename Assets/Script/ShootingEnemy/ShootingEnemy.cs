using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    private AudioClip DeathSound;
    //ShootingEnemy应该在玩家的(-8,8)附近的范围内移动
    public float leftBound=-6f;
    public float rightBound=6f;
    public GameObject player=null;
    public float moveSpeed=0f;//最大速度
    public float currentSpeed=0f;//当前速度
    public Vector3 destination;//目的地
    
    //发射子弹的间隔
    public float shootInterval=0.5f;
    private float shootTimer=0f;

    public float shootTimeLowerBound=1;
    public float shootTimeHigherBound=2;
    public int health=3;


    void Start()
    {
        DeathSound = Resources.Load<AudioClip>("Sounds/死神发射剑气");
        //Debug.Log("ShootingEnemy Start");
        player=GameObject.Find("Player");
        moveSpeed=player.GetComponent<HeroBehavior>().moveSpeed;
        destination=player.transform.position+new Vector3(leftBound,rightBound,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.5f) {
        //移动部分
        //Debug.Log(Vector3.Distance(transform.position,destination));
        //如果距离destination小于0.1f，设置速度为0
        if(Vector3.Distance(transform.position,destination)<0.2f)
        {
            currentSpeed=0;
        }else{
            currentSpeed=moveSpeed;
        }
        //每一帧都更新目的地并向目的地移动
        destination=player.transform.position+new Vector3(leftBound,rightBound,0);
        
        //向目的地移动
        Vector3 direction=destination-transform.position;
        //更改transform.position
        transform.position+=direction.normalized*currentSpeed*Time.smoothDeltaTime;

        //射击部分
        shootTimer+=Time.deltaTime;

        if(shootTimer>shootInterval)
        {
            GetComponent<AudioSource>().PlayOneShot(DeathSound);
            shootTimer=0;
            //发射子弹
            GameObject bullet=Instantiate(Resources.Load("Prefabs/bulletPrefabNew"),transform.position,Quaternion.identity) as GameObject;
            bullet.GetComponent<bullet>().direction=player.transform.position-transform.position;
            //重新生成shootTime;
            shootInterval=Random.Range(shootTimeLowerBound,shootTimeHigherBound);
        }
        }
        
    }

    //碰到反弹的子弹会死
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ShootingEnemy OnTriggerEnter2D");
        if(other.tag=="bullet"&&other.GetComponent<bullet>().reflected==true)
        {
            health--;
            if(health==0){
                Destroy(gameObject);
            }
           
        }
    }
}
