using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelChange1 : MonoBehaviour
{
    public GameObject panelToOpen;
    public GameObject panelToClose;
    public GameObject panelToClose1;


    public void SwitchPanel()
    {
        panelToClose.SetActive(false);
        panelToClose1.SetActive(false);
        panelToOpen.SetActive(true);
    }
}
