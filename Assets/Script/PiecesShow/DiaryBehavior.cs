using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class DiaryBehavior : MonoBehaviour
{
    public Image myImage;//diary content
    public GameObject specialPiece;

    public Image tutorial;//tutorial image

    private Sprite sprite;
    [SerializeField] Transform button;// ciary button
    public Transform button0;// close button
    private bool open = false;

    private static int conditon = 0;//0-no pieces,1-piece1,2-piece2,3-piece12
    private static bool needUpdate = false;

    private static bool end = false; // check game over or not

    // Start is called before the first frame update
    void Start()
    {
        myImage.sprite = Resources.Load("Image/diary0", typeof(Sprite)) as Sprite;
        Color color = myImage.color;
        color.a = 0;
        myImage.color = color;
        button0.GetComponent<Image>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateImage();
    }

    void UpdateImage()
    {
        if (needUpdate)
        {
            needUpdate = false;
            if (conditon == 0)
            {
                myImage.sprite = Resources.Load("Image/diary0", typeof(Sprite)) as Sprite;
            }else if (conditon == 1)
            {
                myImage.sprite = Resources.Load("Image/diary1", typeof(Sprite)) as Sprite;
            }
            else if (conditon == 2)
            {
                myImage.sprite = Resources.Load("Image/diary2", typeof(Sprite)) as Sprite;
            }
            else if (conditon == 3)
            {
                myImage.sprite = Resources.Load("Image/diary12", typeof(Sprite)) as Sprite;
            }

        }
    }

    public static void UpdateCondition(int i)
    {
        if (conditon == 0)
        {
            conditon = i;
        }else if (conditon == 1)
        {
            if (i == 2)
            {
                conditon = 3;
            }
        }else if (conditon == 2)
        {
            if (i == 1)
            {
                conditon = 3;
            }
        }
        needUpdate = true;
    }

    //update diary button image
    public void OpenDiary()
    {
        if (!open)
        {
            Color color = myImage.color;
            color.a = 1;
            myImage.color = color;
            button0.GetComponent<Image>().color = color;
            sprite = Resources.Load("Image/OpenBook", typeof(Sprite)) as Sprite;
            button.GetComponent<Image>().sprite = sprite;
            open = true;

            Time.timeScale = 0f;

        }
    }

    public void CloseDiary()
    {
        if (open)
        {
            sprite = Resources.Load("Image/DiaryCover", typeof(Sprite)) as Sprite;
            button.GetComponent<Image>().sprite = sprite;
            open = false;
            if (!end)
            {
                Time.timeScale = 1f;
            }
        }
    }

    public static void End()
    {
        end = true;
    }

    public static void Continue()
    {
        end = false;
    }

    public void NextPage()
    {
        tutorial.sprite = Resources.Load("Image/Tutorial2", typeof(Sprite)) as Sprite;
    }

    public void LastPage()
    {
        tutorial.sprite = Resources.Load("Image/Tutorial1", typeof(Sprite)) as Sprite;
    }

    public void ReturnHome()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

}
