using UnityEngine;

public class PowerSignalProcessor : MonoBehaviour
{
    [Header("Noise Gate")]
    [Tooltip("When enabled, tiny values under Noise Threshold are treated as 0.")]
    [SerializeField] private bool useNoiseThreshold = true;
    [Tooltip("Minimum normalized input required before the rocket responds.")]
    [Range(0f, 1f)]
    [SerializeField] private float noiseThreshold = 0.03f;

    [Header("Smoothing")]
    [Tooltip("How quickly processed power rises toward higher input values.")]
    [SerializeField] private float riseSpeed = 8f;
    [Tooltip("How quickly processed power falls toward lower input values.")]
    [SerializeField] private float fallSpeed = 5f;

    private float smoothedPower;

    public float Process(float rawPower)
    {
        float normalized = Mathf.Clamp01(rawPower);
        float thresholded = ApplyNoiseThreshold(normalized);

        float speed = thresholded > smoothedPower ? riseSpeed : fallSpeed;
        smoothedPower = Mathf.MoveTowards(smoothedPower, thresholded, speed * Time.deltaTime);
        return smoothedPower;
    }

    private float ApplyNoiseThreshold(float normalizedPower)
    {
        if (!useNoiseThreshold)
        {
            return normalizedPower;
        }

        if (normalizedPower <= noiseThreshold)
        {
            return 0f;
        }

        return Mathf.InverseLerp(noiseThreshold, 1f, normalizedPower);
    }

    public void ResetState()
    {
        smoothedPower = 0f;
    }
}
