using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveVal;
    [SerializeField]
    private float moveSpeed;
    bool jumpPressed;

    public float jumpForce = 10;
    public float gravity = -5f;
    float velocity;

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

        velocity += gravity * Time.deltaTime;

        if (playerController.GetIsOnTheGround() && velocity < 0)
        {
            velocity = 0;
        }

        if (playerController.GetIsOnTheGround() && jumpPressed)
        {
            velocity = jumpForce;
        }
        transform.Translate(new Vector3(0, velocity, 0) * Time.deltaTime);
    }
}
