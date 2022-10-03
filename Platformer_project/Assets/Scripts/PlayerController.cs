using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isOnTheGround;
    
    void Start()
    {
        isOnTheGround = false;
    }

    private void Update()
    {
        if (!isOnTheGround)
        {
            transform.position = transform.position + new Vector3(0, -5, 0) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isOnTheGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isOnTheGround = false;
        }
    }

    public bool GetIsOnTheGround()
    {
        return isOnTheGround;
    }
}
