using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    GameManager gameManager;
    public Image company, title;
    private bool screenSwitch;
    float remainingTime, colorMultiplier = 1;

    private void Start()
    {
        screenSwitch = true;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        remainingTime = 1.5f;
        StartCoroutine(FadeScreen(company));
    }

    private void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0 && screenSwitch)
        {
            company.gameObject.SetActive(false);
            remainingTime = 1.5f;
            StartCoroutine(FadeScreen(title));
            screenSwitch = false;
        }

        if (remainingTime <= 0 && !screenSwitch)
        {
            title.gameObject.SetActive(false);
            Color setOriginalAlpha = title.material.color;
            setOriginalAlpha.a = 1.0f;
            title.material.color = setOriginalAlpha;
            LoadMenu();
        }
    }

    IEnumerator FadeScreen(Image screen)
    {
        screen.gameObject.SetActive(true);
        Color fadeAmmount = screen.material.color;
        remainingTime -= Time.deltaTime;
        while (remainingTime >= 0)
        {
            fadeAmmount.a = remainingTime * colorMultiplier;
            screen.material.color = fadeAmmount;
            yield return null;
        }
    }

    void LoadMenu()
    {
        gameManager.Menu();
    }
}