using UnityEngine;

public class PowerSignalProcessor : MonoBehaviour
{
    [Header("Shaping")]
    [SerializeField] private float deadZone = 0.03f;
    [SerializeField] private float responseExponent = 1.2f;

    [Header("Smoothing")]
    [SerializeField] private float riseSpeed = 8f;
    [SerializeField] private float fallSpeed = 5f;

    private float smoothedPower;

    public float Process(float rawPower)
    {
        float normalized = Mathf.Clamp01(rawPower);
        float deNoised = normalized <= deadZone ? 0f : Mathf.InverseLerp(deadZone, 1f, normalized);
        float curved = Mathf.Pow(deNoised, Mathf.Max(0.01f, responseExponent));

        float speed = curved > smoothedPower ? riseSpeed : fallSpeed;
        smoothedPower = Mathf.MoveTowards(smoothedPower, curved, speed * Time.deltaTime);
        return smoothedPower;
    }

    public void ResetState()
    {
        smoothedPower = 0f;
    }
}
