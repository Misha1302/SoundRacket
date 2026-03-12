using UnityEngine;

public abstract class InputPowerSource : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float debugPower;
    [SerializeField] private bool useDebugPower;

    public float CurrentPower { get; private set; }

    protected virtual void Update()
    {
        float rawPower = useDebugPower ? debugPower : ReadRawPower();
        CurrentPower = Mathf.Clamp01(rawPower);
    }

    protected abstract float ReadRawPower();

    public virtual void ResetState()
    {
        CurrentPower = 0f;
    }
}
