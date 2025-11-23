using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExerciseManager : MonoBehaviour
{
    [Header("Controllers (assign in Inspector)")]
    public RehabAvatarController avatar;             
    public PerformanceGraphTexture graph;            
    public SaveStats saveStats;                     

    [Header("HeaderPanel")]
    public TextMeshProUGUI timeText;                 // DashboardPanel/HeaderPanel/TimeText
    public TextMeshProUGUI exerciseTitleText;        

    [Header("MetricsPanel")]
    public Slider accuracySlider;                    // AccuracySlider
    public Slider fatigueSlider;                     // FatigueSlider
    public Slider velocitySlider;                    // VelocitySlider
    public TextMeshProUGUI accuracyPct;             
    public TextMeshProUGUI fatiguePct;              
    public TextMeshProUGUI velocityVal;             
    public TextMeshProUGUI repsText;                
    public TextMeshProUGUI statusText;              

    [Header("SessionInfoPanel")]
    public TextMeshProUGUI sessionIDText;           
    public TextMeshProUGUI sessionDateText;        
    public TextMeshProUGUI difficultyText;         

    [Header("UI – Status Header")]
    public StatusIndicatorController statusIndicator;   // StatusHeader on MetricsPanel

    [Header("Reps Visualization")]
    public RepsDots repsDots;                        // dots row under Reps
    public RadialTimer radialTimer;                  // countdown ring timer

    [Header("Avatar Feedback")]
    public AvatarBubble avatarBubble;              

    [Header("Tutorial & Tips")]
    public TutorialTipManager tutorialTipManager;   

    [Header("Countdown")]
    public TextMeshProUGUI countdownText;            
    public float startCountdownSeconds = 3f;
    public float resumeCountdownSeconds = 2f;

    [Header("Session Settings")]
    public string startingExercise = "JumpingJacks";    
    
    [Header("Exercise Specific Settings")]
    [Tooltip("Reps for Jumping Jacks")]
    public int jumpingJacksReps = 15;
    [Tooltip("Difficulty stars for Jumping Jacks (1-5)")]
    [Range(1, 5)] public int jumpingJacksDifficulty = 4;
    
    [Tooltip("Reps for Stretching")]
    public int stretchingReps = 8;
    [Tooltip("Difficulty stars for Stretching (1-5)")]
    [Range(1, 5)] public int stretchingDifficulty = 2;
    
    public float sessionDuration = 30f;             
    public float dataUpdateInterval = 0.25f;        

    // runtime state
    string currentExercise;
    bool running;
    bool paused;
    float sessionTime;
    float tickTimer;
    int reps;
    int targetReps;           
    int currentDifficulty;   


    bool inCountdown;
    float countdownRemaining;
    bool startingFromPause;


    bool fatigueTipShown = false;


    float acc = 0.95f;
    float fat = 0.05f;
    float vel = 0.60f;

    void Start()
    {
        currentExercise = startingExercise;
        UpdateExerciseSettings(); 
        
        if (repsDots == null)
            Debug.LogError("ExerciseManager: RepsDots is NOT assigned! Assign it in the Inspector under 'Reps Visualization'");
        if (radialTimer == null)
            Debug.LogError("ExerciseManager: RadialTimer is NOT assigned! Assign it in the Inspector under 'Reps Visualization'");
        if (countdownText == null)
            Debug.LogError("ExerciseManager: CountdownText is NOT assigned! Assign it in the Inspector under 'Countdown'");
        if (avatarBubble == null)
            Debug.LogWarning("ExerciseManager: AvatarBubble is not assigned. Avatar feedback will be disabled.");
        if (tutorialTipManager == null)
            Debug.LogWarning("ExerciseManager: TutorialTipManager is not assigned. Tips will be disabled.");
        
        InitUI();
        UpdateSessionInfo();
        if (graph) graph.Clear();       

    
        if (statusIndicator)
            statusIndicator.SetState(UITheme.SessionState.Ready, "");

     
        if (countdownText)
            countdownText.gameObject.SetActive(false);
        if (radialTimer)
            radialTimer.gameObject.SetActive(false);
    }

    // friendly display name for UI while using internal ids for Animator
    string GetExerciseDisplayName()
    {
        return currentExercise == "JumpingJacks" ? "Jumping Jacks" : currentExercise;
    }

    // Get target reps for current exercise
    void UpdateExerciseSettings()
    {
        if (currentExercise == "JumpingJacks")
        {
            targetReps = jumpingJacksReps;
            currentDifficulty = jumpingJacksDifficulty;
        }
        else if (currentExercise == "Stretching")
        {
            targetReps = stretchingReps;
            currentDifficulty = stretchingDifficulty;
        }
        else
        {
            targetReps = 10; // default
            currentDifficulty = 3; // default
        }

    }

    void InitUI()
    {
        if (timeText) timeText.text = "Time: 00:00";
        if (exerciseTitleText) exerciseTitleText.text = $"Exercise: {GetExerciseDisplayName()}";

        if (accuracySlider) accuracySlider.value = acc;
        if (fatigueSlider)  fatigueSlider.value  = fat;
        if (velocitySlider) velocitySlider.value = vel;

        if (accuracyPct) accuracyPct.text = Mathf.RoundToInt(acc * 100f) + "%";
        if (fatiguePct)  fatiguePct.text  = Mathf.RoundToInt(fat * 100f) + "%";
        if (velocityVal) velocityVal.text = (0.8f + vel * 0.8f).ToString("0.0") + "x";

        if (repsText) repsText.text = $"{reps} / {targetReps}";
        if (repsDots) repsDots.SetReps(reps);  
    }

    void UpdateSessionInfo()
    {
        System.DateTime now = System.DateTime.Now;
        if (sessionIDText)   sessionIDText.text   = "#0001"; 
        if (sessionDateText) sessionDateText.text = now.ToString("MM/dd/yyyy  HH:mm");
        if (difficultyText)  difficultyText.text  = "Difficulty: " + Stars(currentDifficulty);
    }

    string Stars(int n)
    {
        n = Mathf.Clamp(n, 0, 5);
        return new string('★', n) + new string('☆', 5 - n);
    }

    void Update()
    {
        // ---- handle countdown first ----
        if (inCountdown)
        {
            countdownRemaining -= Time.deltaTime;
            if (countdownText)
            {
                int disp = Mathf.CeilToInt(Mathf.Max(countdownRemaining, 0f));
                countdownText.text = disp.ToString();
            }

            if (countdownRemaining <= 0f)
            {
                CompleteCountdown();
            }
            return; 
        }

        if (!running || paused) return;

        sessionTime += Time.deltaTime;
        tickTimer   += Time.deltaTime;

        if (timeText)
            timeText.text = $"Time: {Mathf.FloorToInt(sessionTime/60f):00}:{Mathf.FloorToInt(sessionTime%60f):00}";

        if (tickTimer >= dataUpdateInterval)
        {
            tickTimer = 0f;

            // mock metric drift
            acc = Mathf.Clamp01(acc + Random.Range(-0.03f, 0.02f));
            fat = Mathf.Clamp01(fat + Random.Range( 0.01f, 0.03f));
            vel = Mathf.Clamp01(vel + Random.Range(-0.05f, 0.05f));

            // update sliders + labels
            if (accuracySlider) accuracySlider.value = acc;
            if (fatigueSlider)  fatigueSlider.value  = fat;
            if (velocitySlider) velocitySlider.value = vel;

            if (accuracyPct) accuracyPct.text = Mathf.RoundToInt(acc * 100f) + "%";
            if (fatiguePct)  fatiguePct.text  = Mathf.RoundToInt(fat * 100f) + "%";
            if (velocityVal) velocityVal.text = (0.8f + vel * 0.8f).ToString("0.0") + "x";

            // Check for high fatigue and show warning tip
            if (fat > 0.7f && !fatigueTipShown && tutorialTipManager)
            {
                fatigueTipShown = true;
                tutorialTipManager.ShowFatigueTip();
            }

            // Show avatar bubble for low accuracy
            if (acc < 0.4f && avatarBubble)
            {
                avatarBubble.Show("⚠️ Too fast! Slow down and match the avatar.");
                Invoke(nameof(HideAvatarBubble), 3f);
            }

            // draw to graph
            if (graph) graph.AddSample(acc, fat, vel);

            // mock reps: +1 every 2 seconds until target
            if (sessionTime >= (reps + 1) * 2f && reps < targetReps)
            {
                reps++;
                if (repsText) repsText.text = $"{reps} / {targetReps}";
                
                if (repsDots != null)
                {
                    repsDots.SetReps(reps);
                }
                else
                {
                    Debug.LogWarning("RepsDots reference is NULL! Assign it in the Inspector!");
                }

                // Show completion message when target reached
                if (reps == targetReps)
                {
                    // Stop the session completely
                    running = false;
                    paused = false;
                    
                    if (statusIndicator)
                        statusIndicator.SetState(UITheme.SessionState.Complete, "done");
                    
                    // Avatar stops exercising and returns to idle
                    if (avatar)
                        avatar.StopExercise();
                    
                    if (avatarBubble)
                    {
                        avatarBubble.Show("✅ Session Complete! Great work!");
                        Invoke(nameof(HideAvatarBubble), 3f);
                    }
                }
            }

            if (saveStats) saveStats.RecordFrame(sessionTime, reps, acc, fat, vel);
        }
    }

    // ---- Countdown helpers ----
    void BeginCountdown(float seconds, bool fromPause)
    {
        startingFromPause = fromPause;
        inCountdown = true;
        countdownRemaining = seconds;

        // Show countdown text
        if (countdownText)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = Mathf.CeilToInt(seconds).ToString();
        }

        // Show and start radial timer for countdown
        if (radialTimer)
        {
            radialTimer.gameObject.SetActive(true);
            radialTimer.Begin(seconds);
        }

        if (avatar && fromPause)
        {
            avatar.PauseExercise();
        }

        running = false;
    }

    void CompleteCountdown()
    {
        inCountdown = false;
        
        // Hide countdown text
        if (countdownText)
            countdownText.gameObject.SetActive(false);
        
        // Hide radial timer after countdown
        if (radialTimer)
            radialTimer.gameObject.SetActive(false);

        running = true;
        paused  = false;

        if (avatar != null)
        {
            if (startingFromPause)
            {
                avatar.ResumeExercise();
            }
            else
            {
                avatar.StartExercise(currentExercise);
            }
        }
        else
        {
            Debug.LogError("❌ Avatar is NULL! Assign in ExerciseManager Inspector.");
        }

        if (statusIndicator)
            statusIndicator.SetState(UITheme.SessionState.Active, "running");

        // Show avatar bubble when exercise starts
        if (avatarBubble && !startingFromPause)
        {
            avatarBubble.Show("👋 Follow Me!");
            Invoke(nameof(HideAvatarBubble), 2f);
        }
    }

    void HideAvatarBubble()
    {
        if (avatarBubble) avatarBubble.Hide();
    }

    // ---- Buttons ----
    public void StartSession()
    {
        // reset state
        running = false; paused = false; sessionTime = 0f; tickTimer = 0f; reps = 0;
        acc = 0.95f; fat = 0.05f; vel = 0.60f;
        fatigueTipShown = false;

        // Set to default starting exercise
        currentExercise = startingExercise;
        UpdateExerciseSettings();
        
        // Update RepsDots
        if (repsDots)
        {
            repsDots.UpdateTarget(targetReps);
        }

        if (repsText) repsText.text = $"0 / {targetReps}";
        if (exerciseTitleText) exerciseTitleText.text = $"Ready - Press Switch to Start";
        if (graph) graph.Clear();
        if (saveStats) saveStats.ClearSessionData();

        UpdateSessionInfo();
        InitUI();

        if (statusIndicator)
            statusIndicator.SetState(UITheme.SessionState.Ready, "Press Switch Exercise to begin");

        // Ensure avatar is in Idle state
        if (avatar)
            avatar.PauseExercise();

    }

    public void PauseResume()
    {
        // if we are in countdown, ignore pause/resume
        if (inCountdown) return;

        if (!running && !paused)
            return;

        if (!paused)
        {
            // Go into paused state
            paused = true;
            if (avatar) avatar.PauseExercise();
            if (statusIndicator) statusIndicator.SetState(UITheme.SessionState.Paused, "paused");
        }
        else
        {
            // From paused -> start a short countdown, then resume
            paused = false;
            BeginCountdown(resumeCountdownSeconds, true);
        }
    }

    public void SwitchExercise()
    {
        // If currently in countdown, ignore
        if (inCountdown) return;

        // If not running at all, this is the FIRST exercise selection
        bool isFirstStart = !running && !paused;

        if (running)
        {
            // Already running - pause current exercise before switching
            paused = true;
            running = false;
            if (avatar) avatar.PauseExercise();
        }

        // Toggle exercise type
        currentExercise = (currentExercise == "JumpingJacks") ? "Stretching" : "JumpingJacks";
        
        // Update reps and difficulty for new exercise
        UpdateExerciseSettings();
        
        // Reset reps for new exercise
        reps = 0;
        
        // Update RepsDots to match new exercise target
        if (repsDots)
        {
            repsDots.UpdateTarget(targetReps);
        }
        
        if (exerciseTitleText) exerciseTitleText.text = $"Exercise: {GetExerciseDisplayName()}";
        if (repsText) repsText.text = $"{reps} / {targetReps}";

        // Update difficulty display
        UpdateSessionInfo();

        // Show switching message
        if (avatarBubble)
        {
            string message = isFirstStart ? 
                $"▶️ Starting {GetExerciseDisplayName()}..." : 
                $"🔄 Switching to {GetExerciseDisplayName()}...";
            avatarBubble.Show(message);
        }

        // Reset metrics
        fat = Mathf.Clamp01(fat * 0.7f);
        acc = Mathf.Clamp01(acc - 0.05f);
        vel = 0.60f;
        InitUI();

        tickTimer = 0f;
        sessionTime = 0f; // Reset session time on switch

        if (statusIndicator)
            statusIndicator.SetState(UITheme.SessionState.Ready, "switching");

        // Start countdown before exercise
        BeginCountdown(resumeCountdownSeconds, false);
        
    }

    public void SaveSession()
    {
        // You can keep this short transient text, or route through StatusIndicator with a new tail.
        if (statusText) statusText.text = "💾 Saving…";
        if (saveStats)  saveStats.SaveSessionData(currentExercise);
        if (statusText) statusText.text = "✅ Saved.";
    }

    public void ResetSession()
    {
        running = false; paused = false; sessionTime = 0f; tickTimer = 0f; reps = 0;
        inCountdown = false;
        fatigueTipShown = false;  // reset tip tracking
        
        if (countdownText) countdownText.gameObject.SetActive(false);
        if (radialTimer) radialTimer.gameObject.SetActive(false);

        if (avatar) avatar.StopExercise();
        if (graph) graph.Clear();
        if (saveStats) saveStats.ClearSessionData();
        acc = 0.95f; fat = 0.05f; vel = 0.60f;

        if (repsText) repsText.text = $"{reps} / {targetReps}";
        if (repsDots) repsDots.SetReps(reps);
        InitUI();

        if (statusIndicator)
            statusIndicator.SetState(UITheme.SessionState.Ready, "");
    }
}