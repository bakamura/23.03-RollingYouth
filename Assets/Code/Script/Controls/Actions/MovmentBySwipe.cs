using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovmentBySwipe : BaseActions
{
    [SerializeField] private float _dragTresHold;
    //[SerializeField, Tooltip("the maximum amount of force that the swipe will afect the movment")] private float _maxForceInDrag =1;
    [SerializeField] private Transform _cameraRotation;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private bool _isSwipeActive;

    private RotateByTouch _rotateByTouch;

    public bool IsSwipeActive { get { return _isSwipeActive; } set { UpdateCurrentInputMode(value); } }

#if UNITY_EDITOR
    [SerializeField] private bool _useKeyboard;
#endif

    private void Awake()
    {
        SetTarget(transform, GetComponent<Rigidbody>());
        _rotateByTouch = _cameraRotation.GetComponent<RotateByTouch>();
    }

    public override void ExecuteAction()
    {
        if (IsSwipeActive) SwipeMovment();
    }

    private void SwipeMovment()
    {
#if UNITY_EDITOR
        if (_useKeyboard && _rb)
        {
            Vector3 direction = Input.GetAxis("Vertical") * _cameraRotation.right;
            _rb.AddTorque(direction * _sensitivity, ForceMode.Acceleration);
        }
#endif
        if (Input.touchCount > 0)
        {
            Touch input = Input.GetTouch(0);

            if (input.phase == TouchPhase.Moved && input.deltaPosition.magnitude > _dragTresHold && !_rotateByTouch.IsRotating)
            {
                Vector3 direction = input.deltaPosition.y * _cameraRotation.right + -input.deltaPosition.x * _cameraRotation.forward;
                _rb.AddTorque(direction * _sensitivity, ForceMode.Acceleration);
            }
        }
    }

    private void UpdateCurrentInputMode(bool isActive)
    {
        _isSwipeActive = isActive;
        if (isActive) _rb.maxAngularVelocity = _maxVelocity;
    }
}
