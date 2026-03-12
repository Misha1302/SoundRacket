using UnityEngine;
using UnityEngine.UI;

namespace SoundRacket
{
    public class FlightAttemptUI : MonoBehaviour
    {
        [SerializeField] private Slider powerSlider;
        [SerializeField] private Text powerText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text modeText;

        public void SetValues(float power01, float score, string modeName)
        {
            if (powerSlider != null)
            {
                powerSlider.value = power01;
            }

            if (powerText != null)
            {
                powerText.text = $"Сила: {Mathf.RoundToInt(power01 * 100f)}%";
            }

            if (scoreText != null)
            {
                scoreText.text = $"Результат: {score:F1}";
            }

            if (modeText != null)
            {
                modeText.text = $"Режим: {modeName}";
            }
        }
    }
}
