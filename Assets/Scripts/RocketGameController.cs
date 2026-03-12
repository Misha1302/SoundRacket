using UnityEngine;

public class RocketGameController : MonoBehaviour
{
    private enum InputMode
    {
        Keyboard,
        Microphone
    }

    private enum AttemptState
    {
        Ready,
        Active,
        Result
    }

    [Header("References")]
    [SerializeField] private RocketController rocketController;
    [SerializeField] private RocketUiController uiController;
    [SerializeField] private PowerSignalProcessor signalProcessor;
    [SerializeField] private KeyboardHoldInput keyboardInput;
    [SerializeField] private MicrophoneLoudnessSource microphoneInput;

    [Header("Input")]
    [SerializeField] private InputMode inputMode = InputMode.Keyboard;

    [Header("Attempt Controls")]
    [SerializeField] private KeyCode startAttemptKey = KeyCode.Space;
    [SerializeField] private KeyCode resetKey = KeyCode.R;

    [Header("Attempt Timing")]
    [SerializeField] private float attemptDurationSeconds = 8f;
    [SerializeField] private bool autoResetAfterResult;
    [SerializeField] private float autoResetDelaySeconds = 2f;

    private AttemptState attemptState;
    private float remainingAttemptTime;
    private float resultHeight;
    private float maxHeight;
    private float resultTimer;

    private void Start()
    {
        ResetForNextAttempt();
    }

    private void Update()
    {
        if (rocketController == null || uiController == null || signalProcessor == null)
        {
            return;
        }

        if (Input.GetKeyDown(resetKey))
        {
            ResetForNextAttempt();
        }

        if (attemptState == AttemptState.Ready)
        {
            HandleReadyState();
            return;
        }

        if (attemptState == AttemptState.Active)
        {
            HandleActiveState();
            return;
        }

        HandleResultState();
    }

    private void HandleReadyState()
    {
        uiController.Render(attemptState.ToString(), 0f, 0f, 0f, attemptDurationSeconds, 0f);
        RenderDebugValues(0f, 0f, 0f, 0f);

        if (Input.GetKeyDown(startAttemptKey))
        {
            StartAttempt();
        }
    }

    private void HandleActiveState()
    {
        InputPowerSource activeInput = ResolveInputSource();
        float rawPower = activeInput == null ? 0f : activeInput.CurrentPower;
        float processedPower = signalProcessor.Process(rawPower);

        rocketController.Simulate(processedPower);

        float currentHeight = rocketController.CurrentHeight;
        maxHeight = Mathf.Max(maxHeight, currentHeight);
        remainingAttemptTime = Mathf.Max(0f, remainingAttemptTime - Time.deltaTime);

        uiController.Render(attemptState.ToString(), processedPower, currentHeight, maxHeight, remainingAttemptTime, 0f);
        RenderDebugValues(rawPower, processedPower, currentHeight, maxHeight);

        if (remainingAttemptTime <= 0f)
        {
            EndAttempt();
        }
    }

    private void HandleResultState()
    {
        resultTimer += Time.deltaTime;
        float currentHeight = rocketController.CurrentHeight;

        uiController.Render(attemptState.ToString(), 0f, currentHeight, maxHeight, 0f, resultHeight);
        RenderDebugValues(0f, 0f, currentHeight, maxHeight);

        if (Input.GetKeyDown(startAttemptKey))
        {
            StartAttempt();
            return;
        }

        if (autoResetAfterResult && resultTimer >= autoResetDelaySeconds)
        {
            ResetForNextAttempt();
        }
    }

    private void RenderDebugValues(float rawPower, float processedPower, float currentHeight, float currentMaxHeight)
    {
        uiController.RenderDebug(
            rawPower,
            processedPower,
            rocketController.CurrentVerticalSpeed,
            currentHeight,
            currentMaxHeight,
            inputMode.ToString(),
            attemptState.ToString());
    }

    private void StartAttempt()
    {
        attemptState = AttemptState.Active;
        remainingAttemptTime = Mathf.Max(0.1f, attemptDurationSeconds);
        resultHeight = 0f;
        maxHeight = 0f;
        resultTimer = 0f;

        signalProcessor.ResetState();
        keyboardInput?.ResetState();
        microphoneInput?.ResetState();
        rocketController.ResetToStart();
    }

    private void EndAttempt()
    {
        attemptState = AttemptState.Result;
        resultHeight = maxHeight;
        resultTimer = 0f;

        signalProcessor.ResetState();
        keyboardInput?.ResetState();
        microphoneInput?.ResetState();
    }

    public void ResetForNextAttempt()
    {
        attemptState = AttemptState.Ready;
        remainingAttemptTime = Mathf.Max(0.1f, attemptDurationSeconds);
        resultHeight = 0f;
        maxHeight = 0f;
        resultTimer = 0f;

        signalProcessor?.ResetState();
        keyboardInput?.ResetState();
        microphoneInput?.ResetState();
        rocketController?.ResetToStart();
        uiController?.Render(attemptState.ToString(), 0f, 0f, 0f, remainingAttemptTime, 0f);
        uiController?.RenderDebug(0f, 0f, 0f, 0f, 0f, inputMode.ToString(), attemptState.ToString());
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
