using UnityEngine;

public class SwitchPanel : MonoBehaviour
{
    public GameObject PanelOn;
    public GameObject PanelOff;
    public void Switch()
    {
        PanelOff.SetActive(false);
        PanelOn.SetActive(true);
    }
}
