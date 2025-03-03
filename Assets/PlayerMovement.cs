using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private bool canDoubleJump;
    private bool isDashing;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashSpeed = 15f;
    public float dashTime = 0.2f;

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
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * moveSpeed;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
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
            rb.linearVelocity = transform.forward * dashSpeed;
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
