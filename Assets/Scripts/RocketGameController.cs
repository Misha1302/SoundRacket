using UnityEngine;

public class RocketGameController : MonoBehaviour
{
    private enum InputMode
    {
        Keyboard,
        Microphone
    }

    [Header("References")]
    [SerializeField] private RocketController rocketController;
    [SerializeField] private RocketUiController uiController;
    [SerializeField] private PowerSignalProcessor signalProcessor;
    [SerializeField] private KeyboardHoldInput keyboardInput;
    [SerializeField] private MicrophoneLoudnessSource microphoneInput;

    [Header("Input")]
    [SerializeField] private InputMode inputMode = InputMode.Keyboard;

    [Header("Attempt")]
    [SerializeField] private KeyCode resetKey = KeyCode.R;

    private float maxHeight;

    private void Start()
    {
        ResetRun();
    }

    private void Update()
    {
        if (rocketController == null || uiController == null || signalProcessor == null)
        {
            return;
        }

        InputPowerSource activeInput = ResolveInputSource();
        if (activeInput == null)
        {
            uiController.Render(0f, rocketController.CurrentHeight, maxHeight);
            return;
        }

        float processedPower = signalProcessor.Process(activeInput.CurrentPower);
        rocketController.Simulate(processedPower);

        float currentHeight = rocketController.CurrentHeight;
        maxHeight = Mathf.Max(maxHeight, currentHeight);

        uiController.Render(processedPower, currentHeight, maxHeight);

        if (Input.GetKeyDown(resetKey))
        {
            ResetRun();
        }
    }

    public void ResetRun()
    {
        maxHeight = 0f;

        signalProcessor?.ResetState();
        keyboardInput?.ResetState();
        microphoneInput?.ResetState();
        rocketController?.ResetToStart();
        uiController?.Render(0f, 0f, 0f);
    }

    private InputPowerSource ResolveInputSource()
    {
        if (inputMode == InputMode.Microphone && microphoneInput != null)
        {
            return microphoneInput;
        }

        return keyboardInput;
    }
}
