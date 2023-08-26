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
    
    [SerializeField] private LayerMask climbableLayer;
    [SerializeField] private float climbingSpeed = 2f;
    [SerializeField] private float climbingSideJumpForce = 2f;
    
    public bool disableControls = false;

    private BoxCollider2D boxCollider;
    
    private float horizontalInput;
    private Vector2 horizontalDir;
    private float verticalInput;
    private Vector2 verticalDir;
    
    private bool IsGrounded => CheckGround(Vector2.down);
    private bool isLastTouchWall = false;
    private float currentCoyoteTime;

    private bool didJump;

    private CompositeCollider2D climbableCollider;
    private float climbableProgress = 0;
    
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        // currentJumpCooldown = maxJumpCooldown;
    }

    void Update()
    {
        if (disableControls) return;
        SetAxisInput();

        UpdateClimb();
        
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

        UpdateAnimationState();
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

    private void SetAxisInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        horizontalDir = (Vector2.right * horizontalInput).normalized;
        
        verticalInput = Input.GetAxisRaw("Vertical");
        verticalDir = (Vector2.up * verticalInput).normalized;
    }

    private void UpdateAnimationState()
    {
        if (playerPull.IsGrabbingObject)
        {
            if (Mathf.Approximately(horizontalDir.magnitude, 0) ||
                Mathf.Approximately(Mathf.Sign(playerPull.GrabbedObjectDirection.x), Mathf.Sign(horizontalDir.x)))
            {
                playerAnimationController.SetPush();
            }
            else
            {
                playerAnimationController.SetPull();
            }

            return;
        }
        
        if (climbableCollider != null)
        {
            playerAnimationController.SetClimbing();
            return;
        }

        if (!IsGrounded)
        {
            if ((CheckWall(Vector2.left) && horizontalDir.x < 0) ||
                (CheckWall(Vector2.right) && horizontalDir.x > 0))
            {
                playerAnimationController.SetWallSlide();
                return;    
            }

            if (rb.velocity.y > 0)
            {
                playerAnimationController.SetJump();
            }
            else
            {
                playerAnimationController.SetFall();
            }
        }
        
        if (IsGrounded)
        {
            if (horizontalDir.magnitude > 0)
            {
                playerAnimationController.SetRun();
                return;
            }
            playerAnimationController.SetIdle();
            return;
        }
    }

    private void UpdateClimb()
    {
        if (verticalDir == Vector2.zero) return;
        if (climbableCollider == null)
        {
            if (!TryGetClimbable(Vector2.zero, out Transform hitTransform)) return;

            climbableCollider = hitTransform.GetComponent<CompositeCollider2D>();

            var newTopOfClimbable = climbableCollider.bounds.center + Vector3.up * climbableCollider.bounds.size.y/2;
            var newBottomOfClimbable = climbableCollider.bounds.center - Vector3.up * climbableCollider.bounds.size.y/2;
            climbableProgress = Mathf.InverseLerp(newBottomOfClimbable.y, newTopOfClimbable.y, transform.position.y);
            
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            transform.SetX(climbableCollider.bounds.center.x);
            didJump = false;
            return;
        }

        var topOfClimbable = climbableCollider.bounds.center + Vector3.up * climbableCollider.bounds.size.y/2;
        var bottomOfClimbable = climbableCollider.bounds.center - Vector3.up * climbableCollider.bounds.size.y/2;
        climbableProgress += verticalDir.y * climbingSpeed * Time.deltaTime;

        if (climbableProgress < 0)
        {
            DetachClimbable();
            return;
        }

        climbableProgress = Mathf.Clamp(climbableProgress, 0, 1);
        
        transform.position = Vector2.Lerp(bottomOfClimbable, topOfClimbable, climbableProgress);
    }

    private void UpdateJump()
    {
        if (didJump) return; 
        
        if (!Input.GetButtonDown("Jump")) return;
        
        if (climbableCollider != null)
        {
            DetachClimbable();
            var climbingSideJumpDir = horizontalDir.magnitude > 0.01f ? horizontalDir : Vector2.right; 
            rb.AddForce(Vector2.up * jumpForce + climbingSideJumpDir * climbingSideJumpForce);
            didJump = true;
            return;
        }
        
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
        }
        
        var moveDir = horizontalMovement * speed;
        rb.AddForce(moveDir);
    }

    private void SetFlip()
    {
        if (Mathf.Approximately(horizontalDir.magnitude, 0)) return;
        
        var shouldFlip = horizontalInput < 0;

        if (playerPull.IsGrabbingObject)
        {
            shouldFlip = playerPull.GrabbedObjectDirection.x < 0;
        }
        
        spriteRenderer.flipX = shouldFlip;
        playerPull.FlipX = shouldFlip;
    }

    private void DetachClimbable()
    {
        climbableCollider = null;
        rb.isKinematic = false;
    }

    private bool TryGetClimbable(Vector2 dir, out Transform hitTransform)
    {
        hitTransform = null;
        var hitInfo = BoxCast(dir, climbableLayer);
        if (!hitInfo) return false;
        hitTransform = hitInfo.transform;
        return true;
    } 
    
    private bool CheckGround(Vector2 dir) => BoxCast(dir, jumpableGround);
    private bool CheckWall(Vector2 dir) => BoxCast(dir, jumpableWall);

    private RaycastHit2D BoxCast(Vector2 dir, LayerMask layer)
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, (Vector2)boxCollider.bounds.size, 0f, dir.normalized, 0.1f, layer);
    }

}