using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayController : MonoBehaviour
{
    public GameObject specialPiece;
    public void Replay()
    {
        if (specialPiece.CompareTag("Piece4"))
        {
            SceneManager.LoadScene(2);
        }else
        {
            SceneManager.LoadScene(1);
        }
    }
}
