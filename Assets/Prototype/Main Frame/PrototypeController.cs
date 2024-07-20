using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrototypeController : MonoBehaviour
{
    private Sprite sprite;
    [SerializeField] Transform button;
    private bool open = false;

    public void LoadCollapse()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadCat()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadCollecting()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadEnemy()
    {
        SceneManager.LoadScene(5);
    }

    public void LoadRock()
    {
        SceneManager.LoadScene(6);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenDiary()
    {
        if (open)
        {
            sprite = Resources.Load("Image/OpenBook", typeof(Sprite)) as Sprite;
            button.GetComponent<Image>().sprite = sprite;
        }else
        {
            sprite = Resources.Load("Image/DiaryCover", typeof(Sprite)) as Sprite;
            button.GetComponent<Image>().sprite = sprite;
        }
        open = !open;
    }
}
