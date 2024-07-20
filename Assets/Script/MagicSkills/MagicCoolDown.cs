using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicCoolDown : MonoBehaviour
{
    public Image image;
    public float maxCD;

    private float currentCD;
    private bool isCD = false;

    // Start is called before the first frame update
    void Start()
    {
        currentCD = maxCD;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCD)
        {
            //Debug.Log(currentCD);
            currentCD -= 1 * Time.smoothDeltaTime;
            image.fillAmount = currentCD / maxCD;
            if (currentCD <= 0.01f)
            {
                isCD = false;
                enemyBehavior.setFreezeFlag(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            UsingMagic();
            enemyBehavior.setFreezeFlag(false);
        }
    }

    void UsingMagic()
    {
        if (!isCD)
        {
            //Debug.Log("using magic!");
            isCD = true;
            currentCD = maxCD;
        }
    }
}
