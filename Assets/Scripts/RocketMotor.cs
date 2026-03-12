using UnityEngine;

public class RocketMotor : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 16f;
    [SerializeField] private float deceleration = 10f;

    private float currentSpeed;
    private float totalDistance;

    public float CurrentSpeed => currentSpeed;
    public float TotalDistance => totalDistance;

    public void Tick(float thrust01)
    {
        thrust01 = Mathf.Clamp01(thrust01);

        if (thrust01 > 0f)
        {
            currentSpeed += acceleration * thrust01 * Time.deltaTime;
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

        float move = currentSpeed * Time.deltaTime;
        transform.position += Vector3.up * move;
        totalDistance += move;
    }

    public void ResetFlight(Vector3 startPosition)
    {
        transform.position = startPosition;
        currentSpeed = 0f;
        totalDistance = 0f;
    }
}
