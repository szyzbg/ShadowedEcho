using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RockBehavior : MonoBehaviour
{
    private AudioClip RockDestroyed;
    public GameObject rockSound = null;
    public bool soundTmp = false;
    private GameObject smallRock = null;

    //鼠标砍碎石头
    private Vector3 lastMousePosition;
    private float mouseSpeed;

    public  float breakSpeedThreshold = 10000f; // 触发碎裂的鼠标速度阈值

    public ParticleSystem rockParticles; // Assign this in the inspector

    // Start is called before the first frame update
    void Start()
    {
        RockDestroyed = Resources.Load<AudioClip>("Sounds/劈石头");
        // 获取粒子系统的Emission模块
        ParticleSystem.EmissionModule emissionModule = rockParticles.emission;
        // 确保粒子系统一开始不会自动播放
        rockParticles.Stop();

        ParticleSystem.Burst burst = new ParticleSystem.Burst(0.0f, 20, 20); // 确保发射数量固定
        emissionModule.SetBursts(new ParticleSystem.Burst[] { burst });
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.5f) {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
        float mouseSpeed = mouseDelta.magnitude / Time.deltaTime;
        lastMousePosition = Input.mousePosition;
       // Debug.Log("Mouse speed: " + mouseSpeed);
        //transform.Rotate(Vector3.back, 2f);
        
        }
        
        

    }

    void run(float speed)
    {
        Vector3 vector = transform.localPosition;
        vector.x += speed * Time.smoothDeltaTime;
        transform.localPosition = vector;
    }


    void OnMouseOver()
    {
        if (Time.timeScale > 0.5f) {
            //Debug.Log(1);
        // 当鼠标在石块上且速度足够时碎裂

        Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
        float mouseSpeed = mouseDelta.magnitude / Time.deltaTime;
        lastMousePosition = Input.mousePosition;


        if (mouseSpeed > breakSpeedThreshold)
        {
            // 启动粒子系统发射
            rockParticles.Play();
            soundTmp = true;
            rockSound.GetComponent<AudioSource>().PlayOneShot(RockDestroyed);
            Debug.Log("Particle system started emitting.");

            // 调用销毁石块的方法，延迟时间应该足够让所有粒子发射完毕
            Invoke("DetachAndDestroy", CalculateDestructionDelay());
            MouseMove.StopTutor();
        }
        }
        

    }

    // 计算销毁延迟时间，以确保所有粒子都已经发射完毕
    float CalculateDestructionDelay()
    {
        // 检查粒子系统的Burst设置
        var burst = rockParticles.emission.GetBurst(0);
        float totalDuration = burst.cycleCount * burst.repeatInterval;
        return totalDuration;
    }

    // 分离粒子系统并销毁石块
    void DetachAndDestroy()
    {
        // 将粒子系统的父对象设置为null，以确保它不会随石块一起被销毁
        rockParticles.transform.SetParent(null);
        Debug.Log("Particle system detached.");

        // 销毁石块
        Destroy(gameObject);
        Debug.Log("Rock destroyed.");
    }
}
