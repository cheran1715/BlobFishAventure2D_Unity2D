using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverMenu : MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1f; // Ensure time is normal when restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home()
    {
        Time.timeScale = 1f; // Ensure time is normal when going to home
        SceneManager.LoadScene("MainMenu");
    }
}
