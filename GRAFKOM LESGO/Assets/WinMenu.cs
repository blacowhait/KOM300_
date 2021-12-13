using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    
    public void RestartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
