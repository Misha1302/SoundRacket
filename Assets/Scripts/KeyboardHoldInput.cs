using UnityEngine;

public class KeyboardHoldInput : InputPowerSource
{
    [SerializeField] private KeyCode thrustKey = KeyCode.Space;
    [SerializeField] private float riseSpeed = 6f;
    [SerializeField] private float fallSpeed = 4f;
    [Range(0f, 1f)]
    [SerializeField] private float maxPower = 1f;

    private float holdValue;

    protected override float ReadRawPower()
    {
        float target = Input.GetKey(thrustKey) ? maxPower : 0f;
        float speed = target > holdValue ? riseSpeed : fallSpeed;
        holdValue = Mathf.MoveTowards(holdValue, target, speed * Time.deltaTime);
        return holdValue;
    }

    public void ResetState()
    {
        holdValue = 0f;
    }
}
