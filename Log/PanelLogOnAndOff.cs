using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelLogOnAndOff : MonoBehaviour
{
    public GameObject panelLog, panelLoad;
    private Animator animator;
    public static PanelLogOnAndOff Instance;

    private void Awake()
    {
        CreatePanelOO();
        animator = panelLog.GetComponent<Animator>();
    }

    private void CreatePanelOO()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void OpenPanel()
    {
        animator.SetBool("TurnPanel", true);
        panelLoad.SetActive(false);
    }

    public void ClosePanel()
    {
        StartCoroutine(WaitToOpen());
        animator.SetTrigger("Off");
        animator.SetBool("TurnPanel", false);
    }

    private IEnumerator WaitToOpen()
    {
        yield return new WaitForSeconds(1.5f);
        panelLoad.SetActive(true);
    }
}
