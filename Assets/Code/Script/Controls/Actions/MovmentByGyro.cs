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
            Vector3 rotation = gyro.gravity - _baseGravity;
            if (rotation.sqrMagnitude > _minGyroForce)
            {
                rotation = rotation.y * _cameraRotation.right + -rotation.x * _cameraRotation.forward;
                _rb.AddTorque(rotation * _sensitivity, ForceMode.Acceleration);
            }
        }
#if UNITY_EDITOR
        if (_useKeyboard && _rb)
        {
            Vector3 direction = Input.GetAxis("Vertical") * _cameraRotation.right;
            _rb.AddTorque(direction * _sensitivity, ForceMode.Acceleration);
        }
#endif
    }

    public void ResetGyroPosition()
    {
        _baseGravity = gyro.gravity;
    }

    private void UpdateCurrentInputMode(bool isActive)
    {
        _isGyroActive = isActive;
        if (isActive) _rb.maxAngularVelocity = _maxVelocity;
    }
}
