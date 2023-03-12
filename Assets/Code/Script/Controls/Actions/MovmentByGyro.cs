using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovmentByGyro : BaseActions
{
    [SerializeField] private float _minGyroForce;
    [SerializeField] private float _maxImpulse;
    [SerializeField] private float _maxSpeed;

    private Gyroscope gyro;
    private Vector3 _currentMovment;
    
    private void OnEnable()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            SetTarget(transform, GetComponent<Rigidbody>());
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
            Vector3 gyroMovment = new Vector3(gyro.rotationRateUnbiased.y, 0, -gyro.rotationRateUnbiased.x);
            if (gyroMovment.magnitude >= _minGyroForce && _rb.velocity.magnitude < _maxSpeed)
            {
                _currentMovment = Vector3.ClampMagnitude(gyroMovment + _currentMovment, _maxImpulse);                
                _rb.AddForce(_sensitivity * _currentMovment);
                _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxSpeed);
            }
        }
    }

    public void ResetGyroPosition()
    {
        _currentMovment = Vector3.zero;
    }

    public override void ExecuteAction()
    {
        MoveByGyro();
    }

}
