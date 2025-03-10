using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField]
    Bullet bulletPrefab;

    internal void Shoot(Vector3 targetPosition)
    {
        // Instantiate the bullet at the spawner's position with the correct rotation
        Vector3 verticalDirectionOffset = new Vector3(0, 0.7f, 0);
        Bullet bullet = Instantiate(bulletPrefab, this.transform.position + verticalDirectionOffset, Quaternion.LookRotation(targetPosition - this.transform.position));

        bullet.Move(targetPosition - this.transform.position);
    }

}
