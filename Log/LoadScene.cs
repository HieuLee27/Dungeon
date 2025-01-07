using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public Slider loadingState;
    public float timeChangePanel;
    public static LoadScene Instance;

    private void Awake()
    {
        CreateLoadScene();
    }

    private void CreateLoadScene()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    IEnumerator UpdateLoading()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);

        while (!asyncOperation.isDone)
        {
            float speedLoading = Mathf.Clamp01(asyncOperation.progress / 0.5f);

            loadingState.value = speedLoading;
            yield return null;
        }
        
    }

    public void ContinueLogin()
    {
        StartCoroutine(UpdateLoading());
    }
}
