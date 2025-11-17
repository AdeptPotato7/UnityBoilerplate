using System.Collections;
using UnityEngine;
using TMPro;
public class CheeseManager : MonoBehaviour
{
    public static CheeseManager instance;
    public TextMeshProUGUI cheeseText;
    static int cheese = 0;
    public int cheeseToWin = 18;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        cheese = 0;
        UpdateCheeseDisplay();
    }

    public int getCheese() { return cheese; }

    public void addCheese(int amount)
    { 
        cheese += amount;
        UpdateCheeseDisplay();

        if (cheese >= cheeseToWin)
        {
            WinGame();
        }
    }

    void UpdateCheeseDisplay()
    {
        cheeseText.text = "Cheese: " + cheese + "/" + cheeseToWin;
    }
   void WinGame()
   { 
               UnityEngine.SceneManagement.SceneManager.LoadScene("StartScreen");   
   }

    public int GetCheeseCount()
    {
        return cheese;
    }
}
