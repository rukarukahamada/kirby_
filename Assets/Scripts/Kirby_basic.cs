using UnityEngine;
using UnityEngine.SceneManagement;

public class Kirby_basic : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpForce = 4f;
    private bool isGrounded;
    private Rigidbody rb;

    public int hp = 100;

    private int jumpCount = 0;
    public int maxJumpCount = 10;
    private float currentJumpForce;
    public float gravityMultiplier = 1f;
    private bool canFall = false;
    private bool canJump = true;

    public float maxJumpHeight = 10f;

    private Color originalColor;
    private Renderer playerRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();

        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }

        currentJumpForce = JumpForce;
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || jumpCount < maxJumpCount) && canJump)
        {
            Jump();
        }

        if (canFall)
        {
            ApplyGravity();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(10);
        }
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical) * Speed * Time.deltaTime;
        transform.Translate(movement);
    }

    void Jump()
    {
        if (jumpCount < 3)
        {
            float jumpForceForThisJump = Mathf.Lerp(0, maxJumpHeight - transform.position.y, jumpCount / 3f);
            jumpCount++;
        }
        else if (jumpCount >= 3 && jumpCount <= 10)
        {
            rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
            currentJumpForce = maxJumpHeight - transform.position.y;
            jumpCount++;
        }
        else if (jumpCount > 10)
        {
            currentJumpForce = Mathf.Lerp(currentJumpForce, 0f, 0.1f);
            rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
            jumpCount++;
        }

        if (transform.position.y >= maxJumpHeight)
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.y = Mathf.Min(velocity.y, 0);
            rb.linearVelocity = velocity;

            transform.position = new Vector3(transform.position.x, maxJumpHeight, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
        {
            isGrounded = true;
            jumpCount = 0;
            currentJumpForce = JumpForce;
            canFall = false;
            canJump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("ground"))
        {
            isGrounded = false;
        }
    }

    void ApplyGravity()
    {
        rb.AddForce(Vector3.down * gravityMultiplier, ForceMode.Acceleration);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        SceneManager.LoadScene("GAMEOVER");
    }

    public void Heal(int amount)
    {
        hp += amount;
        if (hp > 100)
        {
            hp = 100;
        }
    }
}
