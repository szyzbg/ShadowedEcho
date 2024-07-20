using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekoAnimation : MonoBehaviour
{
    private RuntimeAnimatorController nekoIdle,nekoRun,nekoWalk;
    private Animator a;
    private int nekoType = 0; // 0:idle, 1: walk, 2:run
    public void catRun() {
        nekoType = 2;
    }
    public void catWalk() {
        nekoType = 1;
    }
    public void catIdle() {
        nekoType = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<Animator>();
        nekoIdle = Resources.Load<RuntimeAnimatorController>("Pet Cats pack/Sprites/Cat-2/Cat-2-Idle_0");
        nekoRun = Resources.Load<RuntimeAnimatorController>("Pet Cats pack/Sprites/Cat-2/Cat-2-Run_0");
        nekoWalk = Resources.Load<RuntimeAnimatorController>("Pet Cats pack/Sprites/Cat-2/Cat-2-Walk_0");
    }

    // Update is called once per frame
    void Update()
    {
        switch (nekoType) {
            case 0 : {
                a.runtimeAnimatorController = nekoIdle;
                break;
            }
            case 1 : {
                a.runtimeAnimatorController = nekoWalk;
                break;
            }
            case 2 : {
                a.runtimeAnimatorController = nekoRun;
                break;
                }
            default: {
                a.runtimeAnimatorController = nekoIdle;
                break;
                }
        }
    }
}
