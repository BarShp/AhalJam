using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float horizontalJumpForce = 5f;
    [SerializeField] float wallSlidingSpeed = 2f;
    [SerializeField] private float maxCoyoteTime = 0.2f;
    
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask jumpableWall;
    
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
        
        SetSpriteFlip();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
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

    private void SetSpriteFlip()
    {
        spriteRenderer.flipX = horizontalInput < 0;
    }

    private bool CheckGround(Vector2 dir) => CheckColliders(dir, jumpableGround); 

    private bool CheckWall(Vector2 dir) => CheckColliders(dir, jumpableWall);

    private bool CheckColliders(Vector2 dir, LayerMask layer)
    {
        return Physics2D.BoxCast(coll.bounds.center, (Vector2)coll.bounds.size, 0f, dir.normalized, 0.1f, layer);
    }

}