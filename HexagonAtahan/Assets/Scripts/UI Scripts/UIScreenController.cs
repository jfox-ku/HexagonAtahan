using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenController : MonoBehaviour
{
    public Transform GameOverScreen;

    public void ShowGameOver() {
        GameOverScreen.gameObject.SetActive(true);
    }


    public void HideGameOver() {
        GameOverScreen.gameObject.SetActive(false);
    }
}
