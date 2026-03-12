using UnityEngine;

public class MicrophoneLoudnessSource : InputPowerSource
{
    [Header("Microphone")]
    [SerializeField] private int requestedSampleRate = 44100;
    [SerializeField] private int sampleWindowSize = 256;

    [Header("Input Scaling")]
    [Tooltip("Scales measured microphone amplitude into a usable 0..1 gameplay input range.")]
    [SerializeField] private float inputGain = 25f;

    [Header("Fallback")]
    [SerializeField] private bool logWarnings = true;

    private AudioClip microphoneClip;
    private string selectedDevice;
    private float[] sampleBuffer;
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
            return 0f;
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

        return Mathf.Clamp01(peakAmplitude * inputGain);
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
