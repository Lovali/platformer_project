using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    int maxHealth = 3;
    int currentHealth;
    [SerializeField] Text healthPointsText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthPointsText.text = currentHealth.ToString();
    }
}
