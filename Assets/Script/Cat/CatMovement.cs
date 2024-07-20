using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public DialogManager dl;
    private int diaCount = 0;
    private bool isCat1DiaShown = false;
    public NekoAnimation nekoAnimation;
    public Transform[] waypoints; // 保存路径点的数组
    public float speed = 2f; // 移动速度

    public GameObject circle; // Circle GameObject的引用


    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0)
            return;

        // 移动到当前路径点
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // 检查是否到达路径点
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // 切换到下一个路径点
            currentWaypointIndex++;
            // if (currentWaypointIndex >= waypoints.Length)
            // {
            //     currentWaypointIndex = 0; // 循环路径
            // }
        }

        if (Input.GetMouseButtonDown(0) && Time.timeScale == 0 && !isCat1DiaShown && circle.transform.position.x >= -15f)
        {
            diaCount++;
            if (diaCount == 2)
            {
                isCat1DiaShown = true;
                Time.timeScale = 1;
                dl.CloseDialog();
            }
        }

        // 检查circle的x坐标是否大于等于-15
        if (circle.transform.position.x >= -15f)
        {
            if (!isCat1DiaShown)
            {
                if (diaCount == 0)
                {
                    dl.ShowDialog(" A black cat? ");
                    Time.timeScale = 0;
                }
                if (diaCount == 1)
                {
                    dl.ShowDialog("It is the only life in this odd place, I think I need to catch it for company.");
                }
            }

            // 计算小猫和circle之间的x坐标差
            float xDifference = Mathf.Abs(circle.transform.position.x - transform.position.x);

            // 检查小猫的x坐标是否大于259
            if (transform.position.x > 259f)
            {
                speed = 0;
                nekoAnimation.catIdle();
            }
            else if (xDifference > 5f)
            {
                // 如果x坐标差大于5并且小猫没有在跳跃，让小猫停下来
                speed = 0;
                nekoAnimation.catIdle();
            }
            else if (xDifference <= 5f)
            {
                // 恢复小猫的速度
                speed = 5;
                nekoAnimation.catRun();
            }
        }
        else
        {
            // 保持小猫的速度为0
            speed = 0;
            nekoAnimation.catIdle();
        }
    }
}
