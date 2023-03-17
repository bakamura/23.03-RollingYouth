using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float _minMassToActivate;
    [SerializeField] private float _flipperSpeed;
    [SerializeField] private float _extraForce;
    [SerializeField, Min(0f)] private float _maxAngle;
    [SerializeField, Tooltip("if true, the flipper will rotate towards +X, else will rotate towards -X")] private bool _useBaseRight = true;
#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool _debugDraw;
    [SerializeField] private Color _debugColor = Color.red;
    [SerializeField] private float _debugDuration;
#endif

    private bool _isMoving;
    private float _currentDelta;
    private bool _isReturning;
    private float _initialRotation;
    private float _currentRotation;
    private float _finalMaxAngle;
    private Rigidbody _currentRb;
    private Vector3 _launchDirection;

    private void Awake()
    {
        _initialRotation = transform.eulerAngles.y;
        _finalMaxAngle = _useBaseRight ? _maxAngle : _maxAngle * -1f;
    }

    public void Bump(Rigidbody rb)
    {
        if (!_isMoving && rb.mass >= _minMassToActivate)
        {
            _currentDelta = 0;
            _isReturning = false;
            _currentRb = rb;
            _launchDirection = transform.forward;
            _isMoving = true;
        }
    }

    private void Update()
    {
        if (_isMoving)
        {
            if (_isReturning)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle(_currentRotation, _initialRotation, _currentDelta), transform.eulerAngles.z);
                if (_currentDelta >= 1f) _isMoving = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle(_initialRotation, _finalMaxAngle + _initialRotation, _currentDelta), transform.eulerAngles.z);
                if (_currentDelta >= 1f)
                {
                    _currentRotation = transform.eulerAngles.y;
                    _isReturning = true;
                    _currentDelta = 0;
                    _currentRb.AddForce(_useBaseRight ? _launchDirection * _extraForce : -_launchDirection * _extraForce, ForceMode.Impulse);
#if UNITY_EDITOR
                    Debug.DrawLine(_currentRb.position, _useBaseRight ? transform.position + _launchDirection * _extraForce : transform.position + -_launchDirection * _extraForce, _debugColor, _debugDuration);
                    Debug.Log($"Target Position {_currentRb.position}, direction {_launchDirection * 3f }");
#endif
                }
            }
            _currentDelta += Time.deltaTime * _flipperSpeed;
        }
    }
}
