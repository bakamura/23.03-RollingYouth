using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlers : BaseSingleton<PlayerControlers>
{
    [SerializeField] private MovmentByGyro _movmentByGyro;
    [SerializeField] private MovmentBySwipe _movmentBySwipe;

    public void UpdateControlers()
    {
        _movmentByGyro.IsGyroActive = !_movmentByGyro.IsGyroActive;
        _movmentBySwipe.IsSwipeActive = !_movmentBySwipe.IsSwipeActive;
    }
}
