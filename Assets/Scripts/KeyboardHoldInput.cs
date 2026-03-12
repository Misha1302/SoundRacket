using UnityEngine;

public class KeyboardHoldInput : InputPowerSource
{
    [SerializeField] private KeyCode thrustKey = KeyCode.Space;
    [Range(0f, 1f)]
    [SerializeField] private float maxPower = 1f;

    protected override float ReadRawPower()
    {
        return Input.GetKey(thrustKey) ? maxPower : 0f;
    }
}
