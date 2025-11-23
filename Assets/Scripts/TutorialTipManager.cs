using UnityEngine;
using TMPro;

public class TutorialTipManager : MonoBehaviour
{
    public GameObject root;       // TipOverlay
    public TMP_Text titleText;    // TipTitle
    public TMP_Text bodyText;     // TipBody

    const string PREF_ACCURACY_TIP = "RV_SHOW_ACCURACY_TIP";
    const string PREF_FATIGUE_TIP = "RV_SHOW_FATIGUE_TIP";

    void Awake()
    {
        if (!PlayerPrefs.HasKey(PREF_ACCURACY_TIP))
            PlayerPrefs.SetInt(PREF_ACCURACY_TIP, 1);
        if (!PlayerPrefs.HasKey(PREF_FATIGUE_TIP))
            PlayerPrefs.SetInt(PREF_FATIGUE_TIP, 1);
    }

    public void ShowAccuracyTip()
    {
        if (PlayerPrefs.GetInt(PREF_ACCURACY_TIP, 1) == 0) return;

        if (root)
        {
            root.SetActive(true);
            if (titleText) titleText.text = "💡 TIP";
            if (bodyText)  bodyText.text =
                "Keep accuracy above 80% for best results!\n\n" +
                "Match the avatar's movements as closely as you can.";
        }
    }

    public void ShowFatigueTip()
    {
        if (PlayerPrefs.GetInt(PREF_FATIGUE_TIP, 1) == 0) return;

        if (root)
        {
            root.SetActive(true);
            if (titleText) titleText.text = "⚠️ NOTICE";
            if (bodyText)  bodyText.text =
                "Your fatigue is high.\n\n" +
                "Take a break if needed or press Pause to rest.";
        }
    }

    public void Hide()
    {
        if (root) root.SetActive(false);
    }

    public void DontShowAccuracyTipAgain()
    {
        PlayerPrefs.SetInt(PREF_ACCURACY_TIP, 0);
        Hide();
    }

    public void DontShowFatigueTipAgain()
    {
        PlayerPrefs.SetInt(PREF_FATIGUE_TIP, 0);
        Hide();
    }
}
