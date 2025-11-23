using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    
    [Header("Orbit Settings")]
    public float distance = 4f;
    public float height = 1.5f;
    public float orbitSpeed = 10f;
    public bool autoOrbit = false;
    
    [Header("Mouse Control")]
    public float mouseSensitivity = 3f;
    public bool enableMouseOrbit = true;
    
    [Header("Zoom")]
    public float minDistance = 2f;
    public float maxDistance = 10f;
    public float zoomSpeed = 2f;
    
    private float currentAngle = 0f;
    private float currentHeight;
    
    private void Start()
    {
        currentHeight = height;
        
    }
    
    private void LateUpdate()
    {
        if (target == null) return;
        
        HandleInput();
        UpdateCameraPosition();
    }
    
    private void HandleInput()
    {
        // Auto orbit
        if (autoOrbit)
        {
            currentAngle += orbitSpeed * Time.deltaTime;
        }
        
        // Mouse orbit
        if (enableMouseOrbit && Input.GetMouseButton(1)) // Right mouse button
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            currentAngle += mouseX;
        }
        
        // Scroll zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            distance -= scroll * zoomSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
        }
        
        // Arrow key orbit
        if (Input.GetKey(KeyCode.LeftArrow))
            currentAngle -= orbitSpeed * Time.deltaTime * 3f;
        if (Input.GetKey(KeyCode.RightArrow))
            currentAngle += orbitSpeed * Time.deltaTime * 3f;
        
        // Height adjustment
        if (Input.GetKey(KeyCode.UpArrow))
            currentHeight += orbitSpeed * Time.deltaTime * 0.5f;
        if (Input.GetKey(KeyCode.DownArrow))
            currentHeight -= orbitSpeed * Time.deltaTime * 0.5f;
        
        currentHeight = Mathf.Clamp(currentHeight, 0.5f, 5f);
    }
    
    private void UpdateCameraPosition()
    {
        // Calculate orbit position
        float radians = currentAngle * Mathf.Deg2Rad;
        
        Vector3 offset = new Vector3(
            Mathf.Sin(radians) * distance,
            currentHeight,
            Mathf.Cos(radians) * distance
        );
        
        transform.position = target.position + offset;
        transform.LookAt(target.position + Vector3.up * 1f); // Look at avatar's chest height
    }
    
    public void SetAutoOrbit(bool enabled)
    {
        autoOrbit = enabled;
    }
    
    public void ResetCamera()
    {
        currentAngle = 0f;
        currentHeight = height;
        distance = 4f;
    }
}
