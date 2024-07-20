using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class endForEcho : MonoBehaviour
{
    private int fordialogTMP = 0;
    public DialogManager3 dm3;
    private int saveDiaCount = 0;
    public GameObject player; // 引用玩家的Transform组件
    public HeroBehavior heroBehavior; // 引用玩家的HeroBehavior组件
    public bool isSaving = false;
    public bool choseNotSave=false;
    public SpriteRenderer sky;
    public SpriteRenderer clouds_01;
    public SpriteRenderer mountains_02;
    public SpriteRenderer mountains_01;
    public SpriteRenderer trees_03;
    public SpriteRenderer trees_02;
    public SpriteRenderer trees_01;
    public SpriteRenderer invisibleMan;
    public Image lost;
    public bool temp=false;
    void Start()
    {
        dm3 = GameObject.Find("DiaP3").GetComponent<DialogManager3>();
        player = GameObject.Find("Player");
        heroBehavior = player.GetComponent<HeroBehavior>();
        lost.enabled=false;
    }
    void Update()
    {

        //首先判断玩家的位置 140，57.5
        if (player.transform.position.x >= 140 && player.transform.position.y >= 57.5)
        {
            //弹出对话框
            if (saveDiaCount == 0)
            {
                heroBehavior.inputEnabled = false;
                Time.timeScale = 0;
                dm3.ShowDialogInvi("HELP ME!!!");
                if (Input.GetMouseButtonDown(0))
                {
                    saveDiaCount++;
                }
            }
        }

        if (saveDiaCount == 1)
        {
            fordialogTMP++;
            Debug.Log("savedc=1");
            dm3.ShowDialogHero("Do you want to save him? The invisible man who wants to kill you before.");
            if (Input.GetMouseButtonDown(0) && fordialogTMP > 10) {
                saveDiaCount++;
            }
        }
        if (saveDiaCount == 2)
        {
            dm3.ShowDialogHero("(Y for yes, N for no)");
            if (Input.GetKeyDown(KeyCode.Y))
            {
                saveDiaCount++;
                Time.timeScale = 1;
                // Do something
                //ECHO选择救
                

                heroBehavior.jumpForce=1000;
                heroBehavior.jump();
                isSaving = true;

                dm3.CloseDialog();
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                choseNotSave=true;
                saveDiaCount++;
                Time.timeScale = 1;
                // Do something
                //ECHO选择不救
                dm3.CloseDialog();

                //笼子掉下去
                Rigidbody2D rb=invisibleMan.gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 2.0f;
            }
        }

        if (player.transform.position.x <= 148&&isSaving)
        {
            heroBehavior.moveSpeed=8;
            heroBehavior.moveForward();
        }

        if(player.transform.position.y<=68&&player.transform.position.x>=147&&isSaving){
            //全屏黑掉
            sky.color=Color.black;
            clouds_01.color=Color.black;
            mountains_02.color=Color.black;
            mountains_01.color=Color.black;
            trees_03.color=Color.black;
            trees_02.color=Color.black;
            trees_01.color=Color.black;
            invisibleMan.color=Color.black;

            HeroBehavior.passedSavePoint=false;
        }

        if(choseNotSave&&!temp){
            //选择不救 延迟两秒后展示
            Invoke("showLost",3);
            Invoke("hideLost",5);
            Invoke("die",6);
            Invoke("enableMouse",6);
            temp=true;
        }

        if(isSaving&&player.transform.position.y<=2&&player.transform.position.x>=144){
            heroBehavior.inputEnabled = true;
        }
    }

    void showLost(){
        lost.CrossFadeAlpha(0, 0, false);
        lost.enabled = true;
        lost.CrossFadeAlpha(1, 1, false);
    }
    void hideLost(){
        lost.CrossFadeAlpha(0, 1, false);
    }
    void die(){
        HealthSystem1.Instance.alive=false;
    }
    void enableMouse(){
        heroBehavior.inputEnabled = true;
    }
}
