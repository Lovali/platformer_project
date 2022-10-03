using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveVal;

    [SerializeField] private float moveSpeed;
    private bool jumpPressed;
    private bool againstLeftWall = false;
    private bool againstRightWall = false;
    private bool canDoubleJump = false;

    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float gravity = -5f;
    private float velocity;

    [SerializeField] private float dashForce = 3;
    private bool dashPressed;

    [SerializeField] private float sprintMultiplier = 3;
    private bool sprintPressed;

    PlayerController playerController;
    private void Awake()
    {
        playerController = gameObject.GetComponent<PlayerController>();
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

    void Update()
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
        }

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

        /*transform.Translate(new Vector3(moveVal.x, 0, 0) * moveSpeed * Time.deltaTime);

        if (againstLeftWall)
        {
            transform.Translate(new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime);
        }
        else if (againstRightWall)
        {
            transform.Translate(new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime);
        }*/
        if ((againstLeftWall && moveVal.x > 0) || (againstRightWall && moveVal.x < 0) || (!againstRightWall && !againstLeftWall))
        {
            transform.Translate(new Vector3(moveVal.x, 0, 0) * moveSpeed * Time.deltaTime);
        }

        velocity += gravity * Time.deltaTime;

        if (playerController.GetIsOnTheGround() && velocity < 0)
        {
            velocity = 0;
        }

        if (jumpPressed)
        {
            if (playerController.GetIsOnTheGround())
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
    }
}
