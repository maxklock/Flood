namespace Flood.Utils
{
    using System.Collections.Generic;

    using UnityEngine;

    public class AxisToButtonUtil
    {
        public static AxisToButtonUtil Instance { get; } = new AxisToButtonUtil();

        private readonly Dictionary<string, bool> _pressedValues = new Dictionary<string, bool>();

        public float Threshold = 0.7f;

        public bool IsPressed(string axis)
        {
            return Input.GetAxis(axis) > Threshold;
        }

        public bool IsDown(string axis)
        {
            var isPressed = IsPressed(axis);
            if (!_pressedValues.ContainsKey(axis))
            {
                _pressedValues.Add(axis, isPressed);
            }
            var oldValue = _pressedValues[axis];

            _pressedValues[axis] = isPressed;

            return isPressed && !oldValue;
        }

        public bool IsUp(string axis)
        {
            var isPressed = IsPressed(axis);
            if (!_pressedValues.ContainsKey(axis))
            {
                _pressedValues.Add(axis, isPressed);
            }
            var oldValue = _pressedValues[axis];

            _pressedValues[axis] = isPressed;

            return !isPressed && oldValue;
        }
    }
}