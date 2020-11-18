using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : PlayerScript
{
    [SerializeField] private string[] moves = null;
    [SerializeField] private HintPlacer hint = null;
    private int moveIndex = 0;
    protected override void InitTurn()
    {
        base.InitTurn();
        if(moves.Length > 0)
        {
            tileSelector.SetDir(moves[moveIndex]);
            moveIndex += 1;
            moveIndex %= moves.Length;
        } else
        {
            tileSelector.enabled = false;
            OnArrival();
        }
    }

    void Update()
    {
        if (isMyTurn)
        {
            if (tileSelector.enabled && tileSelector.SelectedTile != null)
            {
                Vector3 lookAtPos = tileSelector.SelectedTile.position;
                lookAtPos.y = transform.position.y;
                transform.LookAt(lookAtPos);
                hint.enabled = false;
                mover.Move(tileSelector.SelectedTile.position, OnArrival);
                animator.SetTrigger("Walk");
                tileSelector.enabled = false;
            }
        }
    }

    protected override void OnArrival()
    {
        hint.enabled = true;
        base.OnArrival();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            mover.Stop();
            //GetComponent<BoxCollider>().enabled = false;
            //GetComponent<Rigidbody>().isKinematic = true;
            
            if (isMyTurn)
            {
                Vector3 lookAtPos = other.transform.position;
                lookAtPos.y = transform.position.y;
                transform.LookAt(lookAtPos);
                animator.SetTrigger("Attack");
            }
            else if(Vector3.Angle(transform.forward, other.transform.forward) < 120)
            {
                OnDeath();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        return;
    }
}
