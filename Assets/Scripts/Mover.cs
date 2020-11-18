using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public delegate void OnArrival();

    [SerializeField] private float speed = 0;
    private Vector3 destination;
    private bool move = false;
    private OnArrival onArrival = null;

    // Update is called once per frame
    void Update()
    {
        if(move)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, destination, step);
            if (transform.position == destination)
            {
                move = false;
                onArrival();
            }
        }
    }

    public void Move(Vector3 destination, OnArrival onArrival)
    {
        this.destination = destination;
        this.destination.y = transform.position.y;
        move = true;
        this.onArrival = onArrival;
    }

    public void Stop()
    {
        move = false;
    }
}
