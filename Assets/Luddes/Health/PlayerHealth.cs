using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public Healthbar healthbar;
    [SerializeField] private int maxHealth;
    private int currentHealth;
    
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

    public void TakeDamage(int amount) // Typically a player might want to off-themselves through a console command, hence being public
    {

        amount = Mathf.Abs(amount); // make sure the amount of damage taken is non-negative.
        if (currentHealth - amount > 0) // health shouldn't be negative
        {
            currentHealth -= amount; // Updates the health

            healthbar.SetHealth(currentHealth); // Updates the healthbar UI
        }
        else
        {
            currentHealth = 0;
            healthbar.SetHealth(0);
            EventManager.TriggerPlayerDeath();
        }
    }
}