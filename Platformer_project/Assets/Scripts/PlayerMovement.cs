using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PauseMenu pauseMenu;

    private Vector2 moveVal;
    private bool isOnTheGround = false;
    private bool isSlowed = false;
    [SerializeField] private float slowForce = 2;

    [SerializeField] private float moveSpeed;
    private bool jumpPressed;
    private bool againstLeftWall = false;
    private bool againstRightWall = false;
    private bool canDoubleJump = false;

    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float gravity = -5f;
    [SerializeField] private float velocity;
    [SerializeField] private float horizontalVelocity;

    [SerializeField] private float dashForce = 3;
    private bool dashPressed;

    [SerializeField] private float sprintMultiplier = 3;
    private bool sprintPressed;

    [SerializeField] private bool goDownPressed;
    private GameObject traversablePlatformGameObject;

    [SerializeField] private bool isWallSliding = false;
    [SerializeField] private float wallSlidingSpeed = -5;

    [SerializeField] private ParticleSystem dust;

    void OnPause(InputValue value)
    {
        pauseMenu.pausePressed = value.isPressed;
    }

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

    void OnGoDown(InputValue value)
    {
        goDownPressed = value.isPressed;
    }

    private void FixedUpdate()
    {
        if (dashPressed)
        {
            CreateDust();
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
                horizontalVelocity = sprintMultiplier * moveSpeed;
            }
            else
            {
                horizontalVelocity = moveSpeed;
                
            }
            if(isSlowed)
            {
                horizontalVelocity /= slowForce;
            }
            transform.Translate(new Vector3(moveVal.x * horizontalVelocity, 0, 0) * Time.deltaTime);
        }

        if (jumpPressed)
        {
            CreateDust();
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
            transform.parent = collision.gameObject.transform;
        }
        if (collision.gameObject.tag == "Ceil")
        {
            velocity = 0;
        }
        if (collision.gameObject.tag == "Bouncing")
        {
            velocity = 12.5f;
        }
        if (collision.gameObject.tag == "Slowing")
        {
            isSlowed = true;
        }
        if (collision.gameObject.tag == "Traversable")
        {
            isOnTheGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LeftWall"))
        {
            againstLeftWall = false;
        }
        if (collision.gameObject.CompareTag("RightWall"))
        {
            againstRightWall = false;
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnTheGround = false;
            transform.parent = null;
        }
        if (collision.gameObject.CompareTag("Slowing"))
        {
            isSlowed = false;
        }
        if (collision.gameObject.CompareTag("Traversable"))
        {
            isOnTheGround = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Traversable"))
        {
            if (goDownPressed)
            {
                traversablePlatformGameObject = collision.gameObject;
                Down();
                Invoke(nameof(Down), 0.5f);
            }
        }
    }

    private void Down()
    {
        traversablePlatformGameObject.GetComponent<Collider2D>().enabled = !traversablePlatformGameObject.GetComponent<Collider2D>().enabled;
    }

    private void CreateDust()
    {
        dust.Play();
    }
}
