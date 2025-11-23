using UnityEngine;

public class PropSwapController : MonoBehaviour
{
    public GameObject ringProps;    // visible for JumpingJacks
    public GameObject sphereProps;  // visible for Stretching

    public void SetExercise(string exerciseName)
    {
        bool jj = exerciseName == "JumpingJacks";
        if (ringProps)   ringProps.SetActive(jj);
        if (sphereProps) sphereProps.SetActive(!jj);
    }
}
