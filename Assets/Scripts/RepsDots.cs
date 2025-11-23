using UnityEngine;
using UnityEngine.UI;

public class RepsDots : MonoBehaviour
{
    [Header("Setup")]
    public GameObject dotPrefab;    // small circle Image
    public int target = 10;         // total reps / total dots

    [Header("Colors")]
    public Color todoColor = new Color(0.35f, 0.45f, 0.52f, 0.6f); // grey
    public Color doneColor = new Color(0.10f, 0.80f, 0.40f, 1f);   // green

    private Image[] dots;

    void Awake()
    {
        
        if (dotPrefab == null)
        {
            return;
        }
        
        CreateDots();
    }

    void CreateDots()
    {
        // Clear existing dots if any
        if (dots != null)
        {
            foreach (var dot in dots)
            {
                if (dot != null)
                    Destroy(dot.gameObject);
            }
        }

        // Create new dots array
        dots = new Image[target];
        for (int i = 0; i < target; i++)
        {
            GameObject dotGO = Instantiate(dotPrefab, transform);
            var img = dotGO.GetComponent<Image>();
            if (img == null)
            {
                continue;
            }
            img.color = todoColor;
            dots[i] = img;
        }
    }

  
    public void SetReps(int repsCompleted)
    {        
        if (dots == null || dots.Length == 0)
        {
            Debug.LogError("RepsDots: dots array is null or empty!");
            return;
        }

        // Check if dots array size matches target (might have changed)
        if (dots.Length != target)
        {
            CreateDots();
        }
        
        repsCompleted = Mathf.Clamp(repsCompleted, 0, target);

        for (int i = 0; i < target; i++)
        {
            if (dots[i] == null)
            {
                Debug.LogError($"RepsDots: dot[{i}] is null!");
                continue;
            }
            
            Color newColor = (i < repsCompleted) ? doneColor : todoColor;
            dots[i].color = newColor;
        }
    }


    public void UpdateTarget(int newTarget)
    {
        target = newTarget;
        CreateDots();
    }
}