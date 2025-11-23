using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashboardController : MonoBehaviour
{
    [Header("Text Labels")]
    public TextMeshProUGUI exerciseNameText;
    public TextMeshProUGUI repetitionsText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI fatigueText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI statusText;
    
    [Header("Progress Bars")]
    public Slider accuracyBar;
    public Slider fatigueBar;
    public Image accuracyBarFill;
    public Image fatigueBarFill;
    
    [Header("Color Settings")]
    public Color excellentColor = Color.green;
    public Color goodColor = Color.yellow;
    public Color poorColor = Color.red;
    
    [Header("Target Reps")]
    public int targetReps = 10;
    
    private void Start()
    {
        ResetDisplay();
    }
    
    public void UpdateExerciseName(string exerciseName)
    {
        if (exerciseNameText != null)
            exerciseNameText.text = $"Exercise: {FormatExerciseName(exerciseName)}";
    }
    
    public void UpdateMetrics(float accuracy, float fatigue, float velocity, int reps, float time)
    {
        // Update text labels
        if (repetitionsText != null)
            repetitionsText.text = $"Reps: {reps} / {targetReps}";
        
        if (accuracyText != null)
            accuracyText.text = $"Accuracy: {(accuracy * 100):F1}%";
        
        if (fatigueText != null)
            fatigueText.text = $"Fatigue: {(fatigue * 100):F1}%";
        
        if (timerText != null)
        {
            int minutes = (int)(time / 60f);
            int seconds = (int)(time % 60f);
            timerText.text = $"Time: {minutes:D2}:{seconds:D2}";
        }
        
        // Update progress bars
        if (accuracyBar != null)
            accuracyBar.value = accuracy;
        
        if (fatigueBar != null)
            fatigueBar.value = fatigue;
        
        // Update bar colors
        UpdateAccuracyBarColor(accuracy);
        UpdateFatigueBarColor(fatigue);
        
        // Update status message
        UpdateStatusMessage(accuracy, fatigue, velocity);
    }
    
    private void UpdateAccuracyBarColor(float accuracy)
    {
        if (accuracyBarFill == null) return;
        
        if (accuracy > 0.9f)
            accuracyBarFill.color = excellentColor;
        else if (accuracy > 0.75f)
            accuracyBarFill.color = goodColor;
        else
            accuracyBarFill.color = poorColor;
    }
    
    private void UpdateFatigueBarColor(float fatigue)
    {
        if (fatigueBarFill == null) return;
        
        if (fatigue < 0.3f)
            fatigueBarFill.color = excellentColor;
        else if (fatigue < 0.6f)
            fatigueBarFill.color = goodColor;
        else
            fatigueBarFill.color = poorColor;
    }
    
    private void UpdateStatusMessage(float accuracy, float fatigue, float velocity)
    {
        if (statusText == null) return;
        
        string message = "";
        
        // Priority: Fatigue > Accuracy > Velocity
        if (fatigue > 0.7f)
        {
            message = "High Fatigue - Consider Resting";
        }
        else if (accuracy > 0.9f && fatigue < 0.5f)
        {
            message = "Excellent Performance!";
        }
        else if (accuracy > 0.8f)
        {
            message = "Good Job! Keep Going!";
        }
        else if (accuracy < 0.75f)
        {
            if (velocity > 1.2f)
                message = "Try Slower, Focus on Form";
            else
                message = "Focus on Accuracy";
        }
        else
        {
            message = "Maintain Steady Pace";
        }
        
        statusText.text = message;
    }
    
    public void ResetDisplay()
    {
        if (repetitionsText != null)
            repetitionsText.text = $"Reps: 0 / {targetReps}";
        
        if (accuracyText != null)
            accuracyText.text = "Accuracy: 100.0%";
        
        if (fatigueText != null)
            fatigueText.text = "Fatigue: 0.0%";
        
        if (timerText != null)
            timerText.text = "Time: 00:00";
        
        if (statusText != null)
            statusText.text = "Ready to Start";
        
        if (accuracyBar != null)
            accuracyBar.value = 1f;
        
        if (fatigueBar != null)
            fatigueBar.value = 0f;
    }
    
    private string FormatExerciseName(string rawName)
    {
        // Convert "JumpingJacks" to "Jumping Jacks"
        return System.Text.RegularExpressions.Regex.Replace(rawName, "([a-z])([A-Z])", "$1 $2");
    }
}
