using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickBall : MonoBehaviour
{
    private Vector3 velocity;
    private Vector3 previous;
    public bool isSideKicking;
    public bool kciked;
    public GameObject[] colldiers;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void FixedUpdate()
    {
        velocity = ((transform.position - previous)) / Time.fixedDeltaTime;
        previous = transform.position;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            kciked = true;
            //Debug.Log("Ball Detected");
            /*            Debug.Log("Ball Detected");
                        collision.gameObject.GetComponent<Rigidbody>().AddForce(velocity.x * 50, velocity.magnitude * 20, velocity.z * 50);
                        if (isLaceKicking)
                        {
                            Debug.Log("Lace Kick");
                            collision.gameObject.GetComponent<Rigidbody>().AddForce(velocity.x * 50, velocity.magnitude * 20, velocity.z * 50);
                        }
                        if(isSideKicking)
                        {
                            Debug.Log("Side Kick");
                            collision.gameObject.GetComponent<Rigidbody>().AddForce(velocity.x * 30, 0, velocity.z * 30);
                        }*/

            if (isSideKicking)
            {
                //Debug.Log("Side");
                collision.gameObject.GetComponent<Rigidbody>().AddForce(velocity.x * 40, 0, velocity.z * 40);
            }
            else
            {
                //Debug.Log("Normal");
                collision.gameObject.GetComponent<Rigidbody>().AddForce(velocity.x * 70, velocity.magnitude * 20, velocity.z * 70);
            }
        }
    }



}
