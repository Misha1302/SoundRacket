using UnityEngine;

public class RocketGameController : MonoBehaviour
{
    private enum InputMode
    {
        Keyboard,
        Microphone
    }

    [Header("Mode")]
    [SerializeField] private InputMode inputMode = InputMode.Keyboard;

    [Header("References")]
    [SerializeField] private RocketController rocketController;
    [SerializeField] private RocketUiController uiController;
    [SerializeField] private PowerSignalProcessor signalProcessor;
    [SerializeField] private KeyboardHoldInput keyboardInput;
    [SerializeField] private MicrophoneLoudnessSource microphoneInput;

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

        float rawPower = GetActiveInputPower();
        float processedPower = signalProcessor.Process(rawPower);

        rocketController.MoveUp(processedPower);

        float currentHeight = rocketController.CurrentHeight;
        maxHeight = Mathf.Max(maxHeight, currentHeight);

        uiController.Render(processedPower, currentHeight, maxHeight);

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetRun();
        }
    }

    private float GetActiveInputPower()
    {
        InputPowerSource source = inputMode == InputMode.Keyboard
            ? keyboardInput
            : microphoneInput;

        return source != null ? source.CurrentPower : 0f;
    }

    private void ResetRun()
    {
        maxHeight = 0f;

        if (signalProcessor != null)
        {
            signalProcessor.ResetState();
        }

        if (rocketController != null)
        {
            rocketController.ResetToStart();
        }

        if (uiController != null)
        {
            uiController.Render(0f, 0f, 0f);
        }
    }
}
