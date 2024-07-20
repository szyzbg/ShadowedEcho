using UnityEngine;
using UnityEngine.UI;

public class DialogManager3 : MonoBehaviour
{
    public GameObject dialogPanel;
    public GameObject heroIm;
    public GameObject inviIm;
    public TMPro.TMP_Text dialogText;
    //public Button closeButton;

    void Start()
    {
        // 初始化时隐藏对话框
        dialogPanel.SetActive(false);

        // 为关闭按钮添加监听事件
        //closeButton.onClick.AddListener(CloseDialog);
    }

    public void ShowDialogHero(string message)
    {
        inviIm.SetActive(false);
        heroIm.SetActive(true);
        dialogPanel.SetActive(true);
        dialogText.text = message;
    }
    public void ShowDialogInvi(string message)
    {
        inviIm.SetActive(true);
        heroIm.SetActive(false);
        dialogPanel.SetActive(true);
        dialogText.text = message;
    }

    public void CloseDialog()
    {
        dialogPanel.SetActive(false);
    }

}
