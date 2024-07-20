using UnityEngine;
using System.Collections;

public class StoryActivator : MonoBehaviour
{
    public GameObject story1;
    public GameObject story2;
    public GameObject story3;
    public GameObject story4;
    public GameObject story5;
    public float fadeDuration = 1f; // 渐变持续时间

    //////////////
    public GameObject yin1;
    public GameObject yin2;
    public GameObject yin3;
    public GameObject yin4;
    public GameObject yin5;

    private bool is1 = false;
    private bool is2 = false;
    private bool is3 = false;
    private bool is4 = false;
    private bool is5 = false;

    public float speed = 0.01f;
    public RuntimeAnimatorController c0,c1,c2,c3,c4;


    // private void OnTriggerEnter(Collider other)
    // {
    //     switch (other.gameObject.name)
    //     {
    //         case "green":
    //             Debug.Log("绿");
    //             StartCoroutine(FadeIn(story1));
    //             break;
    //         case "粉音符":
    //             StartCoroutine(FadeIn(story2));
    //             break;
    //         case "蓝音符":
    //             StartCoroutine(FadeIn(story3));
    //             break;
    //         case "黄音符":
    //             StartCoroutine(FadeIn(story4));
    //             break;
    //         case "红音符":
    //             StartCoroutine(FadeIn(story5));
    //             break;
    //     }
    // }
    void Start()
    {
        c0 = Resources.Load<RuntimeAnimatorController>("ImageAnimation/XHX/Image0C");
        c1 = Resources.Load<RuntimeAnimatorController>("ImageAnimation/XHX/Image1C");
        c2 = Resources.Load<RuntimeAnimatorController>("ImageAnimation/XHX/Image2C");
        c3 = Resources.Load<RuntimeAnimatorController>("ImageAnimation/XHX/Image3C");
        c4 = Resources.Load<RuntimeAnimatorController>("ImageAnimation/XHX/Image4C");
    }
    private void UpdateYinfu()
    {
        

        if (transform.gameObject.GetComponent<Rigidbody2D>().IsTouching(yin1.GetComponent<BoxCollider2D>())) {
            is1 = true;
        }
        if (transform.gameObject.GetComponent<Rigidbody2D>().IsTouching(yin2.GetComponent<BoxCollider2D>())) {
            is2 = true;
        }
        if (transform.gameObject.GetComponent<Rigidbody2D>().IsTouching(yin3.GetComponent<BoxCollider2D>())) {
            is3 = true;
        }
        if (transform.gameObject.GetComponent<Rigidbody2D>().IsTouching(yin4.GetComponent<BoxCollider2D>())) {
            is4 = true;
        }
        if (transform.gameObject.GetComponent<Rigidbody2D>().IsTouching(yin5.GetComponent<BoxCollider2D>())) {
            is5 = true;
        }
    }

    void Update()
    {
        UpdateYinfu();
        UpdateStory();
    }

    // void FadeIn(GameObject storyObject)
    // {
    //     storyObject.SetActive(true);
    //     CanvasGroup canvasGroup = storyObject.GetComponent<CanvasGroup>();
    //     if (canvasGroup == null)
    //     {
    //         canvasGroup = storyObject.AddComponent<CanvasGroup>();
    //     }
    //     canvasGroup.alpha = 0f;

    //     float elapsedTime = 0f;

    //     while (elapsedTime < fadeDuration)
    //     {
    //         canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }

    //     canvasGroup.alpha = 1f;
    // }

    void UpdateStory() {
        if(is1) {
            Color color = story1.GetComponent<SpriteRenderer>().color;
            color.a += speed;
            story1.GetComponent<SpriteRenderer>().color = color;
            story1.GetComponent<Animator>().runtimeAnimatorController = c0;
        }
        if(is2) {
            Color color = story2.GetComponent<SpriteRenderer>().color;
            color.a += speed;
            story2.GetComponent<SpriteRenderer>().color = color;
            story2.GetComponent<Animator>().runtimeAnimatorController = c1;
        }
        if(is3) {
            Color color = story3.GetComponent<SpriteRenderer>().color;
            color.a += speed;
            story3.GetComponent<SpriteRenderer>().color = color;
            story3.GetComponent<Animator>().runtimeAnimatorController = c2;
        }
        if(is4) {
            Color color = story4.GetComponent<SpriteRenderer>().color;
            color.a += speed;
            story4.GetComponent<SpriteRenderer>().color = color;
            story4.GetComponent<Animator>().runtimeAnimatorController = c3;
        }
        if(is5) {
            Color color = story5.GetComponent<SpriteRenderer>().color;
            color.a += speed;
            story5.GetComponent<SpriteRenderer>().color = color;
            story5.GetComponent<Animator>().runtimeAnimatorController = c4;
        }
        
    }
}
