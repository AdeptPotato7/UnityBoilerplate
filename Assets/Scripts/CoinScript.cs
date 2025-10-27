using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private PlatformManager manager;

    void Start() {
        manager = GameObject.FindObjectOfType<PlatformManager>();
    }

    void OnTriggerEnter(Collider other) {
        manager.collectCoin();
        Destroy(this.gameObject);
    }
}
