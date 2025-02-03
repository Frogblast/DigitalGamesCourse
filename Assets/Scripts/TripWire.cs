using UnityEngine;

public class TripWire : MonoBehaviour
{
    private BulletSpawner shooter;

    private void Start()
    {
        shooter = GetComponentInChildren<BulletSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        shooter.Shoot(other.transform.position);
    }
}
