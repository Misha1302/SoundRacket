using UnityEngine;

namespace SoundRacket
{
    public class RocketGameController : MonoBehaviour
    {
        private enum ControlMode
        {
            Microphone = 0,
            Keyboard = 1
        }

        [Header("Mode")]
        [SerializeField] private ControlMode controlMode = ControlMode.Microphone;
        [SerializeField] private KeyCode switchModeKey = KeyCode.Tab;
        [SerializeField] private KeyCode resetKey = KeyCode.R;

        [Header("Inputs")]
        [SerializeField] private MicrophoneLoudnessInput microphoneInput;
        [SerializeField] private KeyboardHoldInput keyboardInput;
        [SerializeField] private SignalSmoother inputSmoother = new SignalSmoother();

        [Header("Gameplay")]
        [SerializeField] private RocketFlightController rocketFlight;
        [SerializeField, Min(0f)] private float scorePerSecondAtFullPower = 10f;

        [Header("UI")]
        [SerializeField] private FlightAttemptUI ui;

        private float score;

        private void Update()
        {
            if (Input.GetKeyDown(switchModeKey))
            {
                controlMode = controlMode == ControlMode.Microphone ? ControlMode.Keyboard : ControlMode.Microphone;
                inputSmoother.Reset();
            }

            if (Input.GetKeyDown(resetKey))
            {
                ResetAttempt();
            }

            var rawInput = ReadRawInput();
            var processedInput = inputSmoother.Update(rawInput, Time.deltaTime);

            if (rocketFlight != null)
            {
                rocketFlight.Tick(processedInput, Time.deltaTime);
            }

            score += processedInput * scorePerSecondAtFullPower * Time.deltaTime;

            if (ui != null)
            {
                ui.SetValues(processedInput, score, controlMode == ControlMode.Microphone ? "Микрофон" : "Клавиатура");
            }
        }

        private float ReadRawInput()
        {
            if (controlMode == ControlMode.Microphone)
            {
                return microphoneInput != null ? microphoneInput.GetLoudness01() : 0f;
            }

            return keyboardInput != null ? keyboardInput.GetHold01() : 0f;
        }

        private void ResetAttempt()
        {
            score = 0f;
            inputSmoother.Reset();

            if (rocketFlight != null)
            {
                rocketFlight.ResetFlight();
            }

            if (microphoneInput != null && controlMode == ControlMode.Microphone)
            {
                microphoneInput.RestartMicrophone();
            }
        }
    }
}
