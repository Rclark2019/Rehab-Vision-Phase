using UnityEngine;


[System.Serializable]
public class RehabMetrics
{
    // Core performance metrics (0 to 1 range)
    public float accuracy;      // How accurately exercise is performed (0-1)
    public float fatigue;       // Fatigue level (0-1, higher = more tired)
    public float velocity;      // Movement speed (0-1.5 typically)
    
    // Rep counting
    public int repetitions;     // Current rep count
    
    // Timing
    public float timestamp;     // Time since session start
    
    // Joint angles (optional, for detailed tracking)
    public float shoulderAngle;
    public float elbowAngle;
    public float hipAngle;
    public float kneeAngle;
    
    // Quality indicators
    public float formScore;     // Overall form quality (0-1)
    public float rangeOfMotion; // ROM percentage (0-1)
    

    public RehabMetrics()
    {
        accuracy = 1.0f;
        fatigue = 0.0f;
        velocity = 1.0f;
        repetitions = 0;
        timestamp = 0f;
        shoulderAngle = 0f;
        elbowAngle = 0f;
        hipAngle = 0f;
        kneeAngle = 0f;
        formScore = 1.0f;
        rangeOfMotion = 1.0f;
    }
    

    public RehabMetrics(float accuracy, float fatigue, float velocity, int reps, float time = 0f)
    {
        this.accuracy = Mathf.Clamp01(accuracy);
        this.fatigue = Mathf.Clamp01(fatigue);
        this.velocity = Mathf.Clamp(velocity, 0f, 2f);
        this.repetitions = reps;
        this.timestamp = time;
        
        // Calculate derived metrics
        this.formScore = accuracy;
        this.rangeOfMotion = 1.0f - (fatigue * 0.3f); // Fatigue reduces ROM
        
        // Default joint angles
        this.shoulderAngle = 0f;
        this.elbowAngle = 0f;
        this.hipAngle = 0f;
        this.kneeAngle = 0f;
    }
    

    public RehabMetrics(RehabMetrics other)
    {
        this.accuracy = other.accuracy;
        this.fatigue = other.fatigue;
        this.velocity = other.velocity;
        this.repetitions = other.repetitions;
        this.timestamp = other.timestamp;
        this.shoulderAngle = other.shoulderAngle;
        this.elbowAngle = other.elbowAngle;
        this.hipAngle = other.hipAngle;
        this.kneeAngle = other.kneeAngle;
        this.formScore = other.formScore;
        this.rangeOfMotion = other.rangeOfMotion;
    }
    

    public float GetPerformanceScore()
    {
        // Weighted score: accuracy matters most, velocity and fatigue adjust
        float score = (accuracy * 0.6f) + (velocity * 0.2f) + ((1f - fatigue) * 0.2f);
        return Mathf.Clamp01(score) * 100f;
    }
    

    public bool IsGoodPerformance()
    {
        return accuracy >= 0.8f && fatigue < 0.6f && velocity >= 0.7f;
    }
    

    public bool IsWarningCondition()
    {
        return accuracy < 0.6f || fatigue > 0.8f || velocity < 0.5f;
    }
    

    public string GetStatusDescription()
    {
        if (IsWarningCondition())
            return "Needs Attention";
        else if (IsGoodPerformance())
            return "Excellent";
        else
            return "Good";
    }
    

    public override string ToString()
    {
        return $"RehabMetrics[Acc:{accuracy:F2}, Fat:{fatigue:F2}, Vel:{velocity:F2}, Reps:{repetitions}]";
    }
}
