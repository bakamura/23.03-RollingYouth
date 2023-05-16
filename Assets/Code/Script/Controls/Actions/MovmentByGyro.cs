using UnityEngine;

public class MovmentByGyro : BaseActions
{
    private Gyroscope gyro;
    private Vector3 _baseGravity;
    //private Vector3 _currentAngle;
    [SerializeField] private float _minGyroForce;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private Transform _cameraRotation;
    [SerializeField] private bool _isGyroActive = true;

    public bool IsGyroActive
    {
        get { return _isGyroActive; }
        set
        {
            UpdateCurrentInputMode(value);
        }
    }

#if UNITY_EDITOR
    [SerializeField] private bool _useKeyboard;
#endif

    private void Awake()
    {
        SetTarget(transform, GetComponent<Rigidbody>());
    }

    private void OnEnable()
    {
        _rb.maxAngularVelocity = _maxVelocity;
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            _baseGravity = gyro.gravity;
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
        if (IsGyroActive) MoveByGyro();
    }

    private void MoveByGyro()
    {
        if (gyro != null)
        {
            // naka
            Vector3 rotation = gyro.gravity - _baseGravity;
            if (rotation.sqrMagnitude > _minGyroForce)
            {
                // rotation.y = left/right, -rotation.x = up/down 
                rotation = rotation.y * _cameraRotation.right + -rotation.x * _cameraRotation.forward;
                //rotation = Vector3.ClampMagnitude(rotation, _maxAngle);
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
#if UNITY_EDITOR
        if (_useKeyboard && _rb)
        {
            Vector3 direction = Input.GetAxis("Vertical") * _cameraRotation.right;
            direction = Vector3.ClampMagnitude(direction, _maxVelocity);
            _rb.AddTorque(direction * _sensitivity);
        }
#endif
    }

    public void ResetGyroPosition()
    {
        _baseGravity = gyro.gravity;
        //_currentAngle = Vector3.zero;
        //_rb.velocity = Vector3.zero;
    }

    private void UpdateCurrentInputMode(bool isActive)
    {
        _isGyroActive = isActive;
        if (isActive) _rb.maxAngularVelocity = _maxVelocity;
    }
}
