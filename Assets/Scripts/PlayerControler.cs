using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody rb;
    private bool onFloor = false;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        
    }

    void fixedUpdate() {

    }

    void OnCollisionStay(Collision other) {
        if (other.gameObject.CompareTag("Floor"))
            onFloor = true;
    }

    void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Floor"))
            onFloor = false;
    }

    // Update is called once per frame
    void Update() {
        Vector3 vel = rb.velocity;
        if (Input.GetKey(KeyCode.D)) 
            rb.velocity = new Vector3(3f, vel.y, vel.z);
        if (Input.GetKey(KeyCode.A)) 
            rb.velocity = new Vector3(-3f, vel.y, vel.z);
        if (Input.GetKeyDown(KeyCode.Space) && onFloor)
            rb.velocity = new Vector3(vel.x, 12f, vel.z);
    }

    
}
