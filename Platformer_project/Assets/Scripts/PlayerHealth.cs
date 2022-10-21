using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerHealth : MonoBehaviour
{
    int maxHealth = 3;
    int currentHealth;
    [SerializeField] Text healthPointsText;
    [SerializeField] Text defeatText;
    [SerializeField] GameObject defeatCanvas;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject mainMenuButton;

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
        eventSystem.SetSelectedGameObject(mainMenuButton);
        Time.timeScale = 0;
        defeatCanvas.SetActive(true);
    }
}
