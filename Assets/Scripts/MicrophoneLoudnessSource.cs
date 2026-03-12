using UnityEngine;

public class MicrophoneLoudnessSource : MonoBehaviour
{
    [SerializeField] private int sampleWindow = 256;
    [SerializeField] private float sensitivity = 20f;
    [SerializeField] private float smoothingSpeed = 8f;
    [SerializeField] private float noiseFloor = 0.01f;

    private AudioClip microphoneClip;
    private string selectedDevice;
    private float smoothedLevel;

    public bool IsReady => microphoneClip != null;
    public float ProcessedLevel => smoothedLevel;

    private void OnEnable()
    {
        StartMicrophone();
    }

    private void OnDisable()
    {
        StopMicrophone();
    }

    private void Update()
    {
        if (!IsReady)
        {
            smoothedLevel = Mathf.MoveTowards(smoothedLevel, 0f, smoothingSpeed * Time.deltaTime);
            return;
        }

        float rawLevel = ReadRmsLevel();
        float adjustedLevel = Mathf.Max(0f, rawLevel - noiseFloor) * sensitivity;
        float targetLevel = Mathf.Clamp01(adjustedLevel);

        smoothedLevel = Mathf.Lerp(smoothedLevel, targetLevel, smoothingSpeed * Time.deltaTime);
    }

    private void StartMicrophone()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogWarning("No microphone found. Microphone mode will stay at 0.");
            return;
        }

        selectedDevice = Microphone.devices[0];
        microphoneClip = Microphone.Start(selectedDevice, true, 1, AudioSettings.outputSampleRate);
    }

    private void StopMicrophone()
    {
        if (microphoneClip == null)
        {
            return;
        }

        Microphone.End(selectedDevice);
        microphoneClip = null;
        selectedDevice = null;
    }

    private float ReadRmsLevel()
    {
        int micPosition = Microphone.GetPosition(selectedDevice);
        if (micPosition <= 0)
        {
            return 0f;
        }

        int clippedWindow = Mathf.Min(sampleWindow, micPosition);
        float[] samples = new float[clippedWindow];
        microphoneClip.GetData(samples, micPosition - clippedWindow);

        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }

        return Mathf.Sqrt(sum / Mathf.Max(1, samples.Length));
    }
}
