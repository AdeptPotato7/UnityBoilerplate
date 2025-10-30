using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
   public void Play()
   {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DaGame");
   }

    public void Quit()
    {
        Application.Quit();
    }

   void Update()
   {
        Time.timeScale = 1;
   }
}
