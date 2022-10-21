using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] GameObject player;
    private PlayerHealth playerHealth;
    [SerializeField] int damage = 1;

    private void Awake()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage(damage);
            ChangeColor();
            Invoke(nameof(ChangeColor), 0.25f);
        }
    }

    private void ChangeColor()
    {
        if (player.GetComponent<SpriteRenderer>().color == Color.red)
        {
            player.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            player.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
