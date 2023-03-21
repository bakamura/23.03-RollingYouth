using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByTouch : MonoBehaviour
{
    [SerializeField] private float _dragTresHold;
    [SerializeField] private float _sensitivity;
    [SerializeField] private Transform _currentTarget;
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch input = Input.GetTouch(0);

            if (input.phase == TouchPhase.Moved && input.deltaPosition.magnitude > _dragTresHold && _currentTarget)
            {
                _currentTarget.eulerAngles += _sensitivity * new Vector3(0, input.deltaPosition.x, 0);
                //_currentTarget.Rotate(Vector3.up, _sensitivity,Space.Self);
            }
        }
    }
}
