using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    [SerializeField] public Direction direction;
}

public enum Direction
{
    Forward, Left, Right
}