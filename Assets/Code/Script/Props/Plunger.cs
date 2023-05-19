using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plunger : CameraUser
{
    [Header("Plunger Values")]
    [SerializeField] private float _dragTresHold;
    [SerializeField] private float _maxPushDistance;
    [SerializeField] private float _returnPlungerSpeed;
    [SerializeField, Min(1f)] private float _sensitivity;
    [SerializeField] private Vector3 _playerPosition;
    [SerializeField] private Transform _leaver;
    [SerializeField] private Transform _cameraPoint;

    /// <summary>
    /// Needs to define exactly what happens when the player gets lauched, for now it justs add force to player
    /// </summary>
    [Header("LaunchValues")]
    [SerializeField] private float _force;
    [SerializeField] private float _launchDuration;
    [SerializeField] private Vector3 _landingPoint;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool _debugDraw;
    [SerializeField] private Color _debugColorPlayerPosition;
    [SerializeField] private Color _debugColorLandingPoint = Color.black;
    [SerializeField] private float _debugLandingPointSize = .2f;
#endif

    private Vector3 _initialLeaverPosition;
    private Vector3 _positionWhenTouchEnded;
    private bool _isTargetInside;
    private float _currentDelta = 1;
    private float _screenFactor;
    private const float _tickFrequence = .02f;

    protected override void Awake()
    {
        base.Awake();
        _initialLeaverPosition = _leaver.position;
        _screenFactor = Screen.width;
    }

    void Update()
    {
        if (_isTargetInside && !LerpCamera.Instance.IsAnimating)
        {
            //checking for player to push the plunger and moving the leaver
            if (Input.touchCount > 0)
            {
                Touch input = Input.GetTouch(0);
                _positionWhenTouchEnded = _leaver.position;
                if (input.phase == TouchPhase.Moved && input.deltaPosition.magnitude > _dragTresHold)
                {
                    _currentDelta = 0;
                    Vector3 newPosition = _leaver.position + -transform.up * -input.deltaPosition.y / _screenFactor * _sensitivity;
                    if (input.deltaPosition.y < 0)
                    {
                        if (Vector3.Distance(_initialLeaverPosition, newPosition) < _maxPushDistance) _leaver.position = newPosition;
                        else _leaver.position = -transform.up * _maxPushDistance + _initialLeaverPosition;
                    }
                }
            }
            //check to see if player pushed enought the leaver to launch
            else
            {
                if (Vector3.Distance(_leaver.position, _initialLeaverPosition) >= _maxPushDistance)
                {
                    EndCameraUseWithDynamicPoints(_playerComponents.CalculatedCameraPosition);
                    _leaver.position = _initialLeaverPosition;
                    //_playerComponents.PlayerRigidbody.AddForce(transform.up * _force, ForceMode.Impulse);
                    LerpObject.Instance.SlerpObjectPosition(_playerComponents.PlayerTransform, _launchDuration, _playerComponents.PlayerTransform.position + _landingPoint, OnEndLaunchSlerp);
                }
                else if (_currentDelta < 1)
                {
                    _currentDelta += Time.deltaTime * _returnPlungerSpeed;
                    _leaver.position = Vector3.Lerp(_positionWhenTouchEnded, _initialLeaverPosition, _currentDelta);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isTargetInside)
        {
            PlayerComponents temp = other.GetComponent<PlayerComponents>();
            temp.ObjectGrow.UpdateSize(-temp.ObjectGrow.ObjectToGrow.localScale.x, -temp.PlayerRigidbody.mass);
            temp.PlayerUI.ToggleControlUI(false);
            BeginCameraUseWithFixedPoints(temp, _cameraPoint.position, _cameraPoint.rotation);
            UpdatePlayerPosition();
        }
    }

    protected override void BeginCameraUseWithFixedPoints(PlayerComponents playerComponents, Vector3 targetCamPos, Quaternion targetCamRot)
    {
        base.BeginCameraUseWithFixedPoints(playerComponents, targetCamPos, targetCamRot);
        _isTargetInside = true;
    }

    protected override void EndCameraUseWithDynamicPoints(Transform targetPositionAndRotation)
    {        
        base.EndCameraUseWithDynamicPoints(targetPositionAndRotation);
        _isTargetInside = false;
        _playerComponents.PlayerUI.ToggleControlUI(true);
    }

    private void UpdatePlayerPosition()
    {
        _playerComponents.PlayerTransform.position = transform.position + _playerPosition;
        _playerComponents.PlayerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _playerComponents.PlayerRigidbody.useGravity = false;
    }

    private void OnEndLaunchSlerp()
    {
        _playerComponents.PlayerRigidbody.constraints = RigidbodyConstraints.None;
        _playerComponents.PlayerRigidbody.useGravity = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_debugDraw)
        {
            Gizmos.color = _debugColorPlayerPosition;
            Gizmos.DrawSphere(transform.position + _playerPosition, .5f);
            Gizmos.color = _debugColorLandingPoint;
            Gizmos.DrawSphere(transform.position + _landingPoint, _debugLandingPointSize);
        }
    }
    [ContextMenu("RecaclCamOrientation")]
    private void RecalculateCameraOrientation()
    {
        _cameraPoint.LookAt(transform);
    }
#endif
}
