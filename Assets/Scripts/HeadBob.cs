using System.Collections;
using UnityEngine;

/*
 * Apply this to any fps camera which is the child of a playerPhysics gameobject to get a head bob effect.
 * Completely independent of any other script, component or gameobject.
 */
public class HeadBob : MonoBehaviour
{
    [SerializeField] private float idleAmplitude = 0.002f;
    [SerializeField] private float walkingAmplitudeMultiplier = 12f; // Multiplies both frequency and amplitude with this when walking
    [SerializeField] private float frequency = 1.5f;

    private Vector3 dynamicPosition = Vector3.zero;
    private bool isMoving = false;
    private bool isMovingY = false;
    float moveTimer;
    Vector3 offsetRelatativeToParent; // The distance between the camera transform and the playerPhysics
    private void FixedUpdate()
    {
        ApplyBob();
    }

    private void Start()
    {
        StartCoroutine(CheckForMovement());
        offsetRelatativeToParent = transform.position - transform.parent.position;
    }

    private IEnumerator CheckForMovement()
    {
        Vector2 lastPosition = new Vector2(transform.position.x, transform.position.z);
        float lastPositionY = transform.position.y;

        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.z);
            float currentPositionY = transform.position.y;
            isMoving = Vector2.Distance(lastPosition, currentPosition) > float.Epsilon;
            isMovingY = Mathf.Abs(lastPositionY - currentPositionY) > float.Epsilon;
            lastPosition = currentPosition;
            lastPositionY = currentPositionY;
        }
    }

    private void ApplyBob()
    {
        // Apply the bob effect only if not falling or jumping
        if (isMovingY) return;

        // If moving apply more bob effect
        float amountMultiplier = 1;
        if (isMoving)
        {
            moveTimer += Time.deltaTime;
            amountMultiplier = walkingAmplitudeMultiplier;
        }
        else
        {
            moveTimer = 0;
        }

        dynamicPosition.y = Mathf.Sin((float)Time.fixedTimeAsDouble * frequency * amountMultiplier) * idleAmplitude * amountMultiplier;
        transform.position += dynamicPosition;

        if (moveTimer <= float.Epsilon)
        {
            transform.position = Vector3.Lerp(transform.position, transform.parent.position + offsetRelatativeToParent, 0.05f); // (Slowly) Reset position if when standing still
        }
    }
}
