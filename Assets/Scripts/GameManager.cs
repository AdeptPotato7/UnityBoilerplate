using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get; private set;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Play()
    {
        SceneManager.LoadScene("Ryans1");
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
    public void GameOver()
    {
        SceneManager.LoadScene("GameOverScreen");
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
