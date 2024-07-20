using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class FinalDialog : MonoBehaviour
{
    private AudioClip a;
    public DialogManager3 diam;
    public GameObject canvas;
    public GameObject cam;

    public GameObject video; // for ending video play
    private bool check = false;
    private float befTime = 0.0f;

    private int finalDiaTMP = 0;
    private int buffer = 0;
    public Image blackImage;

    // Start is called before the first frame update
    void Start()
    {
        a = Resources.Load<AudioClip>("Sounds/FullSizeRender");
       // diam = GameObject.Find("DiaP3").GetComponent<DialogManager3>();
       blackImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (check && Time.realtimeSinceStartup - befTime >= 49.0f)
        {
            SceneManager.LoadScene(0);
        }

        if (transform.position.x >= 240) {
            if (finalDiaTMP == 0) {
                Time.timeScale = 0;
                diam.ShowDialogHero("You mean, my cat use all 9 lives to create this world for me.");
                if (Input.GetMouseButtonDown(0)) {
                    finalDiaTMP++;
                }
            }
        }
        if (finalDiaTMP == 1) {
            if (buffer <= 10) {
                buffer++;
            }
            diam.ShowDialogHero("So that I can find the path to after life?");
            if (buffer >= 10 && Input.GetMouseButtonDown(0)) {
                finalDiaTMP++;
            }
        }
        if (finalDiaTMP == 2) {
            if (buffer <= 20) {
                buffer++;
            }
            diam.ShowDialogInvi("We were always alone when we were alive. But here, you have your cat, me, and the player in front of the screen.");
            if (buffer >= 20 && Input.GetMouseButtonDown(0)) {
                finalDiaTMP++;
            }
        }
        if (finalDiaTMP == 3) {
            if (buffer <= 30) {
                buffer++;
            }
            diam.ShowDialogInvi("I can't bear the lonely real life any more, so I try to kill you and stop you from winning the game.");
            if (buffer >= 30 && Input.GetMouseButtonDown(0)) {
                finalDiaTMP++;
            }
        }
        if (finalDiaTMP == 4) {
            if (buffer <= 40) {
                buffer++;
            }
            diam.ShowDialogHero("......");
            if (buffer >= 40 && Input.GetMouseButtonDown(0)) {
                finalDiaTMP++;
            }
        }
        if (finalDiaTMP == 5) {
            if (buffer <= 50) {
                buffer++;
            }
            diam.ShowDialogHero("What is your choice?\nY: Lonely but real life\nN: Stay in this game forever");
            if (buffer >= 50 && Input.GetKeyDown(KeyCode.Y)) {
                finalDiaTMP++;
                Time.timeScale = 1;
                diam.CloseDialog();
                //to do
                Debug.Log("Yes");
                VideoPlayer videoPlayer = video.GetComponent<VideoPlayer>();
                videoPlayer.url = System.IO.Path.Combine (Application.streamingAssetsPath,"Ending.mp4"); 
                videoPlayer.Play();
                // video.GetComponent<VideoPlayer>().Play();
                canvas.SetActive(false);
                cam.GetComponent<AudioSource>().volume = 0;
                check = true;
                befTime = Time.realtimeSinceStartup;
            }
            if (buffer >= 50 && Input.GetKeyDown(KeyCode.N)) {
                finalDiaTMP++;
                Time.timeScale = 1;
                diam.CloseDialog();
                //to do
                //黑屏 播放音效 切回Homepage

                blackImage.enabled = true;
                //透明度调成1
                blackImage.color = new Color(0, 0, 0, 1);

                GetComponent<AudioSource>().PlayOneShot(a);

                //等待三秒
                if (Time.realtimeSinceStartup - befTime >= 3.0f)
                {
                    //切回Homepage
                    SceneManager.LoadScene(1);
                    Debug.Log("No");
                }
            }
        } 
    }
}
