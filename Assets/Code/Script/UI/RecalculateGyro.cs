using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecalculateGyro : MonoBehaviour
{
    [SerializeField] private MovmentByGyro _movmentByGyro;

    public void ResetGyro()
    {
        _movmentByGyro.ResetGyroPosition();
    }
}