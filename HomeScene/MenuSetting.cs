using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSetting : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panelSound;
    public GameObject panelSupport;

    public void OpenPanelSound()
    {
        panelSupport.SetActive(false);
        panelSound.SetActive(true);
    }

    public void OpenPanelSupport()
    {
        panelSound.SetActive(false);
        panelSupport.SetActive(true);
    }
}
