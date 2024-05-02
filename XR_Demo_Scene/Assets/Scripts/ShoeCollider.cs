using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoeCollider : MonoBehaviour
{
    public GameObject Shoes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Shoes.GetComponent<KickBall>().isSideKicking = true;
        }
    }

}
