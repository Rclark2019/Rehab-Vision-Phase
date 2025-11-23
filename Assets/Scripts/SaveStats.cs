using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]
public class SessionDataModel
{
    public string exerciseType;
    public string timestamp;
    public float totalDuration;
    public int totalReps;
    
    public float averageAccuracy;
    public float averageFatigue;
    public float averageVelocity;
    
    public float minAccuracy;
    public float maxAccuracy;
    
    public List<FrameData> frames;
    
    public SessionDataModel()
    {
        frames = new List<FrameData>();
    }
}

[System.Serializable]
public class FrameData
{
    public float time;
    public int repetitionCount;
    public float accuracy;
    public float fatigue;
    public float velocity;
}

public class SaveStats : MonoBehaviour
{
    [Header("Save Settings")]
    public string saveDirectory = "RehabData";
    public bool saveAsJson = true;
    public bool saveAsCsv = true;
    
    [Header("Current Session")]
    private SessionDataModel currentSession;
    private List<FrameData> frameBuffer;
    
    private void Start()
    {
        ClearSessionData();
    }
    
    public void ClearSessionData()
    {
        currentSession = new SessionDataModel();
        frameBuffer = new List<FrameData>();
    }
    
    public void RecordFrame(float time, int reps, float accuracy, float fatigue, float velocity)
    {
        FrameData frame = new FrameData
        {
            time = time,
            repetitionCount = reps,
            accuracy = accuracy,
            fatigue = fatigue,
            velocity = velocity
        };
        
        frameBuffer.Add(frame);
    }
    
    public void SaveSessionData(string exerciseType)
    {
        if (frameBuffer.Count == 0)
        {
            Debug.LogWarning("No data to save!");
            return;
        }
        
        currentSession.exerciseType = exerciseType;
        currentSession.timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        currentSession.frames = new List<FrameData>(frameBuffer);
        
        CalculateStatistics();
        
        string fullPath = Path.Combine(Application.dataPath, saveDirectory);
        if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
        
        string baseFilename = $"Session_{exerciseType}_{currentSession.timestamp}";
        
        if (saveAsJson) SaveAsJson(fullPath, baseFilename);
        if (saveAsCsv)  SaveAsCsv(fullPath, baseFilename);
        
        Debug.Log($"Session saved: {baseFilename}");
    }
    
    private void CalculateStatistics()
    {
        if (frameBuffer.Count == 0) return;
        
        float sumAccuracy = 0f;
        float sumFatigue = 0f;
        float sumVelocity = 0f;
        float minAcc = float.MaxValue;
        float maxAcc = float.MinValue;
        
        foreach (var frame in frameBuffer)
        {
            sumAccuracy += frame.accuracy;
            sumFatigue += frame.fatigue;
            sumVelocity += frame.velocity;
            if (frame.accuracy < minAcc) minAcc = frame.accuracy;
            if (frame.accuracy > maxAcc) maxAcc = frame.accuracy;
        }
        
        int count = frameBuffer.Count;
        currentSession.totalDuration = frameBuffer[count - 1].time;
        currentSession.totalReps = frameBuffer[count - 1].repetitionCount;
        currentSession.averageAccuracy = sumAccuracy / count;
        currentSession.averageFatigue = sumFatigue / count;
        currentSession.averageVelocity = sumVelocity / count;
        currentSession.minAccuracy = minAcc;
        currentSession.maxAccuracy = maxAcc;
    }
    
    private void SaveAsJson(string directory, string filename)
    {
        string json = JsonUtility.ToJson(currentSession, true);
        string filepath = Path.Combine(directory, filename + ".json");
        File.WriteAllText(filepath, json);
        Debug.Log($"JSON saved to: {filepath}");
    }
    
    private void SaveAsCsv(string directory, string filename)
    {
        string filepath = Path.Combine(directory, filename + ".csv");
        using (StreamWriter writer = new StreamWriter(filepath))
        {
            writer.WriteLine("Time,Repetitions,Accuracy,Fatigue,Velocity");
            foreach (var frame in frameBuffer)
            {
                writer.WriteLine($"{frame.time:F2},{frame.repetitionCount},{frame.accuracy:F4},{frame.fatigue:F4},{frame.velocity:F4}");
            }
            writer.WriteLine();
            writer.WriteLine("SUMMARY");
            writer.WriteLine($"Exercise Type,{currentSession.exerciseType}");
            writer.WriteLine($"Total Duration,{currentSession.totalDuration:F2}");
            writer.WriteLine($"Total Reps,{currentSession.totalReps}");
            writer.WriteLine($"Average Accuracy,{currentSession.averageAccuracy:F4}");
            writer.WriteLine($"Average Fatigue,{currentSession.averageFatigue:F4}");
            writer.WriteLine($"Average Velocity,{currentSession.averageVelocity:F4}");
            writer.WriteLine($"Min Accuracy,{currentSession.minAccuracy:F4}");
            writer.WriteLine($"Max Accuracy,{currentSession.maxAccuracy:F4}");
        }
        Debug.Log($"CSV saved to: {filepath}");
    }
    
    public SessionDataModel GetCurrentSession() => currentSession;
}
