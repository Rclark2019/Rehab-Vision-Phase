using UnityEngine;
using System;

[System.Serializable]
public struct ExerciseDataPoint
{
    public float time;
    public float accuracy;
    public float fatigue;
    public float velocity;
    public int repetitionCount;
}

public class MockDataGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    public float playbackSpeed = 1f;
    public float sessionLength = 120f; // 2 minutes default
    public int randomSeed = 42;
    
    [Header("Accuracy Settings")]
    [Range(0.5f, 1f)]
    public float minAccuracy = 0.7f;
    [Range(0.5f, 1f)]
    public float maxAccuracy = 1f;
    public float accuracyVariation = 0.1f;
    
    [Header("Fatigue Settings")]
    public float fatigueRate = 0.01f; // Per second
    public float maxFatigue = 0.9f;
    
    [Header("Velocity Settings")]
    public float baseVelocity = 0.8f;
    public float velocityOscillation = 0.3f;
    public float velocityFrequency = 2f;
    
    [Header("Repetition Settings")]
    public float secondsPerRep = 3f;
    public int maxReps = 30;
    
    [Header("Current Data")]
    public ExerciseDataPoint currentData;
    
    private float elapsedTime = 0f;
    private System.Random random;
    private bool isGenerating = false;
    
    private void Start()
    {
        random = new System.Random(randomSeed);
        Reset();
    }
    
    public void StartGeneration()
    {
        isGenerating = true;
    }
    
    public void StopGeneration()
    {
        isGenerating = false;
    }
    
    public void Reset()
    {
        elapsedTime = 0f;
        currentData = new ExerciseDataPoint
        {
            time = 0f,
            accuracy = maxAccuracy,
            fatigue = 0f,
            velocity = baseVelocity,
            repetitionCount = 0
        };
        isGenerating = false;
    }
    
    private void Update()
    {
        if (isGenerating)
        {
            GenerateDataPoint();
        }
    }
    
    private void GenerateDataPoint()
    {
        elapsedTime += Time.deltaTime * playbackSpeed;
        
        // Generate accuracy (random walk with bounds)
        float accuracyDelta = ((float)random.NextDouble() - 0.5f) * accuracyVariation;
        float newAccuracy = currentData.accuracy + accuracyDelta;
        newAccuracy = Mathf.Clamp(newAccuracy, minAccuracy, maxAccuracy);
        
        // Generate fatigue (gradually increases)
        float newFatigue = Mathf.Min(currentData.fatigue + (fatigueRate * Time.deltaTime), maxFatigue);
        
        // Accuracy decreases slightly as fatigue increases
        newAccuracy -= newFatigue * 0.05f;
        newAccuracy = Mathf.Clamp(newAccuracy, minAccuracy, maxAccuracy);
        
        // Generate velocity (oscillates)
        float velocityOffset = Mathf.Sin(elapsedTime * velocityFrequency) * velocityOscillation;
        float newVelocity = baseVelocity + velocityOffset;
        newVelocity = Mathf.Clamp(newVelocity, 0.3f, 1.5f);
        
        // Calculate repetition count
        int newRepCount = Mathf.Min((int)(elapsedTime / secondsPerRep), maxReps);
        
        // Update current data
        currentData = new ExerciseDataPoint
        {
            time = elapsedTime,
            accuracy = newAccuracy,
            fatigue = newFatigue,
            velocity = newVelocity,
            repetitionCount = newRepCount
        };
    }
    
    public ExerciseDataPoint GetCurrentData()
    {
        return currentData;
    }
    
    public string GetDataAsJson()
    {
        return JsonUtility.ToJson(currentData, true);
    }
}
