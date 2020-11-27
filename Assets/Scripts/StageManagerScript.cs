using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManagerScript : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Stage1");
    }


}
