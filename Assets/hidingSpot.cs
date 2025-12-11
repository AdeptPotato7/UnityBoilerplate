using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hidingSpot : MonoBehaviour
{
    private static GameObject[] enemies;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
                enemy.GetComponent<EnemyPathing>().playerHiding = true;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
                enemy.GetComponent<EnemyPathing>().playerHiding = false;
        }
    }
}
