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

    [Header("Attempt")]
    [SerializeField] private KeyCode startAttemptKey = KeyCode.Space;
    [SerializeField] private KeyCode resetKey = KeyCode.R;
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

        if (Input.GetKeyDown(startAttemptKey))
        {
            StartAttempt();
        }
    }

    private void HandleActiveState()
    {
        InputPowerSource activeInput = ResolveInputSource();
        float processedPower = activeInput == null ? 0f : signalProcessor.Process(activeInput.CurrentPower);

        rocketController.Simulate(processedPower);

        float currentHeight = rocketController.CurrentHeight;
        maxHeight = Mathf.Max(maxHeight, currentHeight);
        remainingAttemptTime = Mathf.Max(0f, remainingAttemptTime - Time.deltaTime);

        uiController.Render(attemptState.ToString(), processedPower, currentHeight, maxHeight, remainingAttemptTime, 0f);

        if (remainingAttemptTime <= 0f)
        {
            EndAttempt();
        }
    }

    private void HandleResultState()
    {
        resultTimer += Time.deltaTime;
        uiController.Render(attemptState.ToString(), 0f, rocketController.CurrentHeight, maxHeight, 0f, resultHeight);

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
