using UnityEngine;

namespace SoundRacket
{
    [System.Serializable]
    public class SignalSmoother
    {
        [SerializeField] private float attackSpeed = 3f;
        [SerializeField] private float releaseSpeed = 2f;

        private float currentValue;

        public float CurrentValue => currentValue;

        public float Update(float targetValue, float deltaTime)
        {
            var speed = targetValue > currentValue ? attackSpeed : releaseSpeed;
            currentValue = Mathf.MoveTowards(currentValue, targetValue, speed * deltaTime);
            return currentValue;
        }

        public void Reset()
        {
            currentValue = 0f;
        }
    }
}
