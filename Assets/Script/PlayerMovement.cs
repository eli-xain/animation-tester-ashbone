using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    public GameObject dashDustPrefab;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float moveInput;

    private bool isDashing = false;
    private float dashTimeRemaining;
    private float lastDashTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Regular movement input
        moveInput = Input.GetAxisRaw("Horizontal");

        // Handle sprite flip
        if (moveInput < 0)
            spriteRenderer.flipX = true;
        else if (moveInput > 0)
            spriteRenderer.flipX = false;

        // Trigger dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDashTime + dashCooldown && moveInput != 0 && !isDashing)
        {
            StartDash();
        }

        // Update animator parameters
        animator.SetFloat("Speed", Mathf.Abs(isDashing ? 1 : moveInput));
        animator.SetBool("isDashing", isDashing);
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            float dashDirection = spriteRenderer.flipX ? -1f : 1f;
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y);

            dashTimeRemaining -= Time.fixedDeltaTime;
            if (dashTimeRemaining <= 0)
            {
                EndDash();
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

void StartDash()
{
    isDashing = true;
    dashTimeRemaining = dashDuration;
    lastDashTime = Time.time;

    animator.SetTrigger("Dash");

    // ✅ Immediately apply dash velocity so it doesn't wait for FixedUpdate
    float dashDirection = spriteRenderer.flipX ? -1f : 1f;
    rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y);
    

    // ✅ Instantiate dash dust on correct side
        // if (dashDustPrefab != null)
        // {
        //     float horizontalOffset = spriteRenderer.flipX ? -1f : 1f; // fixed direction
        //     Vector3 dustPosition = new Vector3(
        //         transform.position.x + horizontalOffset,
        //         transform.position.y - 0.5f, // near feet
        //         transform.position.z
        //     );

        //     GameObject dust = Instantiate(dashDustPrefab, dustPosition, Quaternion.identity);

        //     // ✅ Flip the dust based on player direction
        //     SpriteRenderer dustSprite = dust.GetComponent<SpriteRenderer>();
        //     if (dustSprite != null)
        //     {
        //         dustSprite.flipX = spriteRenderer.flipX;

        //         // ✅ Optional: Set opacity
        //         Color c = dustSprite.color;ddddddd
        //         c.a = 1f;
        //         dustSprite.color = c;
        //     }
        // }
}

// This will be called from the animation event
public void TriggerDashDustFromAnimation()
{
    if (dashDustPrefab != null)
    {
        float horizontalOffset = spriteRenderer.flipX ? 5f : -5f;
        Vector3 dustPosition = new Vector3(
            transform.position.x + horizontalOffset,
            transform.position.y - 0.5f,
            transform.position.z
        );

        GameObject dust = Instantiate(dashDustPrefab, dustPosition, Quaternion.identity);

        SpriteRenderer dustSprite = dust.GetComponent<SpriteRenderer>();
        if (dustSprite != null)
        {
            dustSprite.flipX = spriteRenderer.flipX;

            Color c = dustSprite.color;
            c.a = 1f;
            dustSprite.color = c;
        }
    }
}



    void EndDash()
    {
        isDashing = false;
    }
}
