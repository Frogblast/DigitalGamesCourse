using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LethalZoneTrigger : DamageBase
{

    [SerializeField] private int damageNrValue = 10000; // Allows damageNr to be viewd through the inspector
    public override int damageNr => damageNrValue; // Makes sure the player fully dies with high dmg output
    private Collider _collider; //  <<-- Does this do anything? Yes this is the "this" collider which is used together with "other" when checking for collisions


    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ApplyDamage(other.gameObject);
        /*if (other.gameObject.CompareTag("Character"))
        {
            Debug.Log("Player hit lethal zone");
            EventManager.TriggerPlayerDeath(); // Invoke the eventmanagers global event
        }*/
    }


    // for boulder collision, break this out in its own function if it breaks something else
    private void OnCollisionEnter(Collision collision)
    {
        ApplyDamage(collision.gameObject);
        /*
        if (collision.gameObject.CompareTag("Character"))
        {
            Debug.Log("Player collided with death");
            EventManager.TriggerPlayerDeath();
            
        }*/
    }
}
