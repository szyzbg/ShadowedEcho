using UnityEngine;
using System.Collections;

public class AppearCube : MonoBehaviour
{
    public GameObject[] objectsToActivate; // 存储要激活的GameObjects的数组
    public float activationDelay = 0.5f; // 每个GameObject之间的延迟时间
    public GameObject player; // 玩家对象
    public float activationThreshold = 6.0f; // 阈值，当GameObject与玩家x坐标相差小于此值时激活对象
    public float moveDuration = 1.0f; // 移动到目标位置的时间

    private bool[] hasActivated; // 标记每个GameObject是否已经激活

    void Start()
    {
        hasActivated = new bool[objectsToActivate.Length];
    }

    void Update()
    {
        // 遍历数组中的每个GameObject
        for (int i = 0; i < objectsToActivate.Length; i++)
        {
            GameObject obj = objectsToActivate[i];

            if (!hasActivated[i] && Mathf.Abs(obj.transform.position.x - player.transform.position.x) < activationThreshold)
            {
                hasActivated[i] = true;
                StartCoroutine(ActivateAndMoveObject(obj));
            }
        }
    }

    IEnumerator ActivateAndMoveObject(GameObject obj)
    {
        Vector3 targetPosition = obj.transform.position; // 目标位置
        obj.transform.position = new Vector3(targetPosition.x, targetPosition.y - 0.5f, targetPosition.z); // 设置初始位置在目标位置下方
        obj.SetActive(true); // 激活当前的GameObject

        // 移动到目标位置
        float elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, targetPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终位置精确
        obj.transform.position = targetPosition;

        // 等待一段时间
        yield return new WaitForSeconds(activationDelay);
    }
}
