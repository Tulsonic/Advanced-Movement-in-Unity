using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStartingPosition : MonoBehaviour
{
    [HideInInspector] public Vector3 startingPoint;
    [HideInInspector] public Vector3 startingRotation;

    void Start()
    {
        startingPoint = GetComponent<Transform>().localPosition;
        startingRotation = GetComponent<Transform>().localEulerAngles;
    }
}
