using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] private PlayerPull playerPull;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float horizontalJumpForce = 5f;
    [SerializeField] float wallSlidingSpeed = 2f;
    [SerializeField] private float maxCoyoteTime = 0.2f;
    [SerializeField] private float maxHorizontalSpeed = 30;
    [SerializeField] private float maxVerticalSpeed = 20;
    [SerializeField] private float scaleFactor = 0.1f;
    
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask jumpableWall;
    
    public bool disableControls = false;

    private BoxCollider2D coll;
    private float horizontalInput;
    private Vector2 horizontalDir;
    private bool IsGrounded => CheckGround(Vector2.down);
    private bool isLastTouchWall = false;
    // private float currentJumpCooldown;
    private float currentCoyoteTime;

    private bool didJump;
    
    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        // currentJumpCooldown = maxJumpCooldown;
    }

    void Update()
    {
        if (disableControls) return;
        SetHorizontalInput();

        if (IsGrounded)
        {
            isLastTouchWall = false;
            didJump = false;
            currentCoyoteTime = 0;
        } 
        else if (CheckWall(horizontalDir))
        {
            isLastTouchWall = true;
            didJump = false;
            currentCoyoteTime = 0;
        }
        else if (!didJump)
        {
            currentCoyoteTime += Time.deltaTime;
        }
        
        UpdateJump();
        
        SetFlip();
    }

    private void FixedUpdate()
    {
        if (disableControls) return;
        UpdateMovement();
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed), 
                                    Mathf.Clamp(rb.velocity.y, -maxVerticalSpeed, maxVerticalSpeed));
        SquashAndStretch();
    }
    
    private void SquashAndStretch()
    {

        // Calculate squash and stretch factor
        float verticalVelocity = rb.velocity.y; // Get vertical velocity
        float squashFactor = 1 - Mathf.Abs(verticalVelocity) * scaleFactor;

        // Calculate target scale based on squash/stretch

        var currentScale = spriteRenderer.transform.localScale;
        
        var targetScale = new Vector2(1, squashFactor);

        // Smoothly interpolate scale and apply it
        var transitionSpeed = 10.0f; // Adjust as needed
        var newScale = Vector2.Lerp(currentScale, targetScale, Time.fixedDeltaTime * transitionSpeed);
        spriteRenderer.transform.localScale = newScale;
    }

    private void SetHorizontalInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        horizontalDir = (Vector2.right * horizontalInput).normalized;
    }

    private void UpdateJump()
    {
        if (didJump) return; 
        
        if (!Input.GetButtonDown("Jump")) return;
        
        if (IsGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce);
            didJump = true;
            return;
        }
        
        if (CheckWall(horizontalDir))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(-horizontalDir.x * horizontalJumpForce, jumpForce));
            
            // Workaround to solve the walls being detected even after the jump (and causing double jump)
            transform.position += new Vector3(-horizontalDir.x * 0.2f, 0, 0); 
            didJump = true;
            return;
        }

        if (currentCoyoteTime < maxCoyoteTime)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);

            var coyoteJumpForce = Vector2.up * jumpForce; 
            if (isLastTouchWall)
            {
                coyoteJumpForce.x = horizontalDir.x * horizontalJumpForce;
            }
            
            rb.AddForce(coyoteJumpForce);

            didJump = true;
        }
    }

    private void UpdateMovement()
    {
        var horizontalMovement = horizontalDir;
        
        if (CheckGround(horizontalDir) || CheckWall(horizontalDir))
        {
            // Slide if you're going into a wall (or side of ground)
            horizontalMovement = Vector2.zero;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            
            // TODO: Set animation sliding
        }
        else
        {
            if (horizontalMovement == Vector2.zero)
            {
                playerAnimationController.SetIdle();
            }
            else
            {
                playerAnimationController.SetRun();
            }
        }
        
        var moveDir = horizontalMovement * speed;
        rb.AddForce(moveDir);
    }

    private void SetFlip()
    {
        var shouldFlip = horizontalInput < 0;
        spriteRenderer.flipX = shouldFlip;
        playerPull.FlipX = shouldFlip;
    }

    private bool CheckGround(Vector2 dir) => CheckColliders(dir, jumpableGround); 

    private bool CheckWall(Vector2 dir) => CheckColliders(dir, jumpableWall);

    private bool CheckColliders(Vector2 dir, LayerMask layer)
    {
        return Physics2D.BoxCast(coll.bounds.center, (Vector2)coll.bounds.size, 0f, dir.normalized, 0.1f, layer);
    }

}