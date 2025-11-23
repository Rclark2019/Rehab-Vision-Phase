using UnityEngine;
using TMPro;
using System;

public class SessionInfoController : MonoBehaviour
{
    [Header("Text Elements")]
    public TextMeshProUGUI sessionIDText;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI difficultyText;
    
    [Header("Mock Data")]
    public bool useMockData = true;
    public int mockSessionNumber = 1;
    public int mockDifficulty = 2; // 1-5
    
    public void Start()
    {
        if (useMockData) GenerateMockSession();
        else UpdateSessionInfo(1, DateTime.Now, 2);
    }
    
    public void GenerateMockSession()
    {
        int sessionNum = (mockSessionNumber > 0) ? mockSessionNumber : UnityEngine.Random.Range(1, 100);
        UpdateSessionInfo(sessionNum, DateTime.Now, mockDifficulty);
    }
    
    public void UpdateSessionInfo(int sessionNumber, DateTime when, int difficulty)
    {
        if (sessionIDText) sessionIDText.text = $"📊 Session: #{sessionNumber:D4}";
        if (dateText) dateText.text = $"📅 {when:MM/dd/yyyy}  🕐 {when:HH:mm}";
        if (difficultyText) difficultyText.text = $"Difficulty: {GetStars(difficulty)}";
    }
    
    private string GetStars(int difficulty)
    {
        difficulty = Mathf.Clamp(difficulty, 1, 5);
        string s = "";
        for (int i=0;i<difficulty;i++) s += "⭐";
        for (int i=difficulty;i<5;i++) s += "☆";
        return s;
    }
    
    public void IncrementSession()
    {
        mockSessionNumber++;
        GenerateMockSession();
    }
    
    public void SetDifficulty(int level)
    {
        mockDifficulty = Mathf.Clamp(level,1,5);
        GenerateMockSession();
    }
}
