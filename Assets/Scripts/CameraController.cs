using UnityEngine;

public class CameraController : MonoBehaviour
{       
    [SerializeField] Transform player;
    [SerializeField] private float rotationSpeed = 2f;
    
    [SerializeField] float minVerticalAngle = -20;
    [SerializeField] float maxVerticalAngle = 45;

    [SerializeField] float headHeight = 1.8f; // Adjust this value to match the height of the player's head
    
    [SerializeField] bool invertX;
    [SerializeField] bool invertY;
    
    private float invertXVal;
    private float invertYVal;

    float rotationY;
    float rotationX;
    
    // Start is called before the first frame update
    void Start()
    {
        // Initially position the camera at the player's head position
        transform.position = player.transform.position + Vector3.up * headHeight;
        transform.rotation = player.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        invertXVal = invertX ? -1 : 1;
        invertYVal = invertY ? -1 : 1;
        
        rotationY += Input.GetAxis("Mouse X") * invertXVal * rotationSpeed;
        rotationX += Input.GetAxis("Mouse Y") * -invertYVal * rotationSpeed;

        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        // Apply the rotation to the camera and the player
        transform.rotation = targetRotation;
        player.rotation = Quaternion.Euler(0, rotationY, 0);

        // Ensure the camera follows the player's head position
        transform.position = player.position + Vector3.up * headHeight;
    }
    
    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);
}