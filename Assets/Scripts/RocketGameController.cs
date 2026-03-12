using UnityEngine;

public class RocketGameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RocketController rocketController;
    [SerializeField] private RocketUiController uiController;
    [SerializeField] private PowerSignalProcessor signalProcessor;
    [SerializeField] private KeyboardHoldInput keyboardInput;

    [Header("Attempt")]
    [SerializeField] private KeyCode resetKey = KeyCode.R;

    private float maxHeight;

    private void Start()
    {
        ResetRun();
    }

    private void Update()
    {
        if (rocketController == null || uiController == null || signalProcessor == null || keyboardInput == null)
        {
            return;
        }

        float processedPower = signalProcessor.Process(keyboardInput.CurrentPower);
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
        rocketController?.ResetToStart();
        uiController?.Render(0f, 0f, 0f);
    }
}
