using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nature : MonoBehaviour
{
    public GameObject player;
    PlayerScript playerScript;

    void Start()
    {
        playerScript = player.GetComponent<PlayerScript>();
    }
     void Update()
    {
        if (playerScript.worldCollision)
        {
            showWorld();
        }
    }

    void showWorld()
    {
        GetComponent<Renderer>().enabled = true;
        //playerScript.worldCollision = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<Renderer>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<Renderer>().enabled = false;
        }
    }
}
