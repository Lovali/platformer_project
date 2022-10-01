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

    public float jumpForce = 10;
    public float gravity = -5f;
    private float velocity;

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

    void Update()
    {
        transform.Translate(new Vector3(moveVal.x, 0, 0) * moveSpeed * Time.deltaTime);

        if (againstLeftWall)
        {
            transform.Translate(new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime);
        }
        if (againstRightWall)
        {
            transform.Translate(new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime);
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
