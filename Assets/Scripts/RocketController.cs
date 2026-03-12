using UnityEngine;

public class RocketController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float thrustMultiplier = 18f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float maxUpwardSpeed = 12f;

    private Vector3 startPosition;
    private float verticalSpeed;

    public float CurrentHeight => Mathf.Max(0f, transform.position.y - startPosition.y);
    public float CurrentVerticalSpeed => verticalSpeed;

    private void Awake()
    {
        startPosition = transform.position;
    }

    public void Simulate(float processedPower01)
    {
        float power = Mathf.Clamp01(processedPower01);
        float acceleration = (power * thrustMultiplier) - gravity;

        verticalSpeed += acceleration * Time.deltaTime;
        verticalSpeed = Mathf.Clamp(verticalSpeed, 0f, maxUpwardSpeed);

        transform.position += Vector3.up * (verticalSpeed * Time.deltaTime);
    }

    public void ResetToStart()
    {
        transform.position = startPosition;
        verticalSpeed = 0f;
    }
}
