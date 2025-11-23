using System;
using System.Collections.Generic;


[System.Serializable]
public class SessionData
{
    // Session identification
    public string sessionID;
    public string exerciseType;
    
    // Timing information
    public string startTime;
    public string endTime;
    public float duration;
    
    // Performance metrics
    public int targetReps;
    public int totalReps;
    public float averageAccuracy;
    public float averageFatigue;
    public float averageVelocity;
    
    // Peak values
    public float peakAccuracy;
    public float peakFatigue;
    public float peakVelocity;
    
    // Historical data points
    public List<RehabMetrics> metricsHistory;
    
    // Additional metadata
    public string notes;
    public int difficulty;
    

    public SessionData()
    {
        sessionID = System.Guid.NewGuid().ToString();
        exerciseType = "Unknown";
        startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        endTime = "";
        duration = 0f;
        targetReps = 10;
        totalReps = 0;
        averageAccuracy = 0f;
        averageFatigue = 0f;
        averageVelocity = 0f;
        peakAccuracy = 0f;
        peakFatigue = 0f;
        peakVelocity = 0f;
        metricsHistory = new List<RehabMetrics>();
        notes = "";
        difficulty = 1;
    }
    

    public void CalculateAverages()
    {
        if (metricsHistory == null || metricsHistory.Count == 0)
            return;
        
        float sumAccuracy = 0f;
        float sumFatigue = 0f;
        float sumVelocity = 0f;
        
        peakAccuracy = 0f;
        peakFatigue = 0f;
        peakVelocity = 0f;
        
        foreach (var metric in metricsHistory)
        {
            sumAccuracy += metric.accuracy;
            sumFatigue += metric.fatigue;
            sumVelocity += metric.velocity;
            
            if (metric.accuracy > peakAccuracy)
                peakAccuracy = metric.accuracy;
            if (metric.fatigue > peakFatigue)
                peakFatigue = metric.fatigue;
            if (metric.velocity > peakVelocity)
                peakVelocity = metric.velocity;
        }
        
        int count = metricsHistory.Count;
        averageAccuracy = sumAccuracy / count;
        averageFatigue = sumFatigue / count;
        averageVelocity = sumVelocity / count;
    }
}
