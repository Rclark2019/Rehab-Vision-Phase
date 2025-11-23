using UnityEngine;
using TMPro;

public class StatusTextPulse : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public float pulseSpeed = 2f;
    public float minScale = 0.95f;
    public float maxScale = 1.05f;
    public bool enablePulse = true;
    public bool pulseColor = true;
    public Color color1 = Color.yellow;
    public Color color2 = Color.white;
    
    private Vector3 originalScale;
    private Color originalColor;
    private float t = 0f;
    
    void Start()
    {
        if (statusText == null) statusText = GetComponent<TextMeshProUGUI>();
        if (statusText != null)
        {
            originalScale = statusText.transform.localScale;
            originalColor = statusText.color;
        }
    }
    
    void Update()
    {
        if (statusText == null || !enablePulse) return;
        t += Time.deltaTime * pulseSpeed;
        float s = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(t)+1f)/2f);
        statusText.transform.localScale = originalScale * s;
        if (pulseColor) statusText.color = Color.Lerp(color1, color2, (Mathf.Sin(t)+1f)/2f);
    }
    
    public void SetPulseEnabled(bool enabled)
    {
        enablePulse = enabled;
        if (!enabled && statusText != null)
        {
            statusText.transform.localScale = originalScale;
            statusText.color = originalColor;
        }
    }
}
