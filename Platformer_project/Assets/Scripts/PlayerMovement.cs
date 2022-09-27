using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveVal;
    public float moveSpeed;
    void OnMove(InputValue value)
    {
        moveVal = value.Get<Vector2>();
    }

    void Update()
    {
        transform.Translate(new Vector3(moveVal.x, 0, 0) * moveSpeed * Time.deltaTime);
    }
}
