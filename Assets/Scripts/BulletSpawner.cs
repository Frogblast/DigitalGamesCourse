using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField]
    Bullet bulletPrefab;

    internal void Shoot(Vector3 targetPosition)
    {
        // Instantiate the bullet at the spawner's position with the correct rotation
        Bullet bullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.LookRotation(targetPosition - this.transform.position));

        bullet.Move(targetPosition - this.transform.position);
    }

}
