using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    public DialogManager dl;
    public playerAnimation pa;
    private int diaCount = 0;
    private bool isCat1DiaShown = false;
    public NekoAnimation nekoAnimation;
    public float speed = 4.5f; // 小猫的恒定速度
    public float jumpForceSmall = 5f; // 小猫的跳跃力
    public float jumpForceBig = 8f; // 小猫的跳跃力
    private Rigidbody2D rb2D; // 用于操作刚体的引用
    public GameObject circle; // Circle GameObject的引用
    private bool isJumping = false; // 用于判断小猫是否在跳跃
    private AudioClip nekoMeow;
    private bool meowPlayed = false;

    void Start()
    {
        nekoMeow = Resources.Load<AudioClip>("Sounds/team10 猫叫");
        rb2D = GetComponent<Rigidbody2D>(); // 获取Rigidbody2D组件的引用

        // 设置物理材质，确保小猫不会粘在墙上
        PhysicsMaterial2D noFrictionMaterial = new PhysicsMaterial2D();
        noFrictionMaterial.friction = 0f;
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.sharedMaterial = noFrictionMaterial;
        }
    }

    void Update()
    {
        Debug.Log("catdiacount " + diaCount);
        if (Input.GetMouseButtonDown(0) && Time.timeScale == 0 && !isCat1DiaShown && circle.transform.position.x >= -15f)
        {
            diaCount++;
            if (diaCount == 2)
            {
                isCat1DiaShown = true;
                Time.timeScale = 1;
                dl.CloseDialog();
            }
        }

        // 检查circle的x坐标是否大于等于-15
        if (circle.transform.position.x >= -15f)
        {
            if (!isCat1DiaShown)
            {
                if (diaCount == 0)
                {
                    if (!meowPlayed) {
                        meowPlayed = true;
                        // if (pa.nusMode) {
                        //     GetComponent<AudioSource>().PlayOneShot(nekoMeow);
                        // }
                        
                    }
                    
                    dl.ShowDialog(" A black cat? ");
                    Time.timeScale = 0;
                }
                if (diaCount == 1)
                {
                    dl.ShowDialog("It is the only life in this odd place, I think I need to catch it for company.");
                }
            }

            // 计算小猫和circle之间的x坐标差
            float xDifference = Mathf.Abs(circle.transform.position.x - transform.position.x);

            // 检查小猫的x坐标是否大于259
            if (transform.position.x > 259f)
            {
                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                nekoAnimation.catIdle();
            }
            else if (xDifference > 5f && !isJumping)
            {
                // 如果x坐标差大于5并且小猫没有在跳跃，让小猫停下来
                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                nekoAnimation.catIdle();
            }
            else if (xDifference <= 5f)
            {
                // 恢复小猫的速度
                rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
                nekoAnimation.catRun();
            }
        }
        else
        {
            // 保持小猫的速度为0
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            nekoAnimation.catIdle();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞的物体是否具有"Cat Jump"标签
        if (collision.gameObject.CompareTag("Cat Jump"))
        {
            // 轻轻跳一下
            rb2D.AddForce(new Vector2(0, jumpForceSmall), ForceMode2D.Impulse);
            isJumping = true;
        }

        // 检查碰撞的物体是否具有"Cat JumpBig"标签
        if (collision.gameObject.CompareTag("Cat JumpBig"))
        {
            // 重重跳一下
            rb2D.AddForce(new Vector2(0, jumpForceBig), ForceMode2D.Impulse);
            isJumping = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 碰撞到地面后，认为小猫不再跳跃
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
        
        //Debug.Log("Collision detected with: " + collision.gameObject.name);
    }
}
