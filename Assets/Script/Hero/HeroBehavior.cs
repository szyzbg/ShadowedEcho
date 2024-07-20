using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class HeroBehavior : MonoBehaviour
{
    public DialogManager DM;
    public DialogManager3 DM3;
    public playerAnimation pa;
    private AudioClip Hitted;
    public GameObject shakeSound = null;
    private AudioClip shake;
    private AudioClip shoujing;
    // Start is called before the first frame update

    //��ĳ�ַ�ʽ��ȡ���˵�λ��
    public GameObject enemy;
    //��ĳ�ַ�ʽ��ȡ����ı߽��x����
    public float worldBoundX = -100;
    //��ҵĵ�ǰ�ٶ�?
    public float currentSpeed = 0;//����һ��ʸ������ֵ��ʾ�����ƶ�����ֵ��ʾ�����ƶ�
    //����Ƿ��ڵ�����?
    public bool isGrounded = false;
    //�����Ծ����?
    public float jumpForce = 500;
    //Rigidbody2D���?
    public Rigidbody2D rb;
    //���move�ļ��ٶ�
    public float moveAcceleration = 10;
    //��ҵ��ƶ��ٶ�?
    public float moveSpeed = 20;
    //��ҹ���ʱ��С�Ļ����ٶ�?
    public float minSwipeSpeed = 20;
    //����Ƿ���Ա���
    public bool canFreeze = true;
    //�������ܵ���ȴʱ��
    public float coolDownTime = 5;
    //�������ܵ���ȴ��ʱ��
    public float coolDownTimer = 0;
    //FallingWall��GameObject
    public GameObject fallingWall;
    //�ж���������ش�����?
    private static bool isHurt = false;
    private static bool inHurt = false;
    //�����ش̻��˳��ٶ�
    private float v0 = 10f;
    private BoxCollider2D boxCollider;

    //玩家需要攻击飞出来的子弹，冷却时间�?�?
    public float attackCoolDown = 1f;
    //玩家的攻击冷却计时器
    public float attackCoolDownTimer = 0f;
    //玩家是否可以攻击
    public bool canAttack = true;
    //石块的检测半�?
    public float detectionRadius = 3f;

    //玩家是否正在攻击
    public bool isAttacking = false;
    //攻击时间
    public float attackTime = 0.5f;
    //攻击计时�?
    public float attackTimer = 0f;

    //鼠标位置
    public Vector3 mousePosition;

    //会飞的敌人出现的x坐标
    public float flyingEnemyAppears = 215;

    public GameObject flyingEnemy;

    public bool haveFlyingEnemy = false;
    public AddRigidbodyToTilemap add;
    public float detectionRange = 5.0f; // 设置检测范�?
    public float increasedShakeMagnitude = 0.1f;
    public bool falling = false;
    public HealthSystem1 healthSystem1;
    public float deathY;
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
    private float currentLength = 0f;  // 当前绘制的线条长�?
    public Vector2 anchor = new Vector2(0, 0.5f);  // 连接点的偏移�?

    public float originalLength = 0f;
    public float shrinkRate = 0.5f;
    public bool attackMode = true;
    public float lineWidth = 0.1f;
    public Material lineMaterial;
    public Image attackModeUI;
    public Image ropeModeUI;
    public bool isHand = true;

    //当前在游玩的关卡
    public int currentLevel = 1;
    //是否经过了存档点
    public static bool passedSavePoint = false;
    //暂停键盘输入
    public bool inputEnabled = true;

    void Start()
    {
        shoujing = Resources.Load<AudioClip>("Sounds/team5 受惊");
        shake = Resources.Load<AudioClip>("Sounds/EarthquackLong");
        Hitted = pa.nusMode ? Resources.Load<AudioClip>("Sounds/team1 受击") : Resources.Load<AudioClip>("Sounds/玩家被击中");
        rb = GetComponent<Rigidbody2D>();
        fallingWall = GameObject.Find("Falling Wall");
        worldBoundX = fallingWall.transform.position.x;
        boxCollider = GetComponent<BoxCollider2D>();


        lineRenderer = GetComponent<LineRenderer>();
        // lineRenderer.startWidth = 0.05f; // 射线的起始宽�?
        // lineRenderer.endWidth = 0.05f; // 射线的结束宽�?
        // lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 设置材质
        lineRenderer.startColor = Color.white; // 射线的起始颜�?
        lineRenderer.endColor = Color.white; // 射线的结束颜�?

        distanceJoint.enabled = false;

        if(passedSavePoint&&currentLevel==1)
        {
            transform.position = new Vector3(132f, 0f, 0f);
        }else if(passedSavePoint&&currentLevel==2){
            DM.CloseDialog();
            transform.position = new Vector3(58f, -2f, 0f);
        }else if(passedSavePoint&&currentLevel==3){
            DM3.CloseDialog();
            transform.position = new Vector3(127f, 1f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("level:"+currentLevel+" passedSavePoint:"+passedSavePoint);
        if (isHand == true&&inputEnabled)
        {
            if (transform.position.x == 50 && pa.nusMode)
            {
                GetComponent<AudioSource>().PlayOneShot(shoujing);
            }

            //Debug.Log(isGrounded);



            worldBoundX = fallingWall.transform.position.x;
            Hurt();
            //����Ƿ�����D��
            if (!inHurt && !isHurt)
            {
                if (!pa.inClimbing && !pa.overClimbing)
                {
                    rb.gravityScale = 2f;
                }



                if (Input.GetKey(KeyCode.A))
                {
                    moveBackward();
                }
                //����Ƿ�����A��
                else if (Input.GetKey(KeyCode.D))
                {
                    moveForward();
                }
                else
                {
                    stop();
                }

                if (Input.GetKey(KeyCode.S))
                {
                    AdjustCollider(new Vector2(boxCollider.offset.x, 0.4f), new Vector2(boxCollider.size.x, 0.7f));
                }
                else
                {
                    AdjustCollider(new Vector2(boxCollider.offset.x, 0.648f), new Vector2(boxCollider.size.x, 1.2537f));
                }

                //����Ƿ����˿ո��
                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    jump();
                }

                //����Ƿ���Ա���
                if (canFreeze)
                {
                    coolDownTimer -= Time.deltaTime;
                    if (coolDownTimer <= 0)
                    {
                        canFreeze = true;
                    }
                    //����Ƿ�����F��
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        freeze();
                        canFreeze = false;
                        coolDownTimer = coolDownTime;
                    }
                }

                //����Ƿ�������߽�
                if (transform.position.x < worldBoundX)
                {
                    die();
                }

                //����G���ٻ�����

            }

            if (attackMode)
            {
                lineRenderer.enabled = false;
                //首先判断是否可以攻击
                if (canAttack)
                {
                    if (Input.GetMouseButtonDown(0))//消耗一次攻击机�?
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
                    isAttacking = false;
                    //判断位置是否在玩家的附近
                    if (Vector3.Distance(mousePosition, transform.position) < 3)
                    {

                        if (pa.attackTime >= 0.5)
                        {
                            pa.Attack();
                        }

                        //判断子弹是否在玩家的附近
                        // 检测范围内的所有碰撞体
                        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

                        foreach (var hitCollider in hitColliders)
                        {
                            if (hitCollider.CompareTag("bullet") && !hitCollider.GetComponent<bullet>().reflected)
                            {
                                //反弹子弹
                                //生成一�?30�?0度之间的随机角度
                                float angle = Random.Range(-30, 30);
                                //将角度转换为弧度
                                angle = angle * Mathf.Deg2Rad;

                                hitCollider.GetComponent<bullet>().direction = hitCollider.GetComponent<bullet>().direction * (-1) + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                                hitCollider.GetComponent<bullet>().reflected = true;
                            }
                        }
                    }
                }


            }
            else
            {
                lineRenderer.enabled = true;

                // 射线的发射点
                rayOrigin = transform.position;
                rayOrigin.y += 0.1f;
                Debug.Log(rayOrigin);

                Vector3 direction = new Vector3();
                //direction设置为鼠标与玩家之间的方�?
                direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                direction.z = 0;
                direction.Normalize();
                //Debug.Log(direction);
                //发射射线
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, 10f, layerMask);

                if (!isConnecting)
                {
                    //可视化射�?
                    Vector3 endPosition = rayOrigin + (Vector3)direction.normalized * 10f;

                    lineRenderer.startWidth = lineWidth;
                    lineRenderer.endWidth = lineWidth;

                    //设置材质
                    lineRenderer.material = lineMaterial;

                    lineRenderer.SetPosition(0, rayOrigin + (Vector3)anchor);
                    lineRenderer.SetPosition(1, endPosition);
                }

                //点击鼠标发射绳索
                if (Input.GetMouseButtonDown(0) && !isConnecting)
                {

                    if (hit.collider != null && hit.collider.gameObject.tag == "rope")
                    {
                        pa.startHang();
                        Debug.Log("Hit: " + hit.collider.gameObject.name);

                        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 设置材质
                        lineRenderer.startColor = Color.white; // 射线的起始颜�?
                        lineRenderer.endColor = Color.white; // 射线的结束颜�?

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
                else if (Input.GetMouseButtonDown(0) && isConnecting)
                {
                    pa.endHang();
                    isConnecting = false;
                    distanceJoint.enabled = false;
                    currentLength = 0;
                    Debug.Log("松开绳索");
                }

                if (distanceJoint != null && distanceJoint.enabled && isConnecting)
                {
                    float totalLength = distanceJoint.distance;  // 获取连接的总长�?
                                                                 // 逐渐增加当前绘制长度直到达到总长�?


                    if (distanceJoint.distance >= 0.5 * originalLength)
                    {
                        distanceJoint.distance -= shrinkRate;
                    }

                    if (currentLength < totalLength)
                    {
                        currentLength += drawSpeed * Time.deltaTime;
                        currentLength = Mathf.Min(currentLength, totalLength);  // 确保不超过总长�?
                    }
                    float lerpFactor = currentLength / totalLength;  // 计算当前长度与总长度的比例
                    Vector3 pointAlongLine = Vector3.Lerp(transform.position, distanceJoint.connectedBody.transform.TransformPoint(distanceJoint.connectedAnchor), lerpFactor);  // 计算线条上的�?

                    lineRenderer.SetPosition(0, transform.position + (Vector3)distanceJoint.anchor); // 发射体的位置
                    lineRenderer.SetPosition(1, pointAlongLine); // 连接点的位置

                }
            }

            //按C键重置玩�?
            if (Input.GetKeyDown(KeyCode.C))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                currentSpeed = 0;
            }

            //按R键切换模�?
            if (Input.GetKeyDown(KeyCode.R))
            {
                attackMode = !attackMode;
                if (isConnecting)
                {
                    pa.endHang();
                    isConnecting = false;
                    distanceJoint.enabled = false;
                    currentLength = 0;
                }
            }

            if (attackMode)
            {
                Color newColor = attackModeUI.color;
                newColor.a = 1;
                attackModeUI.color = newColor;

                Color newColor1 = ropeModeUI.color;
                newColor1.a = 0;
                ropeModeUI.color = newColor1;

            }
            else
            {

                Color newColor = attackModeUI.color;
                newColor.a = 0;
                attackModeUI.color = newColor;

                Color newColor1 = ropeModeUI.color;
                newColor1.a = 1;
                ropeModeUI.color = newColor1;
            }

            if (transform.position.x >= flyingEnemyAppears && !haveFlyingEnemy && currentLevel!=3)
            {
                GameObject flyingEnemy = Instantiate(Resources.Load("Prefabs/shooting enemy"), new Vector3(transform.position.x - 6, transform.position.y + 6, 0), Quaternion.identity) as GameObject;
                GameObject flyingEnemy1 = Instantiate(Resources.Load("Prefabs/shooting enemy"), new Vector3(transform.position.x + 6, transform.position.y + 6, 0), Quaternion.identity) as GameObject;

                flyingEnemy1.GetComponent<ShootingEnemy>().leftBound = 6;
                flyingEnemy1.GetComponent<ShootingEnemy>().rightBound = 6;

                haveFlyingEnemy = true;
            }

            if (transform.position.y < deathY)
            {
                falling = true;
                healthSystem1.hitPoint = 0;
            }
        }
    }

    public void moveForward()
    {

        // if (isGrounded)
        // {
        //     currentSpeed = 5;
        //     Vector2 beforeVelocity = rb.velocity;
        //     beforeVelocity.x = currentSpeed;
        //     rb.velocity = beforeVelocity;
        // }
        // else
        // {
        //     rb.velocity = new Vector2(0,rb.velocity.y);
        //     currentSpeed = 5;
        //     transform.position += new Vector3(currentSpeed * Time.deltaTime, 0, 0);
        // }
        currentSpeed = 5;
        transform.position += new Vector3(currentSpeed * Time.smoothDeltaTime, 0, 0);

    }

    void moveBackward()
    {
        //����Ƿ���moveSpeed
        // if (currentSpeed > -moveSpeed)
        // {
        //     currentSpeed -= moveAcceleration * Time.deltaTime;
        // }
        // if (isGrounded)
        // {
        //     currentSpeed = -5;
        //     Vector2 beforeVelocity = rb.velocity;
        //     beforeVelocity.x = currentSpeed;
        //     rb.velocity = beforeVelocity;
        // }
        // else
        // {
        //     rb.velocity = new Vector2(0,rb.velocity.y);
        //     currentSpeed = -5;
        //     transform.position += new Vector3(currentSpeed * Time.deltaTime, 0, 0);
        // }
        currentSpeed = -5;
        transform.position += new Vector3(currentSpeed * Time.smoothDeltaTime, 0, 0);

    }

    void AdjustCollider(Vector2 newOffset, Vector2 newSize)
    {
        if (boxCollider != null)
        {
            boxCollider.offset = newOffset;
            boxCollider.size = newSize;
        }
    }

    void stop()
    {
        // //����ͣ����
        // if (currentSpeed > 0)
        // {
        //     currentSpeed -= moveAcceleration * Time.deltaTime;
        // }
        // else if (currentSpeed < 0)
        // {
        //     currentSpeed += moveAcceleration * Time.deltaTime;
        // }
        // if (Mathf.Abs(currentSpeed) < 0.3)
        // {

        // }

        currentSpeed = 0;
        // if (isGrounded)
        // {
        //     Vector2 beforeVelocity = rb.velocity;
        //     beforeVelocity.x = 0;
        //     rb.velocity = beforeVelocity;
        // }


        //transform.position += new Vector3(currentSpeed * Time.deltaTime, 0, 0);
    }

    public void attack()
    {

    }

    public void jump()
    {//���¿ո���������Ծ
        rb.AddForce(new Vector2(0, jumpForce));
        isGrounded = false;
    }

    public void die()
    {
        Destroy(gameObject);
    }

    public void freeze()
    { //����F������������ķ����ͷű�������?

        //enemy.GetComponent<enemyBehavior>().freeze();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision");
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("MoveGround"))
        {
            isGrounded = true;
        }

        //ײǽͣ����
        if (collision.collider.CompareTag("Wall"))
        {
            currentSpeed = 0;
        }


    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("MoveGround"))
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("MoveGround"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //如果与子弹碰撞会掉血
        if (collision.CompareTag("bullet"))
        {
            GetComponent<AudioSource>().PlayOneShot(Hitted);
            Debug.Log("collide with bullet");
            HealthSystem1.Instance.TakeDamage(2f);
        }
    }


    //�ж��Ƿ�ִ�������ش̱�����
    public static void SetHurt()
    {
        isHurt = true;
        CameraTools.StartShake();
    }

    void Hurt()
    {
        if (isHurt)
        {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 0.2f;
            GetComponent<SpriteRenderer>().color = color;
            currentSpeed = v0;
            isHurt = false;
            inHurt = true;
            Debug.Log("isHurt");
            rb.gravityScale = 0f;
        }
        if (!isHurt && inHurt && currentSpeed > v0 * 0.2f)
        {
            rb.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            transform.position += new Vector3(-currentSpeed * Time.deltaTime, currentSpeed * Time.deltaTime * 0.8f, 0);
            currentSpeed = currentSpeed * 0.8f;
            rb.gravityScale = 0f;
            Debug.Log("inHurt: " + rb.gravityScale);
        }
        if (inHurt && currentSpeed <= v0 * 0.2f)
        {
            inHurt = false;
            currentSpeed = 0f;
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 1f;
            GetComponent<SpriteRenderer>().color = color;
        }

    }

    public static bool GetIsHurt()
    {
        return isHurt;
    }
    public static bool GetInHurt()
    {
        return inHurt;
    }

    // 检查Player与tile的距�?
    public void CheckTileProximity(Vector3 tilePosition)
    {
        float distance = Vector3.Distance(transform.position, tilePosition);
        if (distance < detectionRange)
        {
            // 调整shakeMagnitude并启动Shake
            CameraTools.StartShake(increasedShakeMagnitude);
            shakeSound.GetComponent<AudioSource>().volume = 0.3f;
            shakeSound.GetComponent<AudioSource>().PlayOneShot(shake);
        }
        else
        {
            if (shakeSound.GetComponent<AudioSource>().volume > 0)
            {
                shakeSound.GetComponent<AudioSource>().volume -= 0.5f * Time.smoothDeltaTime;
            }
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

        // 配置DistanceJoint2D的属�?
        distanceJoint.connectedBody = targetRigidbody;
        distanceJoint.autoConfigureDistance = false;  // 禁用自动配置距离

        Vector3 temp1 = transform.position;
        Vector3 temp2 = new Vector3(collisionPoint.x, collisionPoint.y, 0);

        distanceJoint.distance = Vector2.Distance(temp1, temp2);//设置连接距离
        originalLength = distanceJoint.distance;
        Debug.Log(distanceJoint.distance);

        distanceJoint.maxDistanceOnly = true;  // 仅限制最大距�?

        distanceJoint.anchor = anchor;  // 设置偏移�?
        // 将连接点设置在碰撞点
        distanceJoint.connectedAnchor = target.transform.InverseTransformPoint(collisionPoint);

        // 启用关节
        distanceJoint.enabled = true;

        isConnecting = true;
    }
}
