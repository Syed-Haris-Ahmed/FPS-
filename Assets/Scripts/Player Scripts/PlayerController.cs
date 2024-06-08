using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float minSpeed = 2f;
    [SerializeField] float maxSpeed = 4f;
    [SerializeField] float rotationSpeed = 500f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpForce = 5f;  // Add jump force

    private CharacterController characterController;
    [SerializeField] CameraController cameraController;
    private Animator animator;

    private bool isGrounded;
    private bool hasControl = true;

    private float ySpeed;

    private Quaternion targetRotation;

    bool isWalking;
    bool isBackwardsWalking;
    bool isSprinting;
    bool isIdle = true;
    bool isJumping;

    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsBackwardsWalking = Animator.StringToHash("isBackwardsWalking");
    private static readonly int IsSprinting = Animator.StringToHash("isRunning");
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private static readonly int IsJumping = Animator.StringToHash("isJumping");

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        moveSpeed = Mathf.Clamp(moveSpeed, minSpeed, maxSpeed);
    }

    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        var moveInput = new Vector3(h, 0, v).normalized;

        var cameraRotation = cameraController.PlanarRotation;

        var moveDir = cameraRotation * moveInput;

        if (!hasControl) {
            return;
        }

        GroundCheck();

        if (isGrounded) {
            ySpeed = -0.5f;  // Small negative value to keep grounded

            // Handle jump input
            if (Input.GetButtonDown("Jump")) {
                isJumping = true;
                ySpeed = jumpForce;
            } else {
                isJumping = false;
            }
        } else {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;

        bool walkingInput = Input.GetKey(KeyCode.W);
        bool backwardsWalkingInput = Input.GetKey(KeyCode.S);
        bool sprintingInput = Input.GetKey(KeyCode.LeftShift);

        if (walkingInput)
        {
            if (sprintingInput)
            {
                isSprinting = true;
                isWalking = false;
                isIdle = false;

                if (moveSpeed < maxSpeed)
                {
                    moveSpeed += 1.5f * Time.deltaTime;
                }
            }
            else
            {
                isSprinting = false;
                isWalking = true;
                isIdle = false;
                if (moveSpeed > minSpeed)
                {
                    moveSpeed -= 2 * Time.deltaTime;
                }
            }
        }
        else if (backwardsWalkingInput)
        {
            isSprinting = false;
            isWalking = false;
            isIdle = false;
            
            isBackwardsWalking = true;
        }
        else
        {
            isSprinting = false;
            isWalking = false;
            isBackwardsWalking = false;
            isIdle = true;
        }

        // Update animator parameters
        animator.SetBool(IsWalking, isWalking);
        animator.SetBool(IsBackwardsWalking, isBackwardsWalking);
        animator.SetBool(IsSprinting, isSprinting);
        animator.SetBool(IsIdle, isIdle);
        animator.SetBool(IsJumping, isJumping);

        characterController.Move(velocity * Time.deltaTime);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, cameraRotation, rotationSpeed * Time.deltaTime);
    }

    private void GroundCheck() {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
        animator.SetBool("isGrounded", isGrounded);  // Ensure Animator knows if we're grounded
    }
}
