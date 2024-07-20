using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI;

public class level3Manager : MonoBehaviour
{
    public Canvas canvas;
    public Image blackImage;
    public Image level3;
    public Image level3_music;
    public Vector3 startScale = new Vector3(8.1f, 4.55625f, 1f);   // 起始缩放比例
    public Vector3 endScale = new Vector3(9f, 5.0625f, 1f);     // 结束缩放比例
    public float duration = 2.0f;                       // 变化持续时间

    private float currentTime = 0f;                     // 当前时间

    public float timer = 0f;
    public static int level3PlayCount = 0;
    public GameObject player; //获取player

    // Start is called before the first frame update
    void Start()
    {
        level3PlayCount++;
        //获取level2和level2_music
        // level3 = GameObject.Find("level3").GetComponent<Image>();
        // level3_music = GameObject.Find("level3_music").GetComponent<Image>();

        blackImage.enabled = false;
        level3.enabled = false;
        level3_music.enabled = false;
        //获取player
        player = GameObject.Find("Player");
        player.GetComponent<HeroBehavior>().currentLevel = 3;//传递当前关卡


        if (level3PlayCount == 1)
        {
            blackImage.enabled = true;
            //禁用ControlFalling的AddRigidbodyToTilemap脚本
            GameObject controlFalling = GameObject.Find("ControlFalling");
            controlFalling.GetComponent<AddRigidbodyToTilemap>().enabled = false;

            Invoke("showLevel3", 0);
            Invoke("hideLevel3", 2);
            Invoke("showLevel3Music", 3);
            Invoke("hideLevel3Music", 5);
            Invoke("blackImageDisappear1", 5);



            //启用ControlFalling的AddRigidbodyToTilemap脚本
            Invoke("EnableAddRigidbodyToTilemap1", 6);
        }
        else
        {
            
            //启用ControlFalling的AddRigidbodyToTilemap脚本
            Invoke("EnableAddRigidbodyToTilemap1", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 3) { level3Large(); }

        //三秒钟后level2_music放大
        if (timer >= 3)
        {
            level3MusicLarge();
        }
        //Debug.Log(timer);

        if (timer >= 6)
        {
            //删掉blackImage，level2，level2_music
            Destroy(blackImage);
            Destroy(level3);
            Destroy(level3_music);
        }
    }

    void showLevel3()
    {
        //将level1的透明度先设为0
        level3.CrossFadeAlpha(0, 0, false);
        level3.enabled = true;
        //逐渐显示
        level3.CrossFadeAlpha(1, 1, false);

    }
    void hideLevel3()
    {
        level3.CrossFadeAlpha(0, 1, false);
    }
    void showLevel3Music()
    {
        //将level1_music的透明度先设为0
        level3_music.CrossFadeAlpha(0, 0, false);
        level3_music.enabled = true;
        //逐渐显示
        level3_music.CrossFadeAlpha(1, 1, false);
    }
    void hideLevel3Music()
    {
        level3_music.CrossFadeAlpha(0, 1, false);
    }

    void EnableAddRigidbodyToTilemap1()
    {
        GameObject controlFalling = GameObject.Find("ControlFalling");
        controlFalling.GetComponent<AddRigidbodyToTilemap>().enabled = true;
        //禁用自身
        //this.enabled = false;
    }
    void blackImageDisappear1()
    {
        blackImage.color = new Color(0, 0, 0, 0);
    }
    void level3Large()
    {
        // 累加时间
        currentTime += Time.deltaTime;
        // 计算当前缩放比例
        level3.rectTransform.localScale = Vector3.Lerp(startScale, endScale, currentTime / duration);

        // 当时间超过duration时，停止在endScale
        if (currentTime >= duration)
        {
            level3.rectTransform.localScale = endScale;
        }

        //Debug.Log(level2.rectTransform.localScale);
    }

    void level3MusicLarge()
    {
        Debug.Log("level2MusicLarge");
        // 累加时间
        currentTime += Time.deltaTime;
        // 计算当前缩放比例
        level3_music.rectTransform.localScale = Vector3.Lerp(startScale, endScale, (currentTime - 3) / duration);

        // 当时间超过duration时，停止在endScale
        if (currentTime - 3 >= duration)
        {
            level3_music.rectTransform.localScale = endScale;
        }

        //Debug.Log(level2_music.rectTransform.localScale);
    }
}
