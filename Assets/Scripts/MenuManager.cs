using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
  
   public void Play()
   {
        SceneManager.LoadScene("Ryans1");
     
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