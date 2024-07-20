using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    //以某种方式获取敌人的位置
    public GameObject enemy;
    //以某种方式获取世界的边界的x坐标
    public float worldBoundX = -100;
    //玩家的当前速度
    public float currentSpeed = 0;//这是一个矢量，正值表示向右移动，负值表示向左移动
    //玩家是否在地面上
    public bool isGrounded = false;
    //玩家跳跃力度
    public float jumpForce = 500;
    //Rigidbody2D组件
    public Rigidbody2D rb;
    //玩家move的加速度
    public float moveAcceleration = 10;
    //玩家的移动速度
    public float moveSpeed = 20;
    //玩家攻击时最小的划动速度
    public float minSwipeSpeed = 20;
    //玩家是否可以冰冻
    public bool canFreeze = true;
    //冰冻技能的冷却时间
    public float coolDownTime = 5;
    //冰冻技能的冷却计时器
    public float coolDownTimer = 0;
    //FallingWall的GameObject
    public GameObject fallingWall;


    //玩家需要攻击飞出来的子弹，冷却时间为1秒
    public float attackCoolDown = 1f;
    //玩家的攻击冷却计时器
    public float attackCoolDownTimer = 0f;
    //玩家是否可以攻击
    public bool canAttack = true;
    //石块的检测半径
    public float detectionRadius = 3f;

    //玩家是否正在攻击
    public bool isAttacking = false;
    //攻击时间
    public float attackTime = 0.5f;
    //攻击计时器
    public float attackTimer = 0f;

    //鼠标位置
    public Vector3 mousePosition;
    //绳索
    public LineRenderer lineRenderer;
    //绳索起始位置
    public Vector3 rayOrigin;
    //layerMask
    public LayerMask layerMask; // 在Inspector中设置要检测的Layers
    //DistanceJoint2D组件
    public DistanceJoint2D distanceJoint;
    //是否正在挂着
    public bool isConnecting = false;
    public float drawSpeed = 5f;  // 控制线条绘制的速度
    private float currentLength = 0f;  // 当前绘制的线条长度
    public Vector2 anchor = new Vector2(0, 0);  // 连接点的偏移量


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fallingWall = GameObject.Find("Falling Wall");
        worldBoundX = fallingWall.transform.position.x;


        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f; // 射线的起始宽度
        lineRenderer.endWidth = 0.05f; // 射线的结束宽度
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 设置材质
        lineRenderer.startColor = Color.white; // 射线的起始颜色
        lineRenderer.endColor = Color.white; // 射线的结束颜色

        distanceJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(isGrounded);
        //Debug.Log("isGrounded: ");
        //获取FallingWall的x坐标
        worldBoundX = fallingWall.transform.position.x;
        //检查是否按下了D键
        if (Input.GetKey(KeyCode.D))
        {
            moveForward();
        }
        //检查是否按下了A键
        else if (Input.GetKey(KeyCode.A))
        {
            moveBackward();
        }
        else
        {
            stop();
        }

        //检查是否按下了空格键
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jump();
        }

        //检查是否可以冰冻
        if (canFreeze)
        {
            coolDownTimer -= Time.deltaTime;
            if (coolDownTimer <= 0)
            {
                canFreeze = true;
            }
            //检查是否按下了F键
            if (Input.GetKeyDown(KeyCode.F))
            {
                freeze();
                canFreeze = false;
                coolDownTimer = coolDownTime;
            }
        }

        //检查是否到了世界边界
        if (transform.position.x < worldBoundX)
        {
            die();
        }

        //首先判断是否可以攻击
        if (canAttack)
        {
            if (Input.GetMouseButtonDown(0))//消耗一次攻击机会
            {
                isAttacking = true;
                //点击鼠标左键攻击
                //判断位置
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z += 10;

                canAttack = false;
                attackCoolDownTimer = attackCoolDown;
            }
        }
        else
        {
            attackCoolDownTimer -= Time.deltaTime;
            if (attackCoolDownTimer <= 0)
            {
                canAttack = true;
            }

            if (isAttacking)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer > attackTime)
                {
                    isAttacking = false;
                    attackTimer = 0;
                }
            }
        }

        if (isAttacking)
        {
            //判断位置是否在玩家的附近
            if (Vector3.Distance(mousePosition, transform.position) < 3)
            {
                //判断子弹是否在玩家的附近
                // 检测范围内的所有碰撞体
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("bullet"))
                    {
                        //反弹子弹

                        //生成一个-30到30度之间的随机角度
                        float angle = Random.Range(-30, 30);
                        //将角度转换为弧度
                        angle = angle * Mathf.Deg2Rad;

                        hitCollider.GetComponent<bullet>().direction = hitCollider.GetComponent<bullet>().direction * (-1) + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                        hitCollider.GetComponent<bullet>().reflected = true;
                    }
                }
            }
        }

        //点击R发射绳索
        if (Input.GetKeyDown(KeyCode.R) && !isConnecting)
        {
            // 射线的发射点
            rayOrigin = transform.position;
            Debug.Log(rayOrigin);

            Vector3 direction = new Vector3();
            //direction设置为鼠标与玩家之间的方向
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            direction.z = 0;
            direction.Normalize();
            Debug.Log(direction);

            // 发射射线
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, 1000f, layerMask);
            if (hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.gameObject.name);
                ConnectObjects(hit.collider.gameObject, hit.point);
                lineRenderer.enabled = true;

            }
            else
            {
                Debug.Log("No collision detected.");
            }

            // 可视化射线（仅在编辑器的Scene视图中）
            Debug.DrawRay(rayOrigin, direction * 100, Color.red);


        }
        else if (Input.GetKeyDown(KeyCode.R) && isConnecting)
        {
            isConnecting = false;
            distanceJoint.enabled = false;
            lineRenderer.enabled = false;
            currentLength = 0;
            Debug.Log("松开绳索");
        }

        if (distanceJoint != null && distanceJoint.enabled && isConnecting)
        {
            float totalLength = distanceJoint.distance;  // 获取连接的总长度
                                                         // 逐渐增加当前绘制长度直到达到总长度
            if (currentLength < totalLength)
            {
                currentLength += drawSpeed * Time.deltaTime;
                currentLength = Mathf.Min(currentLength, totalLength);  // 确保不超过总长度
            }
            float lerpFactor = currentLength / totalLength;  // 计算当前长度与总长度的比例
            Vector3 pointAlongLine = Vector3.Lerp(transform.position, distanceJoint.connectedBody.transform.TransformPoint(distanceJoint.connectedAnchor), lerpFactor);  // 计算线条上的点

            lineRenderer.SetPosition(0, transform.position +(Vector3)distanceJoint.anchor); // 发射体的位置
            lineRenderer.SetPosition(1, pointAlongLine); // 连接点的位置

        }
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
        //检查是否到了moveSpeed
        if (currentSpeed > -moveSpeed)
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
        if (Mathf.Abs(currentSpeed) < 0.3)
        {
            currentSpeed = 0;
        }
        //Debug.Log(currentSpeed);
        transform.position += new Vector3(currentSpeed * Time.deltaTime, 0, 0);
    }

    public void jump()
    {//按下空格键，玩家跳跃
        rb.AddForce(new Vector2(0, jumpForce));
        isGrounded = false;
    }

    public void die()
    {
        Destroy(gameObject);
    }

    public void freeze()
    { //按下F键，朝着面向的方向释放冰冻技能

        //enemy.GetComponent<enemyBehavior>().freeze();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision");
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

    public void ConnectObjects(GameObject target, Vector2 collisionPoint)
    {
        if (target.GetComponent<Rigidbody2D>() == null)
        {
            Debug.LogError("Target does not have a Rigidbody2D.");
            return;
        }
        Rigidbody2D targetRigidbody = target.GetComponent<Rigidbody2D>();

        // 配置DistanceJoint2D的属性
        distanceJoint.connectedBody = targetRigidbody;
        distanceJoint.autoConfigureDistance = false;  // 禁用自动配置距离

        Vector3 temp1 = transform.position;
        Vector3 temp2 = new Vector3(collisionPoint.x, collisionPoint.y, 0);

        distanceJoint.distance = Vector2.Distance(temp1, temp2);//设置连接距离
        Debug.Log(distanceJoint.distance);

        distanceJoint.maxDistanceOnly = true;  // 仅限制最大距离

        distanceJoint.anchor = anchor;  // 设置偏移量
        // 将连接点设置在碰撞点
        distanceJoint.connectedAnchor = target.transform.InverseTransformPoint(collisionPoint);

        // 启用关节
        distanceJoint.enabled = true;

        isConnecting = true;
    }

}
