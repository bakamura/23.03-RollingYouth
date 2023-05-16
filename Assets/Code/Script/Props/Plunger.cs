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

    /// <summary>
    /// Needs to define exactly what happens when the player gets lauched, for now it justs add force to player
    /// </summary>
    [Header("LaunchValues")]
    [SerializeField] private float _force;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool _debugDraw;
    [SerializeField] private Color _debugColor;
#endif

    private Vector3 _initialPosition;
    private Vector3 _positionWhenTouchEnded;
    private bool _isTargetInside;
    private float _currentDelta = 1;
    private float _screenFactor;

    protected override void Awake()
    {
        base.Awake();
        _initialPosition = _leaver.position;
        _screenFactor = Screen.width;
    }

    void Update()
    {
        if (_isTargetInside && !_lerpCamera.IsAnimating)
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
                        if (Vector3.Distance(_initialPosition, newPosition) < _maxPushDistance) _leaver.position = newPosition;
                        else _leaver.position = -transform.up * _maxPushDistance + _initialPosition;
                    }
                }
            }
            //check to see if player pushed enought the leaver to launch
            else
            {
                if (Vector3.Distance(_leaver.position, _initialPosition) >= _maxPushDistance)
                {
                    EndCameraUse();
                    _playerComponents.PlayerRigidbody.AddForce(transform.up * _force, ForceMode.Impulse);
                    _leaver.position = _initialPosition;
                }
                else if (_currentDelta < 1)
                {
                    _currentDelta += Time.deltaTime * _returnPlungerSpeed;
                    _leaver.position = Vector3.Lerp(_positionWhenTouchEnded, _initialPosition, _currentDelta);
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
            BeginCameraUse(temp, temp.Camera.position, temp.Camera.rotation);
            UpdatePlayerPosition();
        }
    }

    protected override void BeginCameraUse(PlayerComponents playerComponents, Vector3 initialCamPos, Quaternion initialCamRot, Action OnEndLerp = null)
    {
        base.BeginCameraUse(playerComponents, initialCamPos, initialCamRot, OnEndLerp);
        _isTargetInside = true;
    }
    protected override void EndCameraUse(Action OnEndLerp = null)
    {
        _playerComponents.PlayerRigidbody.constraints = RigidbodyConstraints.None;
        _playerComponents.PlayerRigidbody.useGravity = true;
        base.EndCameraUse(OnEndLerp);
        _isTargetInside = false;
        _playerComponents.PlayerUI.ToggleControlUI(true);
    }
    private void UpdatePlayerPosition()
    {
        _playerComponents.PlayerTransform.position = transform.position + _playerPosition;
        _playerComponents.PlayerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _playerComponents.PlayerRigidbody.useGravity = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (_debugDraw)
        {
            Gizmos.color = _debugColor;
            Gizmos.DrawSphere(transform.position + _playerPosition, .5f);
        }
    }
#endif
}
