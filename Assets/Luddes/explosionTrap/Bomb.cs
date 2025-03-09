using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : DamageBase
{
    [SerializeField] private TripwireTrigger tripwire;
    [SerializeField] private GameObject explosionEffect;

    private AudioManager audiomanager => AudioManager.Instance;

    private KinematicCharacterMotor movescript;

    [Header("Bomb settings")]
    public int damage = 40; // The explosion damage on the player (or anything with health)
    public float force = 650f; // The pushback of the explosion
    public float delay = 1f; // For when the explosion is triggered
    public float radius = 5f; // The radius of the explosion
    public float knockDuration = 1f; // Time in seconds the player is knocked

    private float countdown;
    private bool isKnockedBack = false;
    public override int damageNr => damage;


    void Start()
    {

        tripwire.EnteredTrigger.AddListener(IgniteFuse); // Adds IgniteFuse as a listener to the tripwire trigger
        countdown = delay;
    }

    private void DisableMove(bool disable)
    {
        if (disable)
        {
            movescript.enabled = false;
        }
        else 
        {
            movescript.enabled = true;
        }
    }



    private void IgniteFuse() // Starts the countdown of the explosion, "ignites the fuse"
    {
        StartCoroutine(DelayedExplosion());
    }


    private IEnumerator DelayedExplosion() // Delay before explosion
    {
        yield return new WaitForSeconds(countdown);
        StartCoroutine(Explode());
    }



    // Great debug tool for visualizing the explosion radius
    /*public Color gizmoColor = Color.red; // Color of the debug sphere
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radius); // Draws the explosion radius
    }*/


    IEnumerator Explode() // Handles the explosion
    {
        Instantiate(explosionEffect,transform.position, transform.rotation); // Spawns a explosion particle effect
        audiomanager.Play("TripwireExplosion");


        Collider[] colliders = Physics.OverlapSphere(transform.position, radius); // A list of every collider within a sphere (explosion radius)

        foreach(Collider nearbyObject in colliders) // Goes through all objects within the sphere
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            if (nearbyObject.CompareTag("Character") && rb != null) // If the player is inside the explosion, code bellow transpires
            {
                movescript = nearbyObject.GetComponent<KinematicCharacterMotor>(); // Retrieves this so it can be deactivated for knockback effect.
                DisableMove(true); // Disables the movement for knockback effect

                rb.AddExplosionForce(force, transform.position, radius); // Knockback force applied to player

                ApplyDamage(nearbyObject.gameObject); // If the player is inside the explosion, apply damage

                yield return new WaitForSeconds(knockDuration); // Duration until player regains controll
                
                
                movescript.SetPosition(rb.position); // Updates the new position
                DisableMove(false); // Player regains controll

                break; // because we found the player
            }
        }
        


        Destroy(transform.parent.gameObject); // Removes tripwire and bomb by accessing the parent and removing it.
    }
}
