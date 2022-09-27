using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveVal;
    public float moveSpeed;
    bool jumpPressed;

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
        if (jumpPressed)
        {
            transform.Translate(new Vector3(0, 1, 0) * moveSpeed * Time.deltaTime);
        }
    }
}
