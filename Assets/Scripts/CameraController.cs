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

    public float Sensitivity {
        get { return sensitivity; }
        set { sensitivity = value; }
    }

    [Range(0.1f, 9f)]
    [SerializeField] float sensitivity = 2f;

    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)]
    [SerializeField] float yRotationLimit = 88f;

    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
    const string yAxis = "Mouse Y";

    void Start()
    {
        // Initially position the camera at the player's head position
        transform.position = player.transform.position + Vector3.up * headHeight;
        transform.rotation = player.transform.rotation;
    }

    void Update()
    {
        invertXVal = invertX ? -1 : 1;
        invertYVal = invertY ? -1 : 1;

        rotation.x += Input.GetAxis(xAxis) * sensitivity * invertXVal;
        rotation.y += Input.GetAxis(yAxis) * -sensitivity * -invertYVal;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);

        var xQuat = Quaternion.AngleAxis(rotation.x - 40f, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation = xQuat * yQuat;

        // Ensure the camera follows the player's head position
        transform.position = player.position + Vector3.up * headHeight;

        // Apply horizontal rotation to the player
        player.rotation = Quaternion.Euler(0, rotation.x, 0);
    }

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotation.x, 0);
}
