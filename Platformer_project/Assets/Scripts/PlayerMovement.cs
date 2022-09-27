using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveVal;
    [SerializeField] private float moveSpeed;
    private bool jumpPressed;
    private bool againstLeftWall = false;
    private bool againstRightWall = false;

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
        if (jumpPressed)
        {
            transform.Translate(new Vector3(0, 1, 0) * moveSpeed * Time.deltaTime);
        }
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
