using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactForce : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody body;
    public Rigidbody soccerBall; // Assign the Rigidbody of the soccer ball
    public bioController footVelocityCalculator; // Assign the script that calculates the foot speed
    private Vector3 lastVelocity;
    private float impactForce;
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        Vector3 currentVelocity = soccerBall.velocity;
        Vector3 velocityChange = currentVelocity - lastVelocity;
        impactForce = soccerBall.mass * velocityChange.magnitude / Time.fixedDeltaTime;
        lastVelocity = currentVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == footVelocityCalculator.footTransform)
        {
            Debug.Log("Impact Force: " + impactForce + " Newtons");
        }
    }
}
