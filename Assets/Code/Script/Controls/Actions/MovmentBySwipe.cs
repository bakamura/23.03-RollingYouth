using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovmentBySwipe : BaseActions
{
    [SerializeField] private float _dragTresHold;
    [SerializeField] private Transform _cameraRotation;

    public static bool IsSwipeActive = false;

    private void Awake()
    {
        SetTarget(transform, GetComponent<Rigidbody>());
    }

    public override void ExecuteAction()
    {
        if(IsSwipeActive)SwipeMovment();
    }

    private void SwipeMovment()
    {
        if (Input.touchCount > 0)
        {
            Touch input = Input.GetTouch(0);

            if (input.phase == TouchPhase.Moved && input.deltaPosition.magnitude > _dragTresHold && _currentTarget)
            {
                Vector3 direction = new Vector3(input.deltaPosition.y, 0, -input.deltaPosition.x).normalized;
                Vector3 a = Vector3.RotateTowards(direction, _cameraRotation.forward, 1f, 1f);
                float dot = Vector3.Dot(direction, _cameraRotation.forward);
                _rb.AddTorque(input.deltaPosition.magnitude * _sensitivity * a);
            }
        }
    }
}
