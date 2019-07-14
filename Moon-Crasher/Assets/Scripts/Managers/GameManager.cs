using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameObject instance;
    public static int score;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this.gameObject;
            score = 0;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        if (score < 0)
        {
            score = 0;
        }
    }
    public void Menu()
    {
        SceneManager.LoadScene(1);
    }

    public void Game()
    {
        SceneManager.LoadScene(2);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(3);
    }

    public void Splash()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}