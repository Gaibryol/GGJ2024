using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorEvent : MonoBehaviour
{
    public class SprayPressed
    {
        public readonly bool ButtonHeld;

        public SprayPressed(bool buttonHeld)
        {
            ButtonHeld = buttonHeld;
        }
    }
}
