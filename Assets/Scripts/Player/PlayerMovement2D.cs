using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float horizontal;
    public float speed = 8f;
    public float jumpForce = 10f;
    public float horizontalJumpForce = 5f;
    public bool isFacingRight = true;

    [SerializeField] private float maxCoyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private bool isLastTouchWall;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask jumpableWall;
    //[SerializeField] private Vector2 wallCastSizeOffset;
    //[SerializeField] private Vector2 groundCastSizeOffset;
    private BoxCollider2D coll;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;


    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        var horizontalMovement = Vector2.right * horizontal;
        if (IsGrounded())
        {
            coyoteTimeCounter = 0;
            isLastTouchWall = false;
        }
        else if (CheckWall(horizontalMovement))
        {
            coyoteTimeCounter = 0;
            isLastTouchWall = true;
        }
        else if(coyoteTimeCounter < maxCoyoteTime)
        {
            coyoteTimeCounter += Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (CheckWall(horizontalMovement) || isLastTouchWall && coyoteTimeCounter < maxCoyoteTime)
            {
                print("Yes wall");
                var horizontalJumpDir = -horizontalMovement.normalized;
                rb.AddForce(new Vector2(horizontalJumpDir.x * horizontalJumpForce, jumpForce));
                coyoteTimeCounter = maxCoyoteTime;
            }
            else if (IsGrounded() || !isLastTouchWall && coyoteTimeCounter < maxCoyoteTime)
            {
                print("Yes Ground");
                rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
                coyoteTimeCounter = maxCoyoteTime;
            }
            
        }
        //wallIsSlide();
        Flip();
    }

    private void FixedUpdate()
    {
        var horizontalMovement = Vector2.right * horizontal;

        if (CheckGround(horizontalMovement.normalized) || CheckWall(horizontalMovement.normalized))
        {
            horizontalMovement = Vector2.zero;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            //rb.velocity = new Vector2(horizontal * speed,0f);
        }

        var moveDir = horizontalMovement * speed;
        rb.AddForce(moveDir);

    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    //private bool IsGrounded()
    //{
    //    return Physics2D.BoxCast(coll.bounds.center, (Vector2)coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    //}

    //private bool IsTouchingAWall()
    //{
    //    return Physics2D.BoxCast(coll.bounds.center, (Vector2)coll.bounds.size + wallCastSizeOffset, 0f, Vector2.down, 0.1f, jumpableWall);
    //}

    //private void wallIsSlide()
    //{
    //    if (IsTouchingAWall() && !IsGrounded() && horizontal != 0f)
    //    {
    //        isWallSliding = true;
    //        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
    //    }
    //    else
    //    {
    //        isWallSliding = false;
    //    }
    //}

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }


    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, (Vector2)coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private bool IsTouchingAWall()
    {
        return Physics2D.BoxCast(coll.bounds.center, (Vector2)coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableWall);
    }

    private bool CheckGround(Vector2 dir)
    {
        return Physics2D.BoxCast(coll.bounds.center, (Vector2)coll.bounds.size, 0f, dir.normalized, 0.1f, jumpableGround);
    }

    private bool CheckWall(Vector2 dir)
    {
        return Physics2D.BoxCast(coll.bounds.center, (Vector2)coll.bounds.size, 0f, dir.normalized, 0.1f, jumpableWall);
    }

}