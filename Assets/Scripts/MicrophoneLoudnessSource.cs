using UnityEngine;

public class MicrophoneLoudnessSource : InputPowerSource
{
    [Header("Microphone")]
    [SerializeField] private int requestedSampleRate = 44100;
    [SerializeField] private int sampleWindowSize = 256;

    [Header("Tuning")]
    [SerializeField] private float microphoneSensitivity = 25f;
    [Range(0f, 1f)]
    [SerializeField] private float noiseThreshold = 0.01f;
    [SerializeField] private float smoothing = 12f;

    [Header("Fallback")]
    [SerializeField] private bool logWarnings = true;

    private AudioClip microphoneClip;
    private string selectedDevice;
    private float[] sampleBuffer;
    private float smoothedPower;
    private bool hasWarnedNoDevice;
    private bool hasWarnedNoPermission;

    private void OnEnable()
    {
        StartMicrophone();
    }

    private void OnDisable()
    {
        StopMicrophone();
    }

    protected override float ReadRawPower()
    {
        if (microphoneClip == null)
        {
            TryWarnAboutUnavailableMicrophone();
            smoothedPower = Mathf.MoveTowards(smoothedPower, 0f, smoothing * Time.deltaTime);
            return smoothedPower;
        }

        int micPosition = Microphone.GetPosition(selectedDevice);
        if (micPosition <= 0)
        {
            return 0f;
        }

        int safeWindow = Mathf.Clamp(sampleWindowSize, 64, microphoneClip.samples);
        EnsureSampleBufferSize(safeWindow);

        int offset = micPosition - safeWindow;
        if (offset < 0)
        {
            offset += microphoneClip.samples;
        }

        microphoneClip.GetData(sampleBuffer, offset);

        float peakAmplitude = 0f;
        for (int i = 0; i < safeWindow; i++)
        {
            float amplitude = Mathf.Abs(sampleBuffer[i]);
            if (amplitude > peakAmplitude)
            {
                peakAmplitude = amplitude;
            }
        }

        float gated = peakAmplitude <= noiseThreshold ? 0f : peakAmplitude - noiseThreshold;
        float normalized = Mathf.Clamp01(gated * microphoneSensitivity);
        smoothedPower = Mathf.MoveTowards(smoothedPower, normalized, smoothing * Time.deltaTime);

        return smoothedPower;
    }

    public override void ResetState()
    {
        base.ResetState();
        smoothedPower = 0f;
    }

    private void StartMicrophone()
    {
        if (Microphone.devices == null || Microphone.devices.Length == 0)
        {
            return;
        }

        selectedDevice = Microphone.devices[0];
        microphoneClip = Microphone.Start(selectedDevice, true, 1, requestedSampleRate);

        if (microphoneClip == null)
        {
            TryWarnAboutUnavailableMicrophone();
            return;
        }

        hasWarnedNoDevice = false;
        hasWarnedNoPermission = false;
    }

    private void StopMicrophone()
    {
        if (!string.IsNullOrEmpty(selectedDevice) && Microphone.IsRecording(selectedDevice))
        {
            Microphone.End(selectedDevice);
        }

        microphoneClip = null;
    }

    private void EnsureSampleBufferSize(int size)
    {
        if (sampleBuffer == null || sampleBuffer.Length != size)
        {
            sampleBuffer = new float[size];
        }
    }

    private void TryWarnAboutUnavailableMicrophone()
    {
        if (!logWarnings)
        {
            return;
        }

        bool hasDevice = Microphone.devices != null && Microphone.devices.Length > 0;
        if (!hasDevice)
        {
            if (!hasWarnedNoDevice)
            {
                Debug.LogWarning("Microphone mode is active but no microphone device is available. Input power will stay at 0.");
                hasWarnedNoDevice = true;
            }

            return;
        }

        if (!hasWarnedNoPermission)
        {
            Debug.LogWarning("Microphone mode could not start recording. Check microphone permission in OS/Unity player settings.");
            hasWarnedNoPermission = true;
        }
    }
}
