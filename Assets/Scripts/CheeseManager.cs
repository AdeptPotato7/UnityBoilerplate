using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseManager : MonoBehaviour
{
    static int cheese = 0;
    public int cheeseToWin = 18;
    void Start()
    {
        cheese = 0;
    }

    public int getCheese() { return cheese; }

    public void addCheese(int amount)
    { 
        cheese += amount;
        if (cheese >= cheeseToWin)
        {
            WinGame();
        }
    }

   void WinGame()
   { 
               UnityEngine.SceneManagement.SceneManager.LoadScene("StartScreen");   
   }
}
