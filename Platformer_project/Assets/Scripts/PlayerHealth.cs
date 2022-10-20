using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    int maxHealth = 3;
    int currentHealth;
    [SerializeField] Text healthPointsText;
    [SerializeField] Text defeatText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthPointsText.text = currentHealth.ToString();
    }

    void Die()
    {
        defeatText.text = "You are dead...";
        defeatText.gameObject.SetActive(true);
    }
}
