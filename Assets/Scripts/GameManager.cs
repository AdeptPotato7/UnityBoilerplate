using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Play()
    {
        SceneManager.LoadScene("DaGame");
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public bool gamePaused = false;
    [SerializeField] GameObject pauseMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gamePaused == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            gamePaused = true;
            pauseMenu.SetActive(true);

        }
        else if ((Input.GetKeyDown(KeyCode.Escape) && gamePaused == true))
        {
            Time.timeScale = 1;
            gamePaused = false;
            pauseMenu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
