using UnityEngine;

namespace SoundRacket
{
    public class MicrophoneLoudnessInput : MonoBehaviour
    {
        [SerializeField] private string microphoneName = string.Empty;
        [SerializeField, Min(64)] private int sampleWindow = 256;
        [SerializeField, Min(0.0001f)] private float referenceLevel = 0.01f;
        [SerializeField, Min(1f)] private float maxDecibels = 30f;

        private AudioClip microphoneClip;
        private readonly float[] sampleBuffer = new float[2048];

        private void OnEnable()
        {
            StartMicrophone();
        }

        private void OnDisable()
        {
            StopMicrophone();
        }

        public float GetLoudness01()
        {
            if (microphoneClip == null || !Microphone.IsRecording(microphoneName))
            {
                return 0f;
            }

            var position = Microphone.GetPosition(microphoneName);
            if (position <= 0)
            {
                return 0f;
            }

            var samplesToRead = Mathf.Min(sampleWindow, sampleBuffer.Length);
            var start = position - samplesToRead;
            if (start < 0)
            {
                return 0f;
            }

            microphoneClip.GetData(sampleBuffer, start);

            float sumSquares = 0f;
            for (int i = 0; i < samplesToRead; i++)
            {
                var sample = sampleBuffer[i];
                sumSquares += sample * sample;
            }

            var rms = Mathf.Sqrt(sumSquares / samplesToRead);
            var decibels = 20f * Mathf.Log10(Mathf.Max(rms / referenceLevel, 0.0001f));
            return Mathf.Clamp01(decibels / maxDecibels);
        }

        public void RestartMicrophone()
        {
            StopMicrophone();
            StartMicrophone();
        }

        private void StartMicrophone()
        {
            if (Microphone.devices.Length == 0)
            {
                Debug.LogWarning("No microphone devices found.");
                return;
            }

            if (string.IsNullOrWhiteSpace(microphoneName))
            {
                microphoneName = Microphone.devices[0];
            }

            microphoneClip = Microphone.Start(microphoneName, true, 1, 44100);
            if (microphoneClip == null)
            {
                Debug.LogWarning($"Failed to start microphone: {microphoneName}");
            }
        }

        private void StopMicrophone()
        {
            if (string.IsNullOrWhiteSpace(microphoneName))
            {
                return;
            }

            if (Microphone.IsRecording(microphoneName))
            {
                Microphone.End(microphoneName);
            }

            microphoneClip = null;
        }
    }
}
