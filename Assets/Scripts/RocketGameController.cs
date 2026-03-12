using UnityEngine;
using UnityEngine.UI;

public class RocketGameController : MonoBehaviour
{
    private enum ControlMode
    {
        Microphone,
        Keyboard
    }

    [Header("Mode")]
    [SerializeField] private ControlMode mode = ControlMode.Microphone;

    [Header("References")]
    [SerializeField] private RocketMotor rocketMotor;
    [SerializeField] private MicrophoneLoudnessSource microphoneInput;
    [SerializeField] private KeyboardHoldInput keyboardInput;

    [Header("UI")]
    [SerializeField] private Slider powerSlider;
    [SerializeField] private Text powerText;
    [SerializeField] private Text resultText;

    [Header("Result")]
    [SerializeField] private float scoreMultiplier = 100f;

    private Vector3 startPosition;

    private void Awake()
    {
        if (rocketMotor != null)
        {
            startPosition = rocketMotor.transform.position;
        }
    }

    private void Update()
    {
        if (rocketMotor == null)
        {
            return;
        }

        float thrust = ReadCurrentInput();
        rocketMotor.Tick(thrust);
        UpdateUi(thrust);

        if (Input.GetKeyDown(KeyCode.R))
        {
            rocketMotor.ResetFlight(startPosition);
        }
    }

    private float ReadCurrentInput()
    {
        if (mode == ControlMode.Keyboard)
        {
            return keyboardInput != null ? keyboardInput.ProcessedLevel : 0f;
        }

        return microphoneInput != null ? microphoneInput.ProcessedLevel : 0f;
    }

    private void UpdateUi(float thrust)
    {
        if (powerSlider != null)
        {
            powerSlider.value = thrust;
        }

        if (powerText != null)
        {
            powerText.text = $"Power: {Mathf.RoundToInt(thrust * 100f)}%";
        }

        if (resultText != null)
        {
            float score = rocketMotor.TotalDistance * scoreMultiplier;
            resultText.text = $"Score: {Mathf.RoundToInt(score)}";
        }
    }
}
