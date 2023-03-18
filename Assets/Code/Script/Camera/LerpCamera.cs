using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LerpCamera : MonoBehaviour
{
    private float _currentDelta;
    private Transform _targetTransform;
    private Vector3 _finalPosition;
    private Quaternion _finalRotation;
    private float _currentSpeed;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private bool _isAnimating;
    private Action<bool> _onEndLerpForCameraUser;
    private Action _onEndLerp;
    private CameraUser _cameraUser;

    public bool IsAnimating => _isAnimating;

    private void Awake()
    {
        _cameraUser = GetComponent<CameraUser>();
        _onEndLerpForCameraUser += _cameraUser.UpdateEntityState;
    }

    private void Update()
    {
        if(_isAnimating)
        {
            _currentDelta += Time.deltaTime * _currentSpeed;
            _targetTransform.SetPositionAndRotation(Vector3.Lerp(_initialPosition, _finalPosition, _currentDelta), Quaternion.Lerp(_initialRotation, _finalRotation, _currentDelta));
            if (_currentDelta >= 1f)
            {
                _isAnimating = false;
                _onEndLerpForCameraUser?.Invoke(_cameraUser.IsBegining);
                _onEndLerp?.Invoke();
            }
        }        
    }

    public void LerpCam(Transform camera, Vector3 finalPosition, Quaternion finalRotation, float speed, Action OnEndLerp = null)
    {
        if(!_isAnimating)
        {
            _targetTransform = camera;
            _finalPosition = finalPosition;
            _finalRotation = finalRotation;
            _initialPosition = camera.position;
            _initialRotation = camera.rotation;
            _currentSpeed = speed;
            _currentDelta = 0f;
            _onEndLerp = null;
            _onEndLerp += OnEndLerp;
            _isAnimating = true;
        }
    }
}
