using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchScene : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panelLoad;
    public GameObject panelCharacter;
    public GameObject panelShop;
    public GameObject panelDaily;
    public GameObject panelModegame;
    public GameObject panelHome;
    public GameObject panelSetting;


    [Space]
    [Header("Component on loadScene")]
    [SerializeField] private Slider slider;

    IEnumerator LoadScene()
    {
        panelLoad.SetActive(true);
        AsyncOperation loadState = SceneManager.LoadSceneAsync("PlayScene");
        while (!loadState.isDone)
        {
            float stateLoading = loadState.progress / 0.8f;
            slider.value = stateLoading;
            yield return null;
        }
    }

    public void StartGame()
    {
        StartCoroutine(LoadScene());
    }

    public void OpenShop()
    {
        panelShop.SetActive(true);
        panelCharacter.SetActive(false);
        panelModegame.SetActive(false);
    }

    public void OpenCharacter()
    {
        panelCharacter.SetActive(true);
        panelModegame.SetActive(false);
        panelShop.SetActive(false);
    }

    public void OpenModeGame()
    {
        panelModegame.SetActive(true);
        panelShop.SetActive(false);
        panelCharacter.SetActive(false);
    }

    public void OpenDaily()
    {
        panelDaily.SetActive(true);
    }

    public void CloseDaily()
    {
        panelDaily.SetActive(false);
    }

    public void OpenSetting()
    {
        panelSetting.SetActive(true);
        panelHome.SetActive(false);
    }

    public void CloseSetting()
    {
        panelHome.SetActive(true);
        panelSetting.SetActive(false);
    }
}
