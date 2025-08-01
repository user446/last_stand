using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Settings
{
    [CreateAssetMenu(fileName = "InputControlSettings", menuName = "Scriptable Objects/InputControlSettings")]
    public class InputControlSettings : ScriptableObject
    {
        public InputActionProperty MoveAction;
        public InputActionProperty LookAction; 
    }
}