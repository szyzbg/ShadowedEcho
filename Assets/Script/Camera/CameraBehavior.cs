using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.UI;

public class CameraBehavior : MonoBehaviour
{
    public GameObject hero = null; // set in unity
    public GameObject initial = null;
    public GameObject camer = null; // set in unity
    private bool shouldMoveCamera = false;
    private float moveTimer = 0f;
    private float moveDuration = 2f; // 移动持续时间
    public AddRigidbodyToTilemap add1;

    public bool knownCollapsing = false;

    public TMP_Text tip1;
    public TMP_Text tip2;

    // Start is called before the first frame update
    void Start()
    {
        CameraTools.elapsedTime = CameraTools.shakeDuration;
        CameraTools.cPosition = camer.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (add1 != null)
        {
            Vector3 currentTilePosition = add1.currentTileWorldPosition;
            float currentTileWorldX = currentTilePosition.x;

            if (currentTileWorldX < -35)
            {
                if (initial != null)
                    CameraTools.CameraFocus(camer, initial, 0, 3);

                // if (!knownCollapsing)
                // {
                //     Debug.Log("ShowTip");
                //     Invoke("showTip1", 2);
                //     Invoke("showTip2", 3);
                //     Invoke("hideTip1", 5);
                //     Invoke("hideTip2", 6);
                // }

                knownCollapsing = true;
            }
            else
            {
                if (hero != null)
                    CameraTools.CameraFocus(camer, hero, 0, -2);
            }
        }
    }



    void Scale(GameObject camer, float scaleTo)
    {
        CameraTools.CameraScale(camer, scaleTo);
        if (camer.GetComponent<Camera>().orthographicSize >= scaleTo)
        {
            CameraTools.initScaleTmp();
        }
    }


    void showTip1()
    {
        if (tip1 != null)
        {
            //将tip1的透明度先设为0
            tip1.CrossFadeAlpha(0, 0, false);
            tip1.enabled = true;
            //逐渐显示
            tip1.CrossFadeAlpha(1, 2, false);
        }

    }

    //让Tip1消失
    void hideTip1()
    {
        if (tip1 != null)
            tip1.CrossFadeAlpha(0, 2, false);
    }

    //让Tip2显示
    void showTip2()
    {
        if (tip2 != null)
        {
            //将tip2的透明度先设为0
            tip2.CrossFadeAlpha(0, 0, false);
            tip2.enabled = true;
            //逐渐显示
            tip2.CrossFadeAlpha(1, 2, false);
        }

    }

    //让Tip2消失
    void hideTip2()
    {
        if (tip2 != null)
            tip2.CrossFadeAlpha(0, 2, false);
    }

}
