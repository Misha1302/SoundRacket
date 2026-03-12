using UnityEngine;

namespace SoundRacket
{
    public class RocketFlightController : MonoBehaviour
    {
        [SerializeField] private Transform rocket;
        [SerializeField, Min(0f)] private float maxUpSpeed = 6f;
        [SerializeField, Min(0f)] private float acceleration = 10f;
        [SerializeField, Min(0f)] private float deceleration = 6f;

        private Vector3 startPosition;
        private float currentSpeed;

        private void Awake()
        {
            if (rocket == null)
            {
                rocket = transform;
            }

            startPosition = rocket.position;
        }

        public void Tick(float thrust01, float deltaTime)
        {
            var targetSpeed = thrust01 * maxUpSpeed;
            var speedStep = targetSpeed > currentSpeed ? acceleration : deceleration;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, speedStep * deltaTime);

            rocket.position += Vector3.up * (currentSpeed * deltaTime);
        }

        public void ResetFlight()
        {
            currentSpeed = 0f;
            rocket.position = startPosition;
        }
    }
}
