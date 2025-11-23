using UnityEngine;
using TMPro;

public class TutorialOverlayController : MonoBehaviour
{
    [Header("References")]
    public TMP_Text body;
    public GameObject root;

    [Header("Behavior")]
    public bool alwaysShowEveryRun = true; 

    [Header("Tutorial Pages (Optional Override)")]
    [TextArea(3, 6)]
    public string[] customPages;

    readonly string[] defaultPages = {
        "Track Your Performance\n   Watch the metrics panel for accuracy",
        "2) Follow the Avatar\n   Mimic the movements you see",
        "3) Monitor Progress\n   The graph shows improvement",
        "4) Control Your Session\n   Use Start / Pause / Save",
        "5) Start Your Session\n   Switch from Idle to begin!"

    };

    int step = 0;
    string[] Pages => (customPages != null && customPages.Length > 0) ? customPages : defaultPages;

    void Start()
    {
        if (!root || !body)
        {
            Debug.LogWarning("[TutorialOverlay] Missing UI references.");
            return;
        }

        if (!alwaysShowEveryRun && PlayerPrefs.GetInt("RV_TutorialDone", 0) == 1)
        {
            root.SetActive(false);
            return;
        }

        root.SetActive(true);
        step = 0;
        body.text = Pages[0];
    }

    public void Next()
    {
        step++;
        if (step >= Pages.Length)
        {
            FinishTutorial();
            return;
        }
        body.text = Pages[step];
    }

    public void Skip()
    {
        FinishTutorial();
    }

    void FinishTutorial()
    {
        if (!alwaysShowEveryRun)
        {
            PlayerPrefs.SetInt("RV_TutorialDone", 1);
            PlayerPrefs.Save();
        }
        if (root) root.SetActive(false);
    }

    // Helper if you ever want to reset from a button
    public static void ResetTutorialProgress()
    {
        PlayerPrefs.DeleteKey("RV_TutorialDone");
        PlayerPrefs.Save();
    }
}
