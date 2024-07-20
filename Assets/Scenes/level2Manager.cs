using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI;

public class level2Manager : MonoBehaviour
{
    public Canvas canvas;
    public Image blackImage;
    public Image level2;
    public Image level2_music;
    public Vector3 startScale = new Vector3(4.55625f, 8.1f, 1f);   // 起始缩放比例
    public Vector3 endScale = new Vector3(5.0625f, 9f, 1f);     // 结束缩放比例
    public float duration = 2.0f;                       // 变化持续时间

    private float currentTime = 0f;                     // 当前时间

    public float timer = 0f;
    public static int level2PlayCount = 0;
    public GameObject player; //获取player


    // Start is called before the first frame update
    void Start()
    {

        level2PlayCount++;
        //获取level2和level2_music
        blackImage.enabled = false;
        level2.enabled = false;
        level2_music.enabled = false;

        //获取player
        player = GameObject.Find("Player");
        player.GetComponent<HeroBehavior>().currentLevel = 2;//传递当前关卡


        if (level2PlayCount == 1)
        {
            blackImage.enabled = true;
            //禁用ControlFalling的AddRigidbodyToTilemap脚本
            GameObject controlFalling = GameObject.Find("ControlFalling");
            controlFalling.GetComponent<AddRigidbodyToTilemap>().enabled = false;

            Invoke("showLevel2", 0);
            Invoke("hideLevel2", 2);
            Invoke("showLevel2Music", 3);
            Invoke("hideLevel2Music", 5);
            Invoke("blackImageDisappear", 5);

            //启用ControlFalling的AddRigidbodyToTilemap脚本
            Invoke("EnableAddRigidbodyToTilemap", 6);
        }
        else
        {

            //启用ControlFalling的AddRigidbodyToTilemap脚本
            Invoke("EnableAddRigidbodyToTilemap", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 3) { level2Large(); }

        //三秒钟后level2_music放大
        if (timer >= 3)
        {
            level2MusicLarge();
        }
        //Debug.Log(timer);

        if (timer >= 6)
        {
            //删掉blackImage，level2，level2_music
            Destroy(blackImage);
            Destroy(level2);
            Destroy(level2_music);
        }
    }

    void showLevel2()
    {
        //将level1的透明度先设为0
        level2.CrossFadeAlpha(0, 0, false);
        level2.enabled = true;
        //逐渐显示
        level2.CrossFadeAlpha(1, 1, false);

    }
    void hideLevel2()
    {
        level2.CrossFadeAlpha(0, 1, false);
    }
    void showLevel2Music()
    {
        //将level1_music的透明度先设为0
        level2_music.CrossFadeAlpha(0, 0, false);
        level2_music.enabled = true;
        //逐渐显示
        level2_music.CrossFadeAlpha(1, 1, false);
    }
    void hideLevel2Music()
    {
        level2_music.CrossFadeAlpha(0, 1, false);
    }

    void EnableAddRigidbodyToTilemap()
    {
        GameObject controlFalling = GameObject.Find("ControlFalling");
        controlFalling.GetComponent<AddRigidbodyToTilemap>().enabled = true;
        //禁用自身
        //this.enabled = false;
    }
    void blackImageDisappear()
    {
        blackImage.color = new Color(0, 0, 0, 0);
    }
    void level2Large()
    {
        // 累加时间
        currentTime += Time.deltaTime;
        // 计算当前缩放比例
        level2.rectTransform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);

        // 当时间超过duration时，停止在endScale
        if (currentTime >= duration)
        {
            level2.rectTransform.localScale = endScale;
        }

        //Debug.Log(level2.rectTransform.localScale);
    }

    void level2MusicLarge()
    {
        Debug.Log("level2MusicLarge");
        // 累加时间
        currentTime += Time.deltaTime;
        // 计算当前缩放比例
        level2_music.rectTransform.localScale = Vector3.Lerp(startScale, endScale, (currentTime - 3) / duration);

        // 当时间超过duration时，停止在endScale
        if (currentTime - 3 >= duration)
        {
            level2_music.rectTransform.localScale = endScale;
        }

        //Debug.Log(level2_music.rectTransform.localScale);
    }
}
