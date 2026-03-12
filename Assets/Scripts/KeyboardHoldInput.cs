using UnityEngine;

public class KeyboardHoldInput : InputPowerSource
{
    [SerializeField] private KeyCode thrustKey = KeyCode.Space;
    [SerializeField] private float riseSpeed = 6f;
    [SerializeField] private float fallSpeed = 4f;

    private float holdValue;

    protected override float ReadRawPower()
    {
        float target = Input.GetKey(thrustKey) ? 1f : 0f;
        float speed = target > holdValue ? riseSpeed : fallSpeed;
        holdValue = Mathf.MoveTowards(holdValue, target, speed * Time.deltaTime);
        return holdValue;
    }
}
