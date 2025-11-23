using UnityEngine;

public class FeedbackController : MonoBehaviour
{
    [Header("Lighting")]
    public Light performanceLight;
    public Light ambientLight;
    
    [Header("Color Settings")]
    public Color excellentColor = Color.green;
    public Color goodColor = Color.yellow;
    public Color warningColor = new Color(1f, 0.5f, 0f); // Orange
    public Color poorColor = Color.red;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip repCompleteSound;
    public AudioClip sessionCompleteSound;
    public AudioClip warningSound;
    
    [Header("Particles")]
    public ParticleSystem repCompleteParticles;
    public ParticleSystem excellentParticles;
    
    [Header("Floor Emission")]
    public Renderer floorRenderer;
    public string emissionProperty = "_EmissionColor";
    private Material floorMaterial;
    
    [Header("Settings")]
    public float lightTransitionSpeed = 2f;
    public float emissionIntensity = 2f;
    
    private Color targetLightColor;
    
    private void Start()
    {
        if (performanceLight != null)
            targetLightColor = performanceLight.color;
        
        if (floorRenderer != null)
        {
            floorMaterial = floorRenderer.material;
        }
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        // Smooth transition of light colors
        if (performanceLight != null)
        {
            performanceLight.color = Color.Lerp(
                performanceLight.color, 
                targetLightColor, 
                Time.deltaTime * lightTransitionSpeed
            );
        }
    }
    
    public void UpdateVisualFeedback(float accuracy, float fatigue)
    {
        // Determine target color based on performance
        Color newColor;
        
        if (fatigue > 0.7f)
        {
            // High fatigue = warning/poor colors
            newColor = fatigue > 0.8f ? poorColor : warningColor;
        }
        else if (accuracy > 0.9f)
        {
            newColor = excellentColor;
        }
        else if (accuracy > 0.75f)
        {
            newColor = goodColor;
        }
        else
        {
            newColor = warningColor;
        }
        
        // Update light target
        targetLightColor = newColor;
        
        // Update ambient light
        if (ambientLight != null)
        {
            ambientLight.color = Color.Lerp(Color.white, newColor, 0.3f);
        }
        
        // Update floor emission
        UpdateFloorEmission(newColor, accuracy);
    }
    
    private void UpdateFloorEmission(Color color, float intensity)
    {
        if (floorMaterial != null)
        {
            Color emissionColor = color * (intensity * emissionIntensity);
            floorMaterial.SetColor(emissionProperty, emissionColor);
        }
    }
    
    public void TriggerRepComplete()
    {
        // Play sound
        if (audioSource != null && repCompleteSound != null)
        {
            audioSource.PlayOneShot(repCompleteSound);
        }
        
        // Trigger particles
        if (repCompleteParticles != null)
        {
            repCompleteParticles.Play();
        }
        
        // Flash floor emission
        if (floorMaterial != null)
        {
            StartCoroutine(FlashEmission());
        }
    }
    
    public void TriggerSessionComplete()
    {
        // Play completion sound
        if (audioSource != null && sessionCompleteSound != null)
        {
            audioSource.PlayOneShot(sessionCompleteSound);
        }
        
        // Trigger celebration particles
        if (excellentParticles != null)
        {
            excellentParticles.Play();
        }
        
        // Set light to excellent color
        targetLightColor = excellentColor;
    }
    
    public void TriggerWarning()
    {
        if (audioSource != null && warningSound != null)
        {
            audioSource.PlayOneShot(warningSound);
        }
        
        targetLightColor = warningColor;
    }
    
    private System.Collections.IEnumerator FlashEmission()
    {
        if (floorMaterial == null) yield break;
        
        Color originalEmission = floorMaterial.GetColor(emissionProperty);
        Color flashColor = targetLightColor * emissionIntensity * 3f;
        
        // Flash up
        float flashDuration = 0.2f;
        float elapsed = 0f;
        
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flashDuration;
            
            Color currentColor = Color.Lerp(originalEmission, flashColor, t);
            floorMaterial.SetColor(emissionProperty, currentColor);
            
            yield return null;
        }
        
        // Flash down
        elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flashDuration;
            
            Color currentColor = Color.Lerp(flashColor, originalEmission, t);
            floorMaterial.SetColor(emissionProperty, currentColor);
            
            yield return null;
        }
    }
    
    public void ResetFeedback()
    {
        targetLightColor = excellentColor;
        
        if (performanceLight != null)
            performanceLight.color = excellentColor;
        
        if (ambientLight != null)
            ambientLight.color = Color.white;
    }
}
