using UnityEngine;

public class MovmentByGyro : BaseActions
{
    private Gyroscope gyro;
    private Vector3 _baseGravity;
    //private Vector3 _currentAngle;
    [SerializeField] private float _minGyroForce;
    [SerializeField] private float _maxAngle;

    private void OnEnable()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            _baseGravity = gyro.gravity;//new Vector3(gyro.gravity.y, 0, -gyro.gravity.x);

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
    public override void ExecuteAction()
    {
        MoveByGyro();
    }

    private void MoveByGyro()
    {
        if (gyro != null)
        {
            // naka
            Vector3 rotation = gyro.gravity - _baseGravity;
            if (rotation.sqrMagnitude > _minGyroForce)
            {
                rotation = new Vector3(rotation.y, 0, -rotation.x);
                rotation = Vector3.ClampMagnitude(rotation, _maxAngle);
                _rb.AddTorque(rotation * _sensitivity, ForceMode.Acceleration);
                //Debug.Log((rotation * _sensitivity).magnitude);
            }

            //vini
            //Vector3 gyroMovment = new Vector3(gyro.rotationRateUnbiased.y, 0, -gyro.rotationRateUnbiased.x);
            //_currentAngle += gyroMovment;
            //if (_currentAngle.magnitude >= _minGyroForce)
            //{
            //    Vector3 direction = gyro.gravity - _baseGravity;
            //    direction = new Vector3(direction.y, 0, -direction.x);
            //    _rb.AddTorque(_currentAngle.magnitude * direction * _sensitivity, ForceMode.Acceleration);
            //    Debug.Log(total.magnitude);
            //}
        }
    }

    public void ResetGyroPosition()
    {
        _baseGravity = gyro.gravity;
        //_currentAngle = Vector3.zero;
        //_rb.velocity = Vector3.zero;
    }

}
