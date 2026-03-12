using UnityEngine;

namespace SoundRacket
{
    public class KeyboardHoldInput : MonoBehaviour
    {
        [SerializeField] private KeyCode thrustKey = KeyCode.Space;

        public float GetHold01()
        {
            return Input.GetKey(thrustKey) ? 1f : 0f;
        }
    }
}
