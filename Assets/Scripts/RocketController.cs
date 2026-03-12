using UnityEngine;

public class RocketController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 18f;
    [SerializeField] private float deceleration = 12f;

    private Vector3 startPosition;
    private float currentSpeed;

    public float CurrentHeight => Mathf.Max(0f, transform.position.y - startPosition.y);

    private void Awake()
    {
        startPosition = transform.position;
    }

    public void MoveUp(float power01)
    {
        float clampedPower = Mathf.Clamp01(power01);

        if (clampedPower > 0f)
        {
            currentSpeed += acceleration * clampedPower * Time.deltaTime;
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        transform.position += Vector3.up * (currentSpeed * Time.deltaTime);
    }

    public void ResetToStart()
    {
        transform.position = startPosition;
        currentSpeed = 0f;
    }
}
