using UnityEngine;

public class RehabAvatarController : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Animation State Names")]
    public string idleStateName = "Idle";
    public string jumpingJacksStateName = "JumpingJacks";
    public string stretchingStateName = "Stretching";

    // Current state tracking
    private string currentExercise = "";
    private bool isExercising = false;

    void Start()
    {
        // Auto-find animator if not assigned
        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("RehabAvatarController: No Animator found! Assign one in Inspector.");
            return;
        }

        // Explicitly start in idle/waving state
        Debug.Log("RehabAvatarController: Starting in Idle state");
        SetAnimationTrigger(idleStateName);
    }


    public void StartExercise(string exerciseName)
    {        
        currentExercise = exerciseName;
        isExercising = true;
        
        // Map exercise name to animation state
        string animationState = GetAnimationStateForExercise(exerciseName);
        
        // Set the trigger for the exercise
        SetAnimationTrigger(animationState);
    }


    public void ResumeExercise()
    {
        isExercising = true;
        
        if (!string.IsNullOrEmpty(currentExercise))
        {
            string animationState = GetAnimationStateForExercise(currentExercise);
            SetAnimationTrigger(animationState);
        }
    }


    public void PauseExercise()
    {
        isExercising = false;
        SetAnimationTrigger(idleStateName);
    }


    public void StopExercise()
    {
        isExercising = false;
        currentExercise = "";
        SetAnimationTrigger(idleStateName);
    }


    public void SwitchAnimation(string newExerciseName)
    {
        currentExercise = newExerciseName;
        
        string animationState = GetAnimationStateForExercise(newExerciseName);
        SetAnimationTrigger(animationState);
    }


    private string GetAnimationStateForExercise(string exerciseName)
    {
        switch (exerciseName)
        {
            case "JumpingJacks":
                return jumpingJacksStateName;
            case "Stretching":
                return stretchingStateName;
            default:
                return idleStateName;
        }
    }

  
    private void SetAnimationTrigger(string stateName)
    {
        if (animator == null)
        {
            return;
        }

        // Get the correct trigger name
        string triggerName = GetTriggerForState(stateName);
        
        
        // Reset all triggers first to ensure clean state
        ResetAllTriggers();
        
        // Set the trigger
        animator.SetTrigger(triggerName);
        
        // Force animator to process the trigger immediately
        animator.Update(0f);
        
    }


    private string GetTriggerForState(string stateName)
    {
        if (stateName == idleStateName) return "ToIdle";
        if (stateName == jumpingJacksStateName) return "ToJumpingJacks";
        if (stateName == stretchingStateName) return "ToStretching";
        return "ToIdle";
    }


    private void ResetAllTriggers()
    {
        animator.ResetTrigger("ToIdle");
        animator.ResetTrigger("ToJumpingJacks");
        animator.ResetTrigger("ToStretching");
    }

    public string GetCurrentExercise()
    {
        return currentExercise;
    }

    public bool IsExercising()
    {
        return isExercising;
    }
}