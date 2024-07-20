using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
public class enemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    //以某种方式获取玩家的位置
    private bool isEnemyDesDiaShown = false;
    private enemyController eC;
    public GameObject player;
    //以某种方式获取世界的边界的x坐标
    public float worldBoundX;
    //敌人初始情况距离玩家的距离
    public float initialDistance = 8;
    //敌人距离玩家多远的距离开始向玩家移动
    public float moveDistance = 10;
    //敌人距离世界边界多少距离开始冲刺
    public float dashDistance = 20;
    //敌人的当前速度
    public float currentSpeed = 0;//这是一个矢量，正值表示向右移动，负值表示向左移动
    //敌人的移动速度
    public float moveSpeed = 5;
    //敌人的冲刺速度
    public float dashSpeed = 15;
    //敌人的冰冻状态
    public bool isFrozen = false;
    //敌人的冰冻时间
    public float frozenTime = 5;
    //敌人的冰冻计时器
    public float frozenTimer = 0;
    //敌人dash时间的下限
    public float dashTimeLowerBound = 0;
    //敌人dash时间的上限
    public float dashTimeUpperBound = 10;
    //敌人的冲刺状态
    public bool isDashing;
    //敌人的dash时间
    public float dashTime;
    //敌人的dash计时器
    public float dashTimer = 0;
    //敌人移动的加速度
    public float moveAcceleration = 10;
    //敌人alpha变化速度
    public float alphaSpeed = 0.5f;
    //敌人存在的时间
    public float lifeTime = 9999;
    //计时器
    public float timer = 0;
    //冰冻计时器
    public float freezeTimer = 0;
    //冰冻CD
    public float freezeCD = 5;
    //是否可以冰冻
    public bool canFreeze = true;
    private static bool freezeFlag = true;
    public Collider2D enemyCollider;
    public Collider2D playerCollider;
    public enemyController enemyCont;
    public bool stage = false;//stage为false说明在底下，为true说明在上面
    public Transform Sky;
    void Start()
    {
        player = GameObject.Find("Player");
        enemyCont = GameObject.Find("invisibleManController").GetComponent<enemyController>();
        enemyCollider = GetComponent<Collider2D>();
        playerCollider = GameObject.Find("Player").GetComponent<Collider2D>();
        //禁用enemy和player的碰撞
        //Physics2D.IgnoreCollision(enemyCollider, playerCollider);
        Debug.Log("调用了enemyBehavior的Start函数");
        //设置player
        eC = GameObject.Find("invisibleManController").GetComponent<enemyController>();
        timer = 0;
        //敌人首先刷新在玩家的前面（也就是右侧）
        transform.position = new Vector3(player.transform.position.x + initialDistance, player.transform.position.y, player.transform.position.z);
        currentSpeed = 0;
        //随机生成一个敌人冲刺时间
        dashTime = UnityEngine.Random.Range(dashTimeLowerBound, dashTimeUpperBound);
        //设置透明度
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 1;
        GetComponent<SpriteRenderer>().color = color;

        worldBoundX = GameObject.Find("ControlFalling").GetComponent<AddRigidbodyToTilemap>().currentTileWorldPosition.x;

        Sky = Camera.main.transform.Find("Sky");
    }

    // Update is called once per frame
    void Update()
    {
        if (stage == false)
        {
            if (isEnemyDesDiaShown && Input.GetMouseButtonDown(0))
            {
                eC.addOneToDiaCount();
            }

            worldBoundX = GameObject.Find("ControlFalling").GetComponent<AddRigidbodyToTilemap>().currentTileWorldPosition.x;
            Debug.Log("worldBoundX" + worldBoundX);
            timer += Time.deltaTime;

            if (!canFreeze)
            {
                freezeTimer += Time.deltaTime;
                if (freezeTimer >= freezeCD)
                {
                    canFreeze = true;
                    freezeTimer = 0;
                }
            }

            transform.position = new Vector3(transform.position.x, 1f, player.transform.position.z);
            if (isFrozen)
            {//判断是否被冰冻
                canFreeze = false;
                Debug.Log(1);

                frozenTimer += Time.deltaTime;
                if (frozenTimer >= frozenTime)
                {
                    isFrozen = false;
                    frozenTimer = 0;
                }
            }
            else
            {

                //检查是否按下了F键
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (canFreeze)
                    {
                        freeze();
                        currentSpeed = 0;
                    }
                }

                //如果时间到了，敌人只会向前移动，直到碰到世界边界，随后消失
                if (timer >= lifeTime)
                {

                    if (transform.position.x > worldBoundX)
                    {
                        moveBackward();
                    }
                    else
                    {
                        stage=true;
                        if (!isEnemyDesDiaShown)
                        {
                            isEnemyDesDiaShown = true;
                            eC.changeToDiaCount();
                        }
                        //Destroy(gameObject);
                    }
                }
                else
                {
                    //如果距离世界边界的距离小于一定值，则向前冲刺
                    if (transform.position.x < worldBoundX + dashDistance)
                    {//进入冲刺状态
                        isDashing = true;
                    }
                    if (isDashing)
                    {//在冲刺状态

                        Debug.Log(3);

                        dashTimer += Time.deltaTime;
                        if (dashTimer <= dashTime)
                        {
                            dashForward();
                        }
                        else
                        {
                            isDashing = false;
                            dashTimer = 0;
                            //随机生成一个敌人冲刺时间
                            dashTime = UnityEngine.Random.Range(dashTimeLowerBound, dashTimeUpperBound);
                        }
                    }
                    else
                    {//不在冲刺状态
                     //如果玩家在敌人左侧一定范围之外，则向左移动
                        if (player.transform.position.x - transform.position.x < -moveDistance)
                        {
                            moveBackward();
                        }
                        //如果玩家在敌人右侧一定范围之外，则向右移动
                        else if (player.transform.position.x - transform.position.x > moveDistance)
                        {
                            moveForward();
                        }
                        //如果敌人的速度为负值，并且敌人的位置在玩家右侧或左侧一定范围内，继续向左移动
                        else if (currentSpeed < 0 && (Math.Abs(player.transform.position.x - transform.position.x) <= moveDistance))
                        {
                            moveBackward();
                        }
                        //如果敌人的速度为正值，并且敌人的位置在玩家右侧或左侧一定范围内，继续向右移动
                        else if (currentSpeed > 0 && (Math.Abs(player.transform.position.x - transform.position.x) <= moveDistance))
                        {
                            moveForward();
                        }
                        else
                        {//速度是0
                            moveBackward();
                        }
                    }
                }
            }
            //如果透明度不是0，则逐渐降低透明度
            if (GetComponent<SpriteRenderer>().color.a >= 0)//后续要将该数字调为0
            {
                changeAlpha();
            }

            if (timer < 5)
            {
                //alpha=1;
                Color color = GetComponent<SpriteRenderer>().color;
                color.a = 1;
                GetComponent<SpriteRenderer>().color = color;
            }
        }
        else
        {
            //首先判断玩家的位置
            if (player.transform.position.x >= 109.9f && player.transform.position.y >= 57.9)
            {
                Debug.Log("stage=true");
                //背景逐渐变暗
                //Sky下移
                if (Sky.localPosition.y >= -13f)
                {
                    Sky.localPosition = new Vector3(Sky.localPosition.x, Sky.localPosition.y - 3 * Time.smoothDeltaTime, Sky.localPosition.z);
                }
            }
            else
            {
                //应该传送到下面 具体位置待定
                //transform.position = new Vector3(119f, 58f, player.transform.position.z);

            }
        }
    }

    void changeAlpha()
    {
        //逐渐降低敌人的透明度
        Color color = GetComponent<SpriteRenderer>().color;
        color.a -= alphaSpeed * Time.deltaTime;
        GetComponent<SpriteRenderer>().color = color;
    }

    void moveForward()
    {
        //检查是否到了moveSpeed
        if (currentSpeed < moveSpeed)
        {
            currentSpeed += moveAcceleration * Time.deltaTime;
        }
        //向前移动
        transform.position += new Vector3(currentSpeed * Time.deltaTime, 0, 0);
    }

    void moveBackward()
    {
        //Debug.Log(currentSpeed);
        //检查是否到了moveSpeed
        if (currentSpeed > -moveSpeed)
        {
            currentSpeed -= moveAcceleration * Time.deltaTime;
        }
        //向后移动
        transform.position += new Vector3(currentSpeed * Time.deltaTime, 0, 0);
    }

    void dashForward()
    {
        //检查是否到了dashSpeed
        if (currentSpeed < dashSpeed)
        {
            currentSpeed += moveAcceleration * Time.deltaTime;
        }
        //向前移动
        transform.position += new Vector3(currentSpeed * Time.deltaTime, 0, 0);
    }

    void dashBackward()
    {
        //检查是否到了dashSpeed
        if (currentSpeed > -dashSpeed)
        {
            currentSpeed -= moveAcceleration * Time.deltaTime;
        }
        //向后移动
        transform.position += new Vector3(currentSpeed * Time.deltaTime, 0, 0);
    }

    void stop()
    {
        //慢慢停下来
        if (currentSpeed > 0)
        {
            currentSpeed -= moveAcceleration * Time.deltaTime;
        }
        else if (currentSpeed < 0)
        {
            currentSpeed += moveAcceleration * Time.deltaTime;
        }
        transform.position += new Vector3(currentSpeed * Time.deltaTime, 0, 0);
    }

    //如果和玩家碰撞，则玩家扣血
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemyCont.hitByEnemy();
            HealthSystem1.Instance.TakeDamage(10f);
        }
    }

    //如果被冰冻
    public void freeze()
    {
        Debug.Log("freezeFlag" + freezeFlag);

        isFrozen = true;
        Debug.Log(isFrozen);
        //将不透明度设置为100%
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 1;
        GetComponent<SpriteRenderer>().color = color;
    }

    public static void setFreezeFlag(bool flag)
    {
        freezeFlag = flag;
    }

    public bool getFreezeFlag()
    {
        return freezeFlag;
    }
}
