using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 7f;
    public float jumpForce = 15f;
    private bool isGrounded;
    private Rigidbody rb;
    public float maxJumpTime = 0.5f; // Maximum duration to hold the jump
    private float jumpTimeCounter;
    private bool isJumping;
    public float fallAcceleration = 20f; // Acceleration rate for falling



    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get Rigidbody component
        Debug.Log("PlayerMovement script initialized.");
    }

    void Update()
    {
        Move();
        Jump();
        ApplyFallAcceleration();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Debug.Log($"Movement Input - Horizontal: {horizontal}, Vertical: {vertical}");

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            Debug.Log("Player is moving...");
            rb.linearVelocity = moveDirection * speed;
        }
        else
        {
            // Stop movement when no keys are pressed
            rb.linearVelocity = Vector3.zero;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = 0f;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter < maxJumpTime)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                jumpTimeCounter += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
        
    }
    void ApplyFallAcceleration()
    {
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            // Incrementally increase the downward velocity
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallAcceleration - 1) * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Player landed on the ground.");
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
