using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Cheese : MonoBehaviour
{
    GameObject manager;

    private void Start()
    {
        manager = GameObject.Find("CheeseManager");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.GetComponent<CheeseManager>().addCheese(1);
            Destroy(gameObject);
        }
    }
}
