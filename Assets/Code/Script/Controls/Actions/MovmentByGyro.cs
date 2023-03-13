using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovmentByGyro : BaseActions
{
    [SerializeField] private float _minGyroForce;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _maxSpeed;
    [SerializeField, Range(0f, 1f)] private float _massChangeInVelocityFactor = .5f;
    [SerializeField] private bool _movmentByRotation;

    private Gyroscope gyro;
    private Vector3 _currentMovment;
    private Vector3 _currentAngle;
    private bool _useAttitude;

    private void OnEnable()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            SetTarget(transform, GetComponent<Rigidbody>());
            _useAttitude = gyro.attitude.x != 0f || gyro.attitude.y != 0f || gyro.attitude.z != 0f || gyro.attitude.w != 0f;
        }
        else Debug.LogWarning("gyro not supported on YOUR device");
    }

    private void OnDisable()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro.enabled = false;
        }
    }

    private void MoveByGyro()
    {
        if (gyro != null && _rb)
        {
            if (_useAttitude)
            {
                _currentTarget.rotation = gyro.attitude;
            }
            else
            {
                Vector3 gyroMovment = new Vector3(gyro.rotationRateUnbiased.y, 0, -gyro.rotationRateUnbiased.x);
                _currentAngle += gyroMovment;
                if (_currentAngle.magnitude >= _minGyroForce && _rb.velocity.magnitude < _maxSpeed)
                {
                    _currentMovment = Mathf.Clamp(_rb.mass * _massChangeInVelocityFactor, 1f, _rb.mass) * Mathf.Clamp(Vector3.Distance(Vector3.zero, _currentAngle), 0f, _maxAngle) * _currentAngle.normalized;
                    if (_movmentByRotation) _rb.AddTorque(_sensitivity * _currentMovment, ForceMode.Force);
                    else _rb.AddForce(_sensitivity * _currentMovment, ForceMode.Force);
                    _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxSpeed);
                }
                //if (_rb.velocity.magnitude < _maxSpeed)
                //{
                //    if (_movmentByRotation) _rb.AddTorque(_sensitivity * _currentMovment, ForceMode.Force);
                //    else _rb.AddForce(_sensitivity * _currentMovment, ForceMode.Force);
                //    _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxSpeed);
                //}
            }
        }
    }

    public void ResetGyroPosition()
    {
        _currentMovment = Vector3.zero;
        _currentAngle = Vector3.zero;
        _rb.velocity = Vector3.zero;
    }

    public override void ExecuteAction()
    {
        MoveByGyro();
    }

}
