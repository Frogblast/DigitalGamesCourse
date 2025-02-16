using JetBrains.Annotations;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public Healthbar healthbar;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth; // Sets the health to a max value
        healthbar.SetMaxHealth(maxHealth);   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) // Just a quick way to insert self-harm >:)
        {
            TakeDamage(20);
        }    
    }

    private void TakeDamage(int amount)
    {
        if (currentHealth - amount > 0) // health shouldn't be negative
        {
            currentHealth -= amount; // Updates the health

            healthbar.SetHealth(currentHealth); // Updates the healthbar UI
        }
        else
        {
            currentHealth = 0;
            healthbar.SetHealth(0);
            Debug.Log("   Y O U   D I E D");
        }
    }
}