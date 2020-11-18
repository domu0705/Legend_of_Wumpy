using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] turnReceiverGameObjects = null;
    private List<ITurnReceiver> turnReceivers = new List<ITurnReceiver>();
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < turnReceiverGameObjects.Length; i++)
        {
            turnReceivers.Add(turnReceiverGameObjects[i].GetComponent<ITurnReceiver>());
        }
        GiveTurn();
    }

    private void GiveTurn()
    {
        turnReceivers[currentIndex].ReceiveTurn(OnEndTurn);
    }

    private void OnEndTurn()
    {
        currentIndex += 1;
        currentIndex %= turnReceivers.Count;
        GiveTurn();
    }
}
