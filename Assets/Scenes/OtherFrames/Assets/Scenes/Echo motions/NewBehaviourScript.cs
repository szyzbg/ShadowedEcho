using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class NewBehaviourScript : MonoBehaviour
{
    //public variables for play video
    public GameObject Video1;
    public GameObject Video2;
    public Transform PlayButton;
    public Transform LV2Button;
    public Transform LV3Button;

    //public variables for little Echo
    [SerializeField]
    Image Echo;
    [SerializeField]
    Image Cat;
    private bool isCat = false;

    private float preTime = 0.0f;
    private int condition = 0;
    private bool over1 = false;


    // Start is called before the first frame update

    void Start()
    {
        preTime = Time.realtimeSinceStartup;
        Color color1 = PlayButton.GetComponent<Image>().color;
        Color color2 = LV2Button.GetComponent<Image>().color;
        Color color3 = LV3Button.GetComponent<Image>().color;
        color1.a = 0;
        color2.a = 0;
        color3.a = 0;
        PlayButton.GetComponent<Image>().color = color1;
        LV2Button.GetComponent<Image>().color = color2;
        LV3Button.GetComponent<Image>().color = color3;

    }

    // Update is called once per frame
    void Update()
    {
        if (!over1 && Input.GetKeyDown(KeyCode.E))
        {
            Video1.SetActive(false);
        }
        if (!over1 && !Video1.GetComponent<VideoPlayer>().isActiveAndEnabled)
        {
            over1 = true;
            VideoPlayer videoPlayer = Video2.GetComponent<VideoPlayer>();
            videoPlayer.url = System.IO.Path.Combine (Application.streamingAssetsPath,"循环字幕.mp4"); 
            videoPlayer.Play();
            // Video2.GetComponent<VideoPlayer>().Play();
            Color color1 = PlayButton.GetComponent<Image>().color;
            Color color2 = LV2Button.GetComponent<Image>().color;
            Color color3 = LV3Button.GetComponent<Image>().color;
            color1.a = 1;
            color2.a = 1;
            color3.a = 1;
            PlayButton.GetComponent<Image>().color = color1;
            LV2Button.GetComponent<Image>().color = color2;
            LV3Button.GetComponent<Image>().color = color3;
            preTime = Time.realtimeSinceStartup-2f;

        }
        if (Time.realtimeSinceStartup - preTime >= 27.3f)
        {
            Video1.SetActive(false);
        }

        UpdateCondition();

        UpdateLittleEcho();
    }

    void UpdateCondition()
    {
        if (over1 && Time.realtimeSinceStartup - preTime > 2f)
        {
            preTime = Time.realtimeSinceStartup;
            if (condition == 0)
            {
                condition = 1;
                Echo.sprite = Resources.Load("Image/Echo1", typeof(Sprite)) as Sprite;
                Color color = Echo.color;
                color.a = 0.0f;
                Echo.color = color;
            }
            else if (condition == 1)
            {
                condition = 2;
                Echo.sprite = Resources.Load("Image/Echo2", typeof(Sprite)) as Sprite;
                Color color = Echo.color;
                color.a = 0.0f;
                Echo.color = color;
                Vector3 scale = Echo.transform.localScale;
                scale.x = scale.x * 1.2f;
                scale.y = scale.y * 0.75f;
                Echo.transform.localScale = scale;
            }
            else if (condition == 2)
            {
                condition = 3;
                isCat = true;
            }
        }
    }

    void UpdateLittleEcho()
    {
        Color color0 = Echo.color;
        if (condition != 0 && color0.a < 0.9f)
        {
            color0.a += 0.005f;
        }
        else if (condition != 0)
        {
            color0.a = 1f;
        }
        Echo.color = color0;

        if (isCat)
        {
            Color color = Cat.color;
            if (color.a < 0.9f)
            {
                color.a += 0.005f;
            }
            else
            {
                color.a = 1f;
                isCat = false;
            }
            Cat.color = color;
        }
    }
}
