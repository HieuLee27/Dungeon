using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    [Header("Button Object")]
    public Button buttonRestart;
    public Button buttonQuit;
    public Button buttonResume;
    public Button buttonOpen;

    [Space]
    [Header("Panel")]
    public GameObject panelMenu;
    public GameObject panelLoadScene;
    public Slider sliderLoad;

    public void OpenMenu()
    {
        panelMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        panelMenu.SetActive(false);
    }

    public void Restart()
    {
        string nameScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nameScene);
    }

    public void Resume()
    {
        panelMenu.SetActive(false);
    }

    public void Home()
    {
        StartCoroutine(TurnHome());
    }

    IEnumerator TurnHome()
    {
        panelLoadScene.SetActive(true);
        string nameScene = "HomeScene";

        AsyncOperation loadSceneStatus = SceneManager.LoadSceneAsync(nameScene);
        while (!loadSceneStatus.isDone)
        {
            float speed = Mathf.Clamp01(loadSceneStatus.progress / 0.8f);
            sliderLoad.value = speed;
            yield return null;
        }
    }
}
