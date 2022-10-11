using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] int damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
