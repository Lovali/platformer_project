using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private PauseMenu pauseMenu;

    [SerializeField] private Vector2 moveVal;
    private bool isOnTheGround = false;

    [SerializeField] private float maxMoveSpeed = 20;
    private float acceleration;
    private float deceleration;
    [SerializeField] private float accelerationRate = 100;
    [SerializeField] private float decelerationRate = 50;

    private bool jumpPressed;
    private bool againstLeftWall = false;
    private bool againstRightWall = false;
    private bool canDoubleJump = false;
    [SerializeField] private float jumpForce = 13;
    [SerializeField] private float maxJumpForce = 20;
    private float jumpBoosted;
    private float jumpBoostRate;

    [SerializeField] private float gravity = -18f;
    private float velocity;
    private float horizontalVelocity;

    [SerializeField] private float dashForce = 5;
    private bool dashPressed;

    [SerializeField] private float sprintMultiplier = 2;
    private bool sprintPressed;

    private bool goDownPressed;
    private GameObject traversablePlatformGameObject;

    private bool isWallSliding = false;
    [SerializeField] private float wallSlidingSpeed = -5;

    private bool isSlowed = false;
    [SerializeField] private float slowForce = 2;

    [SerializeField] private ParticleSystem dust;
    [SerializeField] private Slider jumpSlider;

    // Methods related to new input system
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

    private void Start()
    {
        pauseMenu = GameObject.Find("GameManager").GetComponent<PauseMenu>();
        acceleration = maxMoveSpeed / accelerationRate;
        deceleration = maxMoveSpeed / decelerationRate;
        jumpBoosted = jumpForce;
        jumpBoostRate = (maxJumpForce - jumpForce) / 100;
        jumpSlider.minValue = jumpForce;
        jumpSlider.maxValue = maxJumpForce;
    }
    private void FixedUpdate()
    {
        if (dashPressed)
        {
            CreateDust();
            Vector3 pre_pos = transform.position;
            Vector3 dash = new Vector3(moveVal.x * dashForce, 0, 0) * maxMoveSpeed * Time.deltaTime;
            RaycastHit2D hit = Physics2D.Linecast(pre_pos, pre_pos + dash, LayerMask.GetMask("Collision"));

            if (hit.collider)
            {
                transform.Translate(new Vector3(pre_pos.x - hit.collider.transform.position.x, 0, 0) * maxMoveSpeed * Time.deltaTime);
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
        if ((againstLeftWall && moveVal.x > 0) || (againstRightWall && moveVal.x < 0) || (!againstRightWall && !againstLeftWall && (moveVal.x >= 0.1f || moveVal.x <= -0.1f)))
        {
            // Non-linear horizontal movement
            if (Mathf.Abs(horizontalVelocity) > maxMoveSpeed)
            {
                horizontalVelocity = maxMoveSpeed * moveVal.x;
            }
            else if (moveVal.x < 0 && horizontalVelocity < maxMoveSpeed)
            {
                horizontalVelocity -= acceleration;
            }
            else if (moveVal.x > 0 && horizontalVelocity > -maxMoveSpeed)
            {
                horizontalVelocity += acceleration;
            }
            else
            {
                if (horizontalVelocity > deceleration)
                {
                    horizontalVelocity -= deceleration;
                }
                else if (horizontalVelocity < -deceleration)
                {
                    horizontalVelocity += deceleration;
                }
                else
                {
                    horizontalVelocity = 0;
                }
            }
            if (Mathf.Abs(horizontalVelocity) > maxMoveSpeed)
            {
                horizontalVelocity = maxMoveSpeed * moveVal.x;
            }

            if (sprintPressed)
            {
                horizontalVelocity = maxMoveSpeed * moveVal.x * sprintMultiplier;
            }
            else if (Mathf.Abs(horizontalVelocity) > maxMoveSpeed * sprintMultiplier)
            {
                horizontalVelocity = maxMoveSpeed / 2 * moveVal.x;
            }

            if (isSlowed)
            {
                horizontalVelocity /= slowForce;
            }
            transform.Translate(new Vector3(horizontalVelocity, 0, 0) * Time.deltaTime);
        }
        else
        {
            horizontalVelocity = 0;
        }

        if (jumpPressed)
        {
            CreateDust();
            if (isOnTheGround)
            {
                canDoubleJump = true;
                jumpBoosted += jumpBoostRate;
                jumpSlider.gameObject.SetActive(true);
                jumpSlider.value = jumpBoosted;
                if (jumpBoosted > maxJumpForce)
                {
                    jumpBoosted = maxJumpForce;
                }
                velocity = jumpBoosted;
                return;
            }
            else if (canDoubleJump)
            {
                canDoubleJump = false;
                velocity = jumpForce;
                return;
            }
        }
        else
        {
            jumpBoosted = jumpForce;
            jumpSlider.value = jumpForce;
            jumpSlider.gameObject.SetActive(false);
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
        if (collision.gameObject.CompareTag("LeftWall"))
        {
            againstLeftWall = true;
        }
        if (collision.gameObject.CompareTag("RightWall"))
        {
            againstRightWall = true;
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnTheGround = true;
            transform.parent = collision.gameObject.transform;
        }
        if (collision.gameObject.CompareTag("Ceil"))
        {
            velocity = 0;
        }
        if (collision.gameObject.CompareTag("Bouncing"))
        {
            isOnTheGround = true;
            velocity = 12.5f;
        }
        if (collision.gameObject.CompareTag("Slowing"))
        {
            isOnTheGround = true;
            isSlowed = true;
        }
        if (collision.gameObject.CompareTag("Traversable"))
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
            isOnTheGround = false;
        }
        if (collision.gameObject.CompareTag("Traversable"))
        {
            isOnTheGround = false;
        }
        if (collision.gameObject.CompareTag("Bouncing"))
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
