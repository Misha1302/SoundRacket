using UnityEngine;

public class MicrophoneLoudnessSource : InputPowerSource
{
    [Header("Placeholder stub")]
    [SerializeField] private float simulatedPower = 0.35f;
    [SerializeField] private KeyCode boostKey = KeyCode.LeftShift;

    protected override float ReadRawPower()
    {
        if (Input.GetKey(boostKey))
        {
            return 1f;
        }

        return simulatedPower;
    }
}
