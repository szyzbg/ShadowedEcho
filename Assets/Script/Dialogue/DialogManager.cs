using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public TMPro.TMP_Text dialogText;
    //public Button closeButton;

    void Start()
    {
        // 初始化时隐藏对话框
        dialogPanel.SetActive(false);

        // 为关闭按钮添加监听事件
        //closeButton.onClick.AddListener(CloseDialog);
    }

    public void ShowDialog(string message)
    {
        dialogPanel.SetActive(true);
        dialogText.text = message;
    }

    public void CloseDialog()
    {
        dialogPanel.SetActive(false);
    }
}
