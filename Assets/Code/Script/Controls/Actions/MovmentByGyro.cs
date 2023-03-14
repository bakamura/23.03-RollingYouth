using UnityEngine;

public class MovmentByGyro : BaseActions
{
    private Gyroscope gyro;
    private Vector3 _baseGravity;
    [SerializeField] private float _minGyroForce;
    [SerializeField] private float _maxAngle;

    private void OnEnable()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            _baseGravity = gyro.gravity;

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
            Vector3 rotation = gyro.gravity - _baseGravity;
            print(rotation);
            if (rotation.sqrMagnitude > _minGyroForce)
            {
                rotation = new Vector3(rotation.y, 0, -rotation.x);
                rotation = Vector3.ClampMagnitude(rotation, 0.4f);
                _rb.AddTorque(rotation * _sensitivity, ForceMode.Acceleration);
            }
        }
    }

    public void ResetGyroPosition()
    {
        _baseGravity = gyro.gravity;
    }

}
