using UnityEngine;
using UnityEngine.UI;

public class RocketUiController : MonoBehaviour
{
    [SerializeField] private Slider powerSlider;
    [SerializeField] private Text powerText;
    [SerializeField] private Text resultText;

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
}
