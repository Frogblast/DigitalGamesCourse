using Unity.VisualScripting;
using UnityEngine;

public class DamageBase : MonoBehaviour, ITrapDamage // The base superclass for traps on how they deal damage to the player
{
    PlayerHealth playerhealth;

    public virtual int damageNr => 0; // The damage from trap Can be overwritten in subclass

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Character");
        playerhealth = player.GetComponent<PlayerHealth>();
    }

    public virtual void ApplyDamage(GameObject obj)
    {
        if (obj.CompareTag("Character")) // Makes sure there is a player to apply damage to
        { 
            playerhealth.TakeDamage(damageNr);
        }
    }
}

