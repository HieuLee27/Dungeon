using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    public void ReturnHome()
    {
        SceneManager.LoadSceneAsync("HomeScene");
    }
}
