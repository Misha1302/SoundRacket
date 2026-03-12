using UnityEngine;
using UnityEngine.UI;

public class RocketUiController : MonoBehaviour
{
    [Header("Main UI")]
    [SerializeField] private Slider powerSlider;
    [SerializeField] private Text powerText;
    [SerializeField] private Text resultText;

    [Header("Debug Overlay")]
    [SerializeField] private bool showDebugOverlay = true;
    [SerializeField] private Text debugText;

    public void Render(string stateLabel, float power01, float currentHeight, float maxHeight, float remainingTime, float finalResult)
    {
        if (powerSlider != null)
        {
            powerSlider.value = power01;
        }

        if (powerText != null)
        {
            powerText.text = $"Power: {Mathf.RoundToInt(power01 * 100f)}%";
        }

        if (resultText != null)
        {
            resultText.text =
                $"State: {stateLabel}\n" +
                $"Height: {currentHeight:0.0} (Max: {maxHeight:0.0})\n" +
                $"Time Left: {remainingTime:0.0}s\n" +
                $"Final Result: {finalResult:0.0}";
        }
    }

    public void RenderDebug(
        float rawPower,
        float processedPower,
        float rocketVelocity,
        float currentHeight,
        float maxHeight,
        string modeLabel,
        string stateLabel)
    {
        if (debugText == null)
        {
            return;
        }

        if (!showDebugOverlay)
        {
            debugText.text = string.Empty;
            return;
        }

        debugText.text =
            $"[DEBUG]\n" +
            $"Mode: {modeLabel}\n" +
            $"Attempt: {stateLabel}\n" +
            $"Raw Power: {rawPower:0.000}\n" +
            $"Processed Power: {processedPower:0.000}\n" +
            $"Velocity Y: {rocketVelocity:0.00}\n" +
            $"Height: {currentHeight:0.00}\n" +
            $"Max Height: {maxHeight:0.00}";
    }
}
