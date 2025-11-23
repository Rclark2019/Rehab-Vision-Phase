using UnityEngine;

public class HelpPopup : MonoBehaviour
{
    public GameObject panel;

    public void Toggle()
    {
        if (!panel) return;
        panel.SetActive(!panel.activeSelf);
    }

    public void Show()
    {
        if (panel) panel.SetActive(true);
    }

    public void Hide()
    {
        if (panel) panel.SetActive(false);
    }
}