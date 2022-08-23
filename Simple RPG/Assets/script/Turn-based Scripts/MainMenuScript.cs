using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void EnterBattle()
    {
        SceneManager.LoadScene("WSOA2023 - 2021 - CCF Base");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
