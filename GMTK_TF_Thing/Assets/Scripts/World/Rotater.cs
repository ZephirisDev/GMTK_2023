using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] public bool preyGoLeft;
    private bool hasPreyTriggered;
    private bool hasPredatorTriggered;

    public bool DoRotation(bool isPrey)
    {
        if (isPrey)
        {
            bool n = !hasPreyTriggered;
            hasPreyTriggered = true;
            return n;
        }
        bool m = !hasPredatorTriggered;
        hasPredatorTriggered = true;
        return m;
    }
}
