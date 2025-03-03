using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private bool canDoubleJump;
    private bool isDashing;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 1f;
    public float dashSpeed = 15f;
    public float dashTime = 0.2f;

    public Transform cameraTransform; // Reference to the camera's transform

    private float horizontal;
    private float vertical;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get input from keyboard
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.LeftShift)) Dash();
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
        }
    }

    void Move()
    {
        if (cameraTransform == null) return; // Safety check

        // Get the camera's forward and right direction, ignoring vertical rotation
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Convert movement input to camera-relative movement
        Vector3 movement = (camForward * vertical + camRight * horizontal).normalized * moveSpeed;

        // Apply movement to the Rigidbody
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

        // Rotate player to face movement direction
        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            canDoubleJump = false;
        }
    }

    void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;

            // Dash in the direction the player is facing
            Vector3 dashDirection = transform.forward.normalized;

            // Maintain current Y linearVelocity to avoid sudden vertical movement
            rb.linearVelocity = new Vector3(dashDirection.x * dashSpeed, rb.linearVelocity.y, dashDirection.z * dashSpeed);

            Invoke("StopDash", dashTime);
        }
    }

    void StopDash()
    {
        isDashing = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
