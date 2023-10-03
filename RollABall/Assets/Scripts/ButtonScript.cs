using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void retryButton()
    {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1f;
        CharacterMove.GameIsPaused = false;
    }

    public void exitButton()
    {
        Application.Quit();
    }
}
