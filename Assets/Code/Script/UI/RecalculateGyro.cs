using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecalculateGyro : MonoBehaviour
{
    [SerializeField] private MovmentByGyro _movmentByGyro;

    public void ResetGyro()
    {
#if UNITY_EDITOR
        Debug.Log("reset gyro");
#endif
        _movmentByGyro.ResetGyroPosition();
    }
}