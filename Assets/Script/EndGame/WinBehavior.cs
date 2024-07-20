using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinBehavior : MonoBehaviour
{
    private AudioClip spSound;
    public GameObject ProgressBar = null; //������
    public GameObject hero = null; //����
    public GameObject panel; // 引用Panel对象
    public RectTransform text = null;

    public GameObject ParticleSystem1; //for particle effect
    public GameObject ParticleSystem2;
    private bool emission = false;

    private BoxCollider2D collider = null;
    private Vector3 size = new Vector3(0.0f, 0.1f, 1.0f); //��ʼ������λ��
    private float mMaxBarWidth = 0.8f;
    private float speed = 0.4f;
    private bool mActive = false;
    private bool mFinished = false;
    private float temp = 1.0f;

    private float x = 0.0f;
    private RuntimeAnimatorController c;
    private RuntimeAnimatorController a;
    private RuntimeAnimatorController b;
    private Animator tmp;

    // Start is called before the first frame update
    void Start()
    {
        spSound = Resources.Load<AudioClip>("Sounds/拾取碎片");
        Debug.Assert(ProgressBar != null);
        Debug.Assert(hero != null);
        collider = hero.GetComponent<BoxCollider2D>();
        ProgressBar.transform.localScale = Vector3.zero;
        tmp = hero.GetComponent<Animator>();
        c = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/CrouchC");
        b = Resources.Load<RuntimeAnimatorController>("Hero/Art/Animations/IdleC");
      //  tmp.runtimeAnimatorController = b;
    }

    // Update is called once per frame
    void Update()
    {

        if (mFinished)
        {
            ProgressBar.transform.localScale = Vector3.zero;
            if (emission)
            {
                ParticleSystem1.SetActive(false);
                ParticleSystem2.GetComponent<ParticleSystem>().Play();
                emission = false;
                GetComponent<AudioSource>().PlayOneShot(spSound);

                //捡完了 需要将通过存档点设为false
                HeroBehavior.passedSavePoint=false;
            }

            if (temp < 60)
            {
                temp += 0.6f;
                UpdatePieces();
            }
            else
            {
                if (GetComponent<SpriteRenderer>().color.a > 0.3)
                {
                    Color color = GetComponent<SpriteRenderer>().color;
                    if (color.a > 100)
                    {
                        color.a *= 0.9f;
                    }
                    else
                    {
                        color.a = 0f;
                    }
                    GetComponent<SpriteRenderer>().color = color;

                }
                else
                {
                    mFinished = false;

                    if (transform.gameObject.CompareTag("Piece1"))
                    {
                        DiaryBehavior.UpdateCondition(1);
                    }
                    else if (transform.gameObject.CompareTag("Piece2"))
                    {
                        DiaryBehavior.UpdateCondition(2);
                    }
                    else if (transform.gameObject.CompareTag("Piece3"))
                    {
                        DiaryBehavior2.UpdateCondition(2);
                    }
                    else if (transform.gameObject.CompareTag("Piece4"))
                    {
                        DiaryBehavior2.UpdateCondition(1);
                    }

                    //for win behavior
                    panel.SetActive(true);
                    UnityEngine.UI.Button sample = Resources.Load<UnityEngine.UI.Button>("Prefabs/Button") as UnityEngine.UI.Button;
                    UnityEngine.UI.Button button = Instantiate(sample, text);
                    text.GetComponent<TextMeshProUGUI>().text = "And then?";
                    button.transform.localPosition += new Vector3(0, 70f, 0);
                    button.onClick.AddListener(ClickButton);
                    Time.timeScale = 0f;
                    DiaryBehavior2.End();
                    DiaryBehavior.End();

                }
            }
        }
        else
        {

            if (GetComponent<Rigidbody2D>().IsTouching(collider))
            {
                if (Input.GetKey(KeyCode.S))
                {
                    
                    if (mActive)
                    {
                        Debug.Log("Update Progress Bar: "+tmp.runtimeAnimatorController);
                        tmp.runtimeAnimatorController = c;
                        UpdateProgressBar();
                        ParticleSystem1.GetComponent<ParticleSystem>().emissionRate += 8f * Time.smoothDeltaTime;
                    }
                    else
                    {
                        Debug.Log("Activate");
                        mActive = true;
                        ProgressBar.transform.localScale = size;
                        ParticleSystem1.SetActive(true);
                        ParticleSystem1.transform.position = transform.position;
                        ParticleSystem1.GetComponent<ParticleSystem>().Play();
                        ParticleSystem2.transform.position = transform.position;
                    }
                }
                else
                {
                    tmp.runtimeAnimatorController = b;
                    Debug.Log("Interrupt");
                    mActive = false;
                    ProgressBar.transform.localScale = Vector3.zero;
                    ParticleSystem1.GetComponent<ParticleSystem>().Stop();
                }
            }
        }
    }

    void ClickButton() {
        if (transform.gameObject.CompareTag("Piece4"))
        {
            SceneManager.LoadScene(3);
            
        }else {
            SceneManager.LoadScene(2);
        }
        Time.timeScale = 1f;
    }


    private void UpdateProgressBar()
    {
        Vector3 mPosition = ProgressBar.transform.localPosition;
        float curWidth = ProgressBar.transform.localScale.x;
        x = mPosition.x - curWidth / 2;
        float percentage = curWidth / mMaxBarWidth;

        if (percentage >= 1)
        {
            mActive = false;
            mFinished = true;
            percentage = 1.0f;
            Debug.Log("Finish!");
            emission = true;
        }
        curWidth += speed * Time.smoothDeltaTime;
        percentage = curWidth / mMaxBarWidth;
     //   Debug.Log("Update Witdth: " + curWidth);
        Vector3 s = ProgressBar.transform.localScale;
        s.x = percentage * mMaxBarWidth;
        ProgressBar.transform.localScale = s;
        x = s.x + curWidth / 2;
        mPosition.x = x;
        //  ProgressBar.transform.localPosition = mPosition;
    }

    private void UpdatePieces()
    {
        Vector3 mSize = transform.localScale;
        if (mSize.x < 6.0f)
        {
            Vector3 changeSize = new Vector3(0.1f, 0.1f, 0.0f);
            mSize += changeSize;

            Vector3 mPosition = transform.localPosition;
            Vector3 cPosition = Camera.main.transform.localPosition;
            Vector3 difference = cPosition - mPosition;
            difference.z = 0.0f;
            mPosition += difference * 0.01f;


            transform.localPosition = mPosition;
            transform.localScale = mSize;
        }
    }



}
