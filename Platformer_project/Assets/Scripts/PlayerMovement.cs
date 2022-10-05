using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveVal;
    private bool isOnTheGround = false;

    [SerializeField] private float moveSpeed;
    private bool jumpPressed;
    private bool againstLeftWall = false;
    private bool againstRightWall = false;
    private bool canDoubleJump = false;

    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float gravity = -5f;
    [SerializeField] private float velocity;

    [SerializeField] private float dashForce = 3;
    private bool dashPressed;

    [SerializeField] private float sprintMultiplier = 3;
    private bool sprintPressed;

    [SerializeField] private bool isWallSliding = false;
    [SerializeField] private float wallSlidingSpeed = -5;

    void OnMove(InputValue value)
    {
        moveVal = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        jumpPressed = value.isPressed;
    }

    void OnDash(InputValue value)
    {
        dashPressed = value.isPressed;
    }

    void OnSprint(InputValue value)
    {
        sprintPressed = value.isPressed;
    }

    private void FixedUpdate()
    {
        if (dashPressed)
        {
            Vector3 pre_pos = transform.position;
            Vector3 dash = new Vector3(moveVal.x * dashForce, 0, 0) * moveSpeed * Time.deltaTime;
            RaycastHit2D hit = Physics2D.Linecast(pre_pos, pre_pos + dash, LayerMask.GetMask("Collision"));

            if (hit.collider)
            {
                transform.Translate(new Vector3(pre_pos.x - hit.collider.transform.position.x, 0, 0) * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(dash);
            }
            dashPressed = false;
        }
    }

    void Update()
    {
        if ((againstLeftWall && moveVal.x > 0) || (againstRightWall && moveVal.x < 0) || (!againstRightWall && !againstLeftWall))
        {
            if(sprintPressed)
            {
                transform.Translate(new Vector3(moveVal.x * sprintMultiplier, 0, 0) * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(new Vector3(moveVal.x, 0, 0) * moveSpeed * Time.deltaTime);
            }            
        }

        if (jumpPressed)
        {
            if (isOnTheGround)
            {
                canDoubleJump = true;
                velocity = jumpForce;
                return;
            }
            else if (canDoubleJump)
            {
                canDoubleJump = false;
                velocity = jumpForce;
                return;
            }
        }

        if ((againstLeftWall || againstRightWall) && !isOnTheGround)
        {
            isWallSliding = true;
            canDoubleJump = true;
        }
        else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            if (velocity > 0 && !jumpPressed) velocity = 0;
            velocity += wallSlidingSpeed * Time.deltaTime;
        }
        else
        {
            velocity += gravity * Time.deltaTime;
        }

        if (isOnTheGround && velocity < 0)
        {
            velocity = 0;
        }

        transform.Translate(new Vector3(0, velocity, 0) * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LeftWall")
        {
            againstLeftWall = true;
        }
        if (collision.gameObject.tag == "RightWall")
        {
            againstRightWall = true;
        }
        if (collision.gameObject.tag == "Ground")
        {
            isOnTheGround = true;
        }
        if (collision.gameObject.tag == "Ceil")
        {
            velocity = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "LeftWall")
        {
            againstLeftWall = false;
        }
        if (collision.gameObject.tag == "RightWall")
        {
            againstRightWall = false;
        }
        if (collision.gameObject.tag == "Ground")
        {
            isOnTheGround = false;
        }
    }
}
