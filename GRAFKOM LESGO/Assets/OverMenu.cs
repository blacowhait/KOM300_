using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverMenu : MonoBehaviour
{
    
    public void ExitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level1");
    }
}
