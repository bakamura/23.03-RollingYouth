using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlers : BaseSingleton<PlayerControlers>
{
    [SerializeField] private MovmentByGyro _movmentByGyro;
    [SerializeField] private MovmentBySwipe _movmentBySwipe;
    private static bool _isGyroActive = true;
    private static bool _isSwipeActive;

    public enum ControlTypes
    {
        GYROSCOPE,
        SWIPE
    }

    protected override void Awake()
    {
        base.Awake();
        UpdateInputs();
    }

    public void UpdateControlers()
    {
        _isGyroActive = !_isGyroActive;
        _isSwipeActive = !_isSwipeActive;
        UpdateInputs();
    }

    private void UpdateInputs()
    {
        if (_movmentByGyro) _movmentByGyro.IsGyroActive = _isGyroActive;
        if (_movmentBySwipe) _movmentBySwipe.IsSwipeActive = _isSwipeActive;
        Debug.Log($"gyro is {_isGyroActive}, swipe is {_isSwipeActive}");
    }
}
