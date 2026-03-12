using UnityEngine;
using UnityEngine.UI;

public class RocketUiController : MonoBehaviour
{
    [SerializeField] private Slider powerSlider;
    [SerializeField] private Text powerText;
    [SerializeField] private Text resultText;

    public void Render(float power01, float currentResult, float maxResult)
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
            resultText.text = $"Height: {currentResult:0.0} / Best: {maxResult:0.0}";
        }
    }
}
