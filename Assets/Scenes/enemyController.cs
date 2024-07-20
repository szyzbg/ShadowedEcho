using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System;

public class enemyController : MonoBehaviour
{
    private int dialogCount = 0;
    private bool isOutBlack = false;
    private bool enemyGenFin = false;
    private bool inDia = false;
    public DialogManager3 diaM;
    private AudioClip pound;
    public GameObject player;
    public bool hasEnemy = false;
    public Vignette vignette;
    public GameObject enemy;
    public PostProcessVolume volume;
    public float timer = 0;
    public float poundingTimer = 0;
    public bool isPounding = false;
    public void changeToDiaCount() {
        dialogCount = 5;
    }
    public void addOneToDiaCount() {
        dialogCount++;
    }
    // Start is called before the first frame update
    void Start()
    {
        pound = Resources.Load<AudioClip>("Sounds/hearbeat_once");
        // 尝试从后处理卷中获取Vignette组件
        if (volume.profile.TryGetSettings(out vignette))
        {
            Debug.Log("Vignette component found!");
        }
        else
        {
            Debug.LogWarning("Vignette component not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && dialogCount < 5 && isOutBlack )
        {
            if (dialogCount < 4) {
                dialogCount++;
            }
            else {
                dialogCount = 6;
            }
        }
        transform.position = new Vector3(transform.position.x, 0.48f, 0);

        //Debug.Log(hasEnemy);
        //玩家的横坐标大于等于50的时候加载敌人
        if (player.transform.position.x >= 50 && (!hasEnemy || enemyGenFin))
        {
            isOutBlack = true;
            if (!hasEnemy && !enemyGenFin)
            {
                enemy = Instantiate(Resources.Load("Prefabs/enemyPrefab")) as GameObject;
            }
            enemyGenFin = true;
            //加载敌人

            //设置敌人的位置
            switch (dialogCount)
            {
                case 0:
                    {
                        inDia = true;
                        Time.timeScale = 0;
                        diaM.ShowDialogHero("Who are you? Why do we look the same?");
                        break;
                    }
                case 1:
                    {
                        diaM.ShowDialogInvi("Stop here. Don't go foward!");
                        break;
                    }
                case 2:
                    {
                        diaM.ShowDialogHero("Are you kidding? Stop means die!");
                        break;
                    }
                case 3:
                    {
                        diaM.ShowDialogInvi("I say, STOP!!!");
                        break;
                    }
                case 4:
                    {
                        diaM.ShowDialogHero("Hey!");
                        break;
                    }
                case 5:
                    {
                        //Time.timeScale = 0;
                        //diaM.ShowDialogHero("It seems that the invisible man disappears.");
                        break;
                    }
                case 6:
                    {
                        //Debug.Log("alws clsing");
                        diaM.CloseDialog();
                        Time.timeScale = 1;
                        dialogCount++;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            if (!hasEnemy)
            {
                Debug.Log("加载敌人");
                hasEnemy = true;
            }


        }

        if (hasEnemy)
        {
            vignette.intensity.value = Math.Max(0, 0.5f - (Vector3.Distance(enemy.transform.position, player.transform.position)) / 50);
            Debug.Log("value"+vignette.intensity.value);
            timer += Time.deltaTime;
            if (timer >= 1 || isPounding)
            {
                isPounding = true;
                heartPounding();
                timer = 0;
            }
        }
        else
        {
            vignette.intensity.value = 0;
        }
    }

    void heartPounding()
    {
        //Debug.Log("timer " + poundingTimer);
        //屏幕黑框抖动
        vignette.intensity.value = vignette.intensity.value + 0.03f * (poundingTimer / 0.1f);
        poundingTimer += Time.deltaTime;
        if (poundingTimer >= 0.1f)
        {
            GetComponent<AudioSource>().PlayOneShot(pound);
            isPounding = !isPounding;
            poundingTimer = 0;
        }
    }

    public void hitByEnemy(){
        //扣血
        //外围变成红色
        vignette.color.value=Color.red;
        StartCoroutine(ChangeVignetteColor());
    }

    IEnumerator ChangeVignetteColor(){
        float time=0;
        while(time<1){
            time+=Time.deltaTime;
            vignette.color.value=Color.Lerp(Color.red,Color.black,time);
            yield return null;
        }
        vignette.color.value=Color.black;
    }
}