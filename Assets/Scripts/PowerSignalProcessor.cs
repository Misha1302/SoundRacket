using UnityEngine;

public class PowerSignalProcessor : MonoBehaviour
{
    [SerializeField] private float deadZone = 0.05f;
    [SerializeField] private float responseExponent = 1.25f;
    [SerializeField] private float smoothingSpeed = 8f;

    private float smoothedPower;

    public float Process(float rawPower)
    {
        float normalized = Mathf.Clamp01(rawPower);
        float deNoised = normalized <= deadZone ? 0f : Mathf.InverseLerp(deadZone, 1f, normalized);
        float curved = Mathf.Pow(deNoised, Mathf.Max(0.01f, responseExponent));

        smoothedPower = Mathf.Lerp(smoothedPower, curved, smoothingSpeed * Time.deltaTime);
        return smoothedPower;
    }

    public void ResetState()
    {
        smoothedPower = 0f;
    }
}
