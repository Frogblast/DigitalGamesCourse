using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private TripwireTrigger tripwire;
    public float delay = 3f;
    private float countdown;
    public GameObject explosionEffect;


    // Start is called before the first frame update
    void Start()
    {
        tripwire.EnteredTrigger.AddListener(IgniteFuse);
        countdown = delay;
    }

    private void IgniteFuse()
    {
        Invoke("Explode", countdown);
        //Explode();
    }


    private void Explode()
    {
        Instantiate(explosionEffect,transform.position, transform.rotation);

        Debug.Log("Explosion again");

        Destroy(gameObject);
        // destory tripwire here with "Destory(gameObject)"
    }
}
