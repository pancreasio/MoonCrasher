using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    internal void Menu()
    {
        SceneManager.LoadScene(1);
    }

    internal void Game()
    {
        SceneManager.LoadScene(2);
    }

    internal void GameOver()
    {
        SceneManager.LoadScene(3);
    }

    internal void Splash()
    {
        SceneManager.LoadScene(0);
    }
}