using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumNextDirectionScript : MonoBehaviour
{
    public enum NextDirection
    {
        Forward,
        Backward,
        Left,
        Right
    }
    public NextDirection nextDirection;
}

