using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBehavior1 : MonoBehaviour
{
    // Start is called before the first frame update

    //以某种方式获取敌人的位置
    public GameObject enemy;
    //以某种方式获取世界的边界的x坐标
    public float worldBoundX=-100;
    //玩家的当前速度
    public float currentSpeed=0;//这是一个矢量，正值表示向右移动，负值表示向左移动
    //玩家是否在地面上
    public bool isGrounded=true;
    //玩家跳跃力度
    public float jumpForce=100;
    //Rigidbody2D组件
    public Rigidbody2D rb;
    //玩家move的加速度
    public float moveAcceleration=1;
    //玩家的移动速度
    public float moveSpeed=10;
    //玩家攻击时最小的划动速度
    public float minSwipeSpeed=20;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //检查是否按下了D键
        if(Input.GetKey(KeyCode.D)){
            moveForward();
        }
        //检查是否按下了A键
        else if(Input.GetKey(KeyCode.A)){
            moveBackward();
        }
        else{
            stop();
        }

        //检查是否按下了空格键
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            jump();
        }



        //检查是否到了世界边界
        if(transform.position.x<worldBoundX){
            die();
        }

        //按下G键召唤敌人
        if(Input.GetKeyDown(KeyCode.G)){
            //Debug.Log("按下了G键");
            //使用resources.load方法加载预制体
            enemy=Resources.Load("Prefabs/enemyPrefab") as GameObject;
            //实例化敌人
            Instantiate(enemy);
            
        }
    }

    void moveForward(){
        //检查是否到了moveSpeed
        if(currentSpeed<moveSpeed){
            currentSpeed+=moveAcceleration*Time.deltaTime;
        }
        //向前移动
        transform.position+=new Vector3(currentSpeed*Time.deltaTime,0,0);
    }

    void moveBackward(){
        //检查是否到了moveSpeed
        if(currentSpeed>-moveSpeed){
            currentSpeed-=moveAcceleration*Time.deltaTime;
        }
        //向后移动
        transform.position+=new Vector3(currentSpeed*Time.deltaTime,0,0);
    }

    void stop(){
        //慢慢停下来
        if(currentSpeed>0){
            currentSpeed-=moveAcceleration*Time.deltaTime;
        }else if(currentSpeed<0){
            currentSpeed+=moveAcceleration*Time.deltaTime;
        }
        if(Mathf.Abs(currentSpeed)<0.3){
            currentSpeed = 0;
        }
        //Debug.Log(currentSpeed);
        transform.position+=new Vector3(currentSpeed*Time.deltaTime,0,0);
    }

    public void jump(){//按下空格键，玩家跳跃
        rb.AddForce(new Vector2(0, jumpForce));
        isGrounded = false;
    }

    public void die(){
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        
        //撞墙停下来
        if (collision.collider.CompareTag("Wall"))
        {
            currentSpeed = 0;
        }
    }
}
