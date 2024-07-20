using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MouseMove : MonoBehaviour
{
    public GameObject mouse = null;
    public GameObject line = null;
    public GameObject hero = null;
    public Rigidbody2D begin = null;

    private bool start = false;
    private static bool over = false;
    private Vector3 oriPosition;
    private float speed = 0.002f;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(mouse != null);
        Debug.Assert(line != null);
        Debug.Assert(hero != null);
        mouse.SetActive(false);
        line.SetActive(false);
        oriPosition = mouse.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.5f) {
            if (!start && !over && begin.IsTouching(hero.GetComponent<BoxCollider2D>()))
        {
            start = true;
            mouse.SetActive(true);
            line.SetActive(true);
        }
        if (over) {
            mouse.SetActive(false);
            line.SetActive(false);
        }
        UpdateMouse();
        }
        
    }

    void UpdateMouse()
    {
        if (start && !over)
        {
            if (mouse.transform.localPosition.x < oriPosition.x + line.transform.localScale.x)
            {
                Vector3 vector = mouse.transform.localPosition;
                vector.x += speed;
                mouse.transform.localPosition = vector;
            }
            else
            {
                Debug.Log("again");
                mouse.transform.localPosition = oriPosition;
            }
            

        }
    }

    public static void StopTutor()
    {
        over = true;
    }
}
