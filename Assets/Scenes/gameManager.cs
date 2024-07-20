using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI;

public class BlackScreenController : MonoBehaviour
{
    public Canvas canvas;
    public Image blackScreenImage;
    public Image level1;
    public Image level1_music;
    public TMP_Text who;
    public TMP_Text am;
    public TMP_Text i;
    public TMP_Text question1;
    public TMP_Text where;
    public TMP_Text am2;
    public TMP_Text i2;
    public TMP_Text question2;
    public GameObject player;
    public float interval1 = 0.3f;
    public float interval2 = 1f;

    public GameObject mainCamera;

    public Vector3 startScale = new Vector3(4.55625f, 8.1f, 1f);   // 起始缩放比例
    public Vector3 endScale = new Vector3(5.0625f, 9f, 1f);     // 结束缩放比例
    public float duration = 2.0f;                       // 变化持续时间

    private float currentTime = 0f;                     // 当前时间
    public float timer = 0f;
    public Image blackImage;
    //关卡游玩次数计数器
    public static int level1PlayCount = 0;
    

    void Start()
    {
        level1PlayCount++;
        // 初始状态为启用
        blackScreenImage.enabled = false;

        //首先禁用所有文本
        who.enabled = false;
        am.enabled = false;
        i.enabled = false;
        question1.enabled = false;
        where.enabled = false;
        am2.enabled = false;
        i2.enabled = false;
        question2.enabled = false;
        blackImage.enabled = false;
        level1.enabled = false;
        level1_music.enabled = false;

        //获取player
        player = GameObject.Find("Player");
        player.GetComponent<HeroBehavior>().currentLevel = 1;//传递当前关卡

        if (level1PlayCount == 1)
        {
            mainCamera = GameObject.Find("Main Camera");
            //禁用ControlFalling的AddRigidbodyToTilemap脚本
            GameObject controlFalling = GameObject.Find("ControlFalling");
            controlFalling.GetComponent<AddRigidbodyToTilemap>().enabled = false;

            blackScreenImage.enabled = true;
            blackImage.enabled = true;

            Invoke("showLevel1", 0);
            Invoke("hideLevel1", 2);
            Invoke("showLevel1Music", 3);
            Invoke("hideLevel1Music", 5);
            Invoke("blackImageDisappear", 5);

            //延时显示文本
            Invoke("ShowWho", interval1 + 6);
            Invoke("ShowAm", interval1 * 2 + 6);
            Invoke("ShowI", interval1 * 3 + 6);
            Invoke("ShowQuestion1", interval1 * 4 + 6);
            Invoke("ShowWhere", interval1 * 4 + interval2 + 6);
            Invoke("ShowAm2", interval1 * 5 + interval2 + 6);
            Invoke("ShowI2", interval1 * 6 + interval2 + 6);
            Invoke("ShowQuestion2", interval1 * 7 + interval2 + 6);

            //逐步改变透明度
            Invoke("changeAlpha", interval1 * 7 + 3 * interval2 + 6);

            //启用ControlFalling的AddRigidbodyToTilemap脚本
            Invoke("EnableAddRigidbodyToTilemap", interval1 * 7 + 3 * interval2 + 2 + 6);

        }
        else
        {
            //启用ControlFalling的AddRigidbodyToTilemap脚本
            Invoke("EnableAddRigidbodyToTilemap", 0);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 3) { level1Large(); }

        //三秒钟后level2_music放大
        if (timer >= 3)
        {
            level1MusicLarge();
        }
        //Debug.Log(timer);
        if (timer >= 6f)
        {
            //删掉blackImage，level2，level2_music
            Destroy(blackImage);
            Destroy(level1);
            Destroy(level1_music);
        }
    }

    #region showText
    void ShowWho()
    {
        who.enabled = true;
    }

    void ShowAm()
    {
        am.enabled = true;
    }

    void ShowI()
    {
        i.enabled = true;
    }

    void ShowQuestion1()
    {
        question1.enabled = true;
    }

    void ShowWhere()
    {
        where.enabled = true;
    }

    void ShowAm2()
    {
        am2.enabled = true;
    }

    void ShowI2()
    {
        i2.enabled = true;
    }

    void ShowQuestion2()
    {
        question2.enabled = true;
    }

    #endregion

    void changeAlpha()
    {
        //逐步改变透明度
        blackScreenImage.CrossFadeAlpha(0, 2, false);
        //改变文字透明度
        who.CrossFadeAlpha(0, 2, false);
        am.CrossFadeAlpha(0, 2, false);
        i.CrossFadeAlpha(0, 2, false);
        question1.CrossFadeAlpha(0, 2, false);
        where.CrossFadeAlpha(0, 2, false);
        am2.CrossFadeAlpha(0, 2, false);
        i2.CrossFadeAlpha(0, 2, false);
        question2.CrossFadeAlpha(0, 2, false);

    }

    void EnableAddRigidbodyToTilemap()
    {
        GameObject controlFalling = GameObject.Find("ControlFalling");
        controlFalling.GetComponent<AddRigidbodyToTilemap>().enabled = true;
        //禁用自身
        //this.enabled = false;
    }

    void showLevel1()
    {
        //将level1的透明度先设为0
        level1.CrossFadeAlpha(0, 0, false);
        level1.enabled = true;
        //逐渐显示
        level1.CrossFadeAlpha(1, 1, false);

    }
    void hideLevel1()
    {
        level1.CrossFadeAlpha(0, 1, false);
    }
    void showLevel1Music()
    {
        //将level1_music的透明度先设为0
        level1_music.CrossFadeAlpha(0, 0, false);
        level1_music.enabled = true;
        //逐渐显示
        level1_music.CrossFadeAlpha(1, 1, false);
    }
    void hideLevel1Music()
    {
        level1_music.CrossFadeAlpha(0, 1, false);
    }

    void level1Large()
    {
        // 累加时间
        currentTime += Time.deltaTime;
        // 计算当前缩放比例
        level1.rectTransform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);

        // 当时间超过duration时，停止在endScale
        if (currentTime >= duration)
        {
            level1.rectTransform.localScale = endScale;
        }

        //Debug.Log(level2.rectTransform.localScale);
    }

    void level1MusicLarge()
    {
        //   Debug.Log("level2MusicLarge");
        // 累加时间
        currentTime += Time.deltaTime;
        // 计算当前缩放比例
        level1_music.rectTransform.localScale = Vector3.Lerp(startScale, endScale, (currentTime - 3) / duration);

        // 当时间超过duration时，停止在endScale
        if (currentTime - 3 >= duration)
        {
            level1_music.rectTransform.localScale = endScale;
        }

        //Debug.Log(level2_music.rectTransform.localScale);

    }
    void blackImageDisappear()
    {
        blackImage.color = new Color(0, 0, 0, 0);
    }
}
