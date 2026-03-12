using UnityEngine;

public class KeyboardHoldInput : MonoBehaviour
{
    [SerializeField] private KeyCode thrustKey = KeyCode.Space;
    [SerializeField] private float riseSpeed = 6f;
    [SerializeField] private float fallSpeed = 4f;

    private float holdValue;

    public float ProcessedLevel => holdValue;

    private void Update()
    {
        float target = Input.GetKey(thrustKey) ? 1f : 0f;
        float speed = target > holdValue ? riseSpeed : fallSpeed;
        holdValue = Mathf.MoveTowards(holdValue, target, speed * Time.deltaTime);
    }
}
