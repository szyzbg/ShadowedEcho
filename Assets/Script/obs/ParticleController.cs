using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public Redgreen spriteMonitor; // 引用Redgreen脚本的GameObject
    public ParticleSystem particleSystem; // 引用的粒子系统

    void Start()
    {
        if (particleSystem == null)
        {
            Debug.LogError("ParticleSystem component is missing.");
            return;
        }

        if (spriteMonitor == null)
        {
            Debug.LogError("SpriteMonitor (Redgreen) is not assigned.");
            return;
        }

        // 订阅Redgreen脚本的事件
        spriteMonitor.OnSpriteSwitch += ControlParticlesBasedOnSprite;
    }

    void OnDestroy()
    {
        // 取消订阅事件以防止内存泄漏
        if (spriteMonitor != null)
        {
            spriteMonitor.OnSpriteSwitch -= ControlParticlesBasedOnSprite;
        }
    }

    private void ControlParticlesBasedOnSprite(string spriteName)
    {
        if (spriteName == "red")
        {
            particleSystem.Stop(); // 停止粒子效果
            particleSystem.Clear(); // 清除粒子
        }
        else if (spriteName == "green")
        {
            particleSystem.Play(); // 启动粒子效果
        }
    }
}
