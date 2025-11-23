using UnityEngine;
using TMPro;

public class AvatarBubble : MonoBehaviour
{
    public Transform follow;        
    public Vector3 worldOffset = new Vector3(0, 2f, 0);
    public CanvasGroup cg;
    public TMP_Text text;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
        Hide();
    }

    void LateUpdate()
    {
        if (!cam || !follow) return;
        Vector3 worldPos = follow.position + worldOffset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
        (transform as RectTransform).position = screenPos;
    }

    public void Show(string msg)
    {
        if (text) text.text = msg;
        if (cg)
        {
            cg.alpha = 1f;
            cg.blocksRaycasts = false;
        }
    }

    public void Hide()
    {
        if (cg)
        {
            cg.alpha = 0f;
            cg.blocksRaycasts = false;
        }
    }
}