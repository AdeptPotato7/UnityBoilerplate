using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    Ray ray;
    RaycastHit Hit;
    float rayDistance = 10f;
    public GameObject player;
    private Rigidbody rb;
    public float speed = 0.2f;
    public int rayCount = 20;
    public float coneAngle = 30f;
    public bool drawDebug = true;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        DetectPlayer();
    }
   
     void DetectPlayer()
    {
        var direction = (player.transform.position - transform.position).normalized;

        // Only use cone raycast
        bool playerDetected = ConeRaycast(transform.forward);

        // Move towards player if detected
        if (playerDetected)
        {
            rb.velocity = direction * speed;
        }
    }

    bool ConeRaycast(Vector3 forward)
    {
        for (int i = 0; i < rayCount; i++)
        {
            float angle = (i / (float)rayCount) * 360f;
            Vector3 rayDirection = GetConicalDirection(forward, angle, coneAngle);

            if (drawDebug)
            {
                Debug.DrawRay(transform.position, rayDirection * rayDistance, Color.yellow);
            }

            if (Physics.Raycast(transform.position, rayDirection, out Hit, rayDistance))
            {
                if (Hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    Vector3 GetConicalDirection(Vector3 forward, float angle, float coneAngle)
    {
        // Create a rotation around the forward vector
        Quaternion rotation = Quaternion.AngleAxis(angle, forward);

        // Get perpendicular vector to forward
        Vector3 right = Vector3.Cross(forward, Vector3.up);
        if (right.magnitude < 0.001f)
            right = Vector3.Cross(forward, Vector3.forward);

        // Tilt by cone angle
        Quaternion tilt = Quaternion.AngleAxis(coneAngle, right);
        Vector3 direction = tilt * forward;

        // Apply rotation around forward
        return rotation * direction;
    }
}
