using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;
using System.Security.Cryptography;

public class playerAnimation : MonoBehaviour
{
    private bool TscaleTmp = true;
    public DialogManager3 diaM3;
    private bool stuckModel;
    public bool nusMode = false;
    //public DialogManager diaM = null;
    public GameObject shakeSound = null;
    public bool inClimbing = false;
    public bool overClimbing = false;
    private bool inHang = false;
    private float ladderFinTime;
    public HeroBehavior pb = null;

    public GameObject Player = null;
    public bool JbreakDown = false;
    private bool jumpFlag = false;
    private int jumpTmp = 0;
    private int jumptmp = 0;
    public float attackTime;
    private Animator a;
    private bool jumpClipTmp = false;
    private float deltaTmp;
    //public HeroBehaviorForNUS heroBehavior;
    public HealthSystem1 healthSystem1;
    private bool isDieAnimationPlaying = false;
    public GameObject panel; // 引用Panel对象
    public GameObject helpButton;//to open help button

    public RectTransform text = null;
    // public UnityEngine.UI.Button buttonPrefab; // 明确使用UnityEngine.UI.Button

    public GameObject specialPiece;//piece to identify scene index;
    public bool isIdle = true;
    private Collider2D heroCol;
    AudioClip jumpClip, attackSound;
    private RuntimeAnimatorController crouch,jump, jumpMid, jumpFall, idle, run, roll, ladderclimbing, ladderidle, ladderfinish, slide, attack, die, wallclimbI;


    public void setNUS() {
        nusMode = true;
    }
    public void notNUS() {
        nusMode = false;
    }
    public void startClimb()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        inClimbing = true;
        overClimbing = false;
    }
    public void endClimb()
    {
        inClimbing = false;
        overClimbing = true;
    }
    public void startHang()
    {
        inHang = true;
    }
    public void endHang()
    {
        inHang = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        heroCol = GetComponent<Collider2D>();
        ladderFinTime = 100 / Time.smoothDeltaTime;
        panel.SetActive(false);
        jumpClip = Resources.Load<AudioClip>("Sounds/跳");
        attackSound = Resources.Load<AudioClip>("Sounds/玩家攻击");
        a = GetComponent<Animator>();
        attackTime = 100f;
        crouch = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/CrouchC");
        wallclimbI = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/WallClimbIdleC");
        jump = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/JumpC");
        jumpMid = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/JumpMidC");
        jumpFall = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/JumpFallC");
        idle = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/IdleC");
        run = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/RunC");
        roll = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/RollC");
        ladderclimbing = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/LadderClimbC");
        ladderidle = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/ClimbIdleC");
        ladderfinish = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/LadderClimbFinishC");
        slide = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/SlideC");
        attack = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/SwordAttackC");
        die = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/DieC");
    }
    
   
    // Update is called once per frame
    void Update()
    {
        Debug.Log("ts " + Time.timeScale);
       // Debug.Log("isG "+pb.isGrounded);
        
        Collider2D[] results = new Collider2D[10];

        // 使用 ContactFilter2D 过滤器设置无过滤条件
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        int overlapCount = heroCol.OverlapCollider(filter, results);

        // 如果有重叠的碰撞器
        if (overlapCount > 0) {
            for (int i = 0; i < overlapCount; i++) {
                if (results[i].gameObject.CompareTag("Ground")) {
                    if (Mathf.Abs(results[i].bounds.center.x - heroCol.bounds.center.x) < 0.7f&&
                    Mathf.Abs(results[i].bounds.center.y - heroCol.bounds.center.y) < 1.1f) {
                        stuckModel = true;
                        break;
                    }
                    
                }
            }
        }
        if (stuckModel) {
            Debug.Log("in");
            stuckModel = false;
            transform.position = AvoidModelBugManager.getHeroPosition();
        }
        else if (overlapCount > 0) {
            for (int i = 0; i < overlapCount; i++) {
                if (results[i].gameObject.CompareTag("MoveGround")) {
                    if (Input.GetKey(KeyCode.S)) {
                        Vector3 t = transform.position;
                    }
                    if (Mathf.Abs(results[i].bounds.center.x - heroCol.bounds.center.x) < 1.7f&&
                    Mathf.Abs(results[i].bounds.center.y - heroCol.bounds.center.y) < 1.1f) {
                        if (!Input.GetKey(KeyCode.S)) {
                            Vector3 t = transform.position;
                            t.y = results[i].bounds.center.y + 1.1f;
                            transform.position = t;
                        }
                        
                    }
                    
                }
                else {
                    AvoidModelBugManager.updateHeroPosition(transform.position);
                }
            }
        }
        else {
        //    Debug.Log("off");
            AvoidModelBugManager.updateHeroPosition(transform.position);
        }
        
        
        if (healthSystem1.alive == true && pb.falling == false)
        {
            
            if (!inHang)
            {
                if (!inClimbing && !overClimbing)
                {

                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        Time.timeScale = (Time.timeScale > 0.5f) ? 0f : 1f;
                        if (Time.timeScale == 1f)
                        {
                            panel.SetActive(false);
                            helpButton.SetActive(false);
                            text.GetComponent<TextMeshProUGUI>().text = "";
                            DiaryBehavior2.Continue();
                            DiaryBehavior.Continue();
                        }
                        else
                        {
                            panel.SetActive(true);
                            helpButton.SetActive(true);
                            text.GetComponent<TextMeshProUGUI>().text = "Click P to continue.";
                            DiaryBehavior.End();
                            DiaryBehavior2.End();
                        }
                    }
                    if (Time.timeScale > 0.5f)
                    {

                        if (attackTime >= 0.5)
                        {
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                Debug.Log("jumpFlag");
                                deltaTmp = 1 / Time.smoothDeltaTime;

                                Jump();
                                jumpFlag = true;
                            }
                            //Debug.Log(jumpFlag);
                            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !Input.GetKey(KeyCode.S) && !jumpFlag)
                            {
                                Run();

                                SpriteRenderer sr = Player.GetComponent<SpriteRenderer>();

                                if (Input.GetKey(KeyCode.A))
                                {
                                    sr.flipX = true;
                                }
                                else
                                {
                                    sr.flipX = false;
                                }

                            }
                            else if (Input.GetKey(KeyCode.S) && !jumpFlag)
                            {
                                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
                                    Slide();
                                }
                                else {
                                    Crouch();
                                }
                            }
                            else if (jumpFlag)
                            {
                                jumptmp++;
                                Jump();
                                if (pb.isGrounded && jumptmp >= 20)
                                {
                                    jumpFlag = false;
                                    jumpTmp = 0;
                                    jumptmp = 0;
                                }
                            }
                            else if (!jumpFlag && pb.isGrounded)
                            {
                                if (isIdle) {
                                    Idle();
                                }
                                
                                
                            }

                        }
                        // if (Input.GetMouseButtonDown(0) && attackTime >= 0.5/Time.smoothDeltaTime)
                        // {
                        //     Attack();
                        // }

                        //LadderClimbing(Input.GetKey(KeyCode.M));
                        RuntimeAnimatorController r = overClimbing?idle:a.runtimeAnimatorController;
                        if (attackTime == 0f)
                        {
                            GetComponent<AudioSource>().PlayOneShot(attackSound);
                            attackTime += Time.smoothDeltaTime;
                            a.runtimeAnimatorController = attack;
                        }
                        else if (attackTime >= 0.5f)
                        {
                            if (!r.name.Equals(ladderclimbing.name) && !r.name.Equals(ladderidle.name) && !r.name.Equals(ladderfinish.name))
                            {
                                a.runtimeAnimatorController = r;
                            }

                        }
                        else
                        {
                            attackTime += Time.smoothDeltaTime;
                        }
                    }



                }
                else if (inClimbing)
                {
                    Player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                    Player.GetComponent<Rigidbody2D>().gravityScale = 0;
                    ladderFinTime = 0;
                    if (Input.GetKey(KeyCode.W))
                    {
                        a.runtimeAnimatorController = ladderclimbing;
                        //GetComponent<Rigidbody2D>().velocity = new Vector2(0,2);
                        transform.position += new Vector3(0,2*Time.smoothDeltaTime,0);
                    }
                    else if (Input.GetKey(KeyCode.S)) {
                        a.runtimeAnimatorController = ladderclimbing;
                        //GetComponent<Rigidbody2D>().velocity = new Vector2(0,-2);
                        transform.position -= new Vector3(0,2*Time.smoothDeltaTime,0);
                    }
                    else
                    {
                        a.runtimeAnimatorController = ladderidle;
                        Vector2 v = GetComponent<Rigidbody2D>().velocity;
                        v.y = 0f;
                        GetComponent<Rigidbody2D>().velocity = v;
                    }
                }
                else
                {
                    
                    ladderFinTime = 100 / Time.smoothDeltaTime;
                    if (pb.isGrounded) {
                        overClimbing = false;
                    }
                    Player.GetComponent<Rigidbody2D>().gravityScale = 2;
                }
            }
            else
            {
                a.runtimeAnimatorController = wallclimbI;
            }
        }
        else
        {
            Die();
        }
    }
    
    void OnTriggerEnter2D(Collider2D ladC) {
        if (ladC.gameObject.CompareTag("ladder")) {
            pb.isGrounded = false;
            startClimb();
        }
    }
    void OnTriggerExit2D(Collider2D ladC) {
        if (ladC.gameObject.CompareTag("ladder")) {
            endClimb();
        }
    }
    
    void Crouch() {
        a.runtimeAnimatorController = crouch;
    }
    public void Attack()
    {
        Debug.Log("atta");
        attackTime = 0f;
    }
    void Roll()
    {
        a.runtimeAnimatorController = roll;
    }
    void Idle()
    {
        a.runtimeAnimatorController = idle;
    }
    public void Run()
    {
        a.runtimeAnimatorController = run;
    }
    void Jump()
    {
        jumpTmp++;
        if (JbreakDown)
        {
            jumpTmp = 0;
            JbreakDown = false;
        }
        if (jumpTmp <= 0.1 * deltaTmp)
        {
            if (!jumpClipTmp)
            {
                GetComponent<AudioSource>().PlayOneShot(jumpClip);
                jumpClipTmp = true;
            }

            a.runtimeAnimatorController = jump;
        }
        else if (jumpTmp <= 0.5 * deltaTmp)
        {
            jumpClipTmp = false;
            a.runtimeAnimatorController = jumpMid;
        }
        else
        {
            a.runtimeAnimatorController = jumpFall;
        }
    }



    void Slide()
    {
        a.runtimeAnimatorController = slide;
    }
    void Hurted()
    {
        a.runtimeAnimatorController = jumpFall;
    }
    void Die()
    {
        a.runtimeAnimatorController = die;
        isDieAnimationPlaying = true;
        shakeSound.GetComponent<AudioSource>().volume -= 10 * Time.smoothDeltaTime;
        DiaryBehavior.End();
        DiaryBehavior2.End();
    }

    public void PauseGame()
    {
        // CreateButton("Click Me", OnButtonClick);
        //Debug.Log("PauseApped");

        panel.SetActive(true);
        UnityEngine.UI.Button sample = Resources.Load<UnityEngine.UI.Button>("Prefabs/ReplayButton") as UnityEngine.UI.Button;
        UnityEngine.UI.Button button = Instantiate(sample, text);
        text.GetComponent<TextMeshProUGUI>().text = "Is that all?";
        button.transform.localPosition += new Vector3(0, 70f, 0);
        button.onClick.AddListener(ClickButton);
        Time.timeScale = 0f;
        Debug.Log(button.transform.localPosition);
    }

    void ClickButton()
    {
        Debug.Log("click");
        if (specialPiece.CompareTag("Piece4")) {
            SceneManager.LoadScene(2);
        }else if (specialPiece.CompareTag("Piece5"))
        {
            SceneManager.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
        
        Time.timeScale = 1f;
        DiaryBehavior.Continue();
        DiaryBehavior2.Continue();
    }

    // public void CreateButton(string buttonText, UnityEngine.Events.UnityAction onClickAction)
    // {
    //     if (panel == null || buttonPrefab == null)
    //     {
    //         Debug.LogError("Panel or ButtonPrefab is not assigned.");
    //         return;
    //     }

    //     // 创建按钮实例
    //     UnityEngine.UI.Button newButton = Instantiate(buttonPrefab, panel.transform);

    //     // 设置按钮文本
    //     TMP_Text buttonTextComponent = newButton.GetComponentInChildren<TMP_Text>();
    //     if (buttonTextComponent != null)
    //     {
    //         buttonTextComponent.text = buttonText;
    //     }

    //     // 添加点击事件监听器
    //     newButton.onClick.AddListener(onClickAction);
    // }

    // private void OnButtonClick()
    // {
    //     Debug.Log("Button clicked!");
    // }
}
