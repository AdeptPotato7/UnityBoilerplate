using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseManager : MonoBehaviour
{
    static int cheese = 0;

    void Start()
    {
        cheese = 0;
    }

    public int getCheese() { return cheese; }

    public void addCheese(int amount) { cheese += amount; }
}
