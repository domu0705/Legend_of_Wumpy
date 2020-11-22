using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Gold, Heart };
    public Type type;
    public int value;

    // Update is called once per frame
    void Start()
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        
    }
    void Update()
    {
        Item item = gameObject.GetComponent<Item>();
        switch (item.type)
        {
            case Item.Type.Heart:
                transform.Rotate(Vector3.up * 20 * Time.deltaTime);
                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<Renderer>().enabled = true;
            GetComponentInChildren<ParticleSystem>().Play();    
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<Renderer>().enabled = false;
            GetComponentInChildren<ParticleSystem>().Stop();
        }
    }
}
