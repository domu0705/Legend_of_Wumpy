using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] turnReceiverGameObjects = null;
    [SerializeField] private Text turnIndicator = null;
    [SerializeField] private GameObject player = null;
    [SerializeField] private int currentStage = 1;
    [SerializeField] private GameObject stageClearUI = null;
    private List<ITurnReceiver> turnReceivers = new List<ITurnReceiver>();
    private int currentIndex = 0;
    private bool stageClearUIOn = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < turnReceiverGameObjects.Length; i++)
        {
            turnReceivers.Add(turnReceiverGameObjects[i].GetComponent<ITurnReceiver>());
        }
        stageClearUI.SetActive(false);
        GiveTurn();
    }

    void Update()
    {
        if (!stageClearUIOn)
        {
            checkClearCondition();
        }
    }

    private void GiveTurn()
    {
        turnIndicator.text = turnReceivers[currentIndex].GetCharacterName() + " turn";
        turnReceivers[currentIndex].ReceiveTurn(OnEndTurn);
        
    }

    private void OnEndTurn()
    {
        currentIndex += 1;
        currentIndex %= turnReceivers.Count;
        GiveTurn();
    }

    public void StageClear()
    {
        if (currentStage == 3)
        {
            SceneManager.LoadScene("GameClear");
        }
        SceneManager.LoadScene("Stage"+(currentStage+1).ToString());
    }

    public void RestartStage()
    {
        SceneManager.LoadScene("Stage"+ currentStage.ToString());
    }

    public void checkClearCondition()
    {
        if (currentStage == 1 || currentStage == 2)
        {
            if (player.GetComponent<PlayerScript>().Gold == 100)
            {
                stageClearUI.SetActive(true);
            }
        } else if (currentStage == 3)
        {
            if (turnReceivers.Count == 1 && turnReceivers[0].GetCharacterName() == "player") ;
            {
                StageClear();
            }
        }
    }
}
