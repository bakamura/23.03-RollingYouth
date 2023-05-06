using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(LerpCamera), typeof(EntityStateChanger))]
public abstract class CameraUser : MonoBehaviour
{
    [Header("Camera Values")]
    [SerializeField, Tooltip("The camera will always look to the forward vector (+Z)")] protected Transform _cameraTransform;
    [SerializeField] private float _cameraLerpDuration;

    protected LerpCamera _lerpCamera;
    protected PlayerComponents _playerComponents;
    protected Vector3 _initialCameraPosition;
    protected Quaternion _initialCameraRotation;
    protected EntityStateChanger _entityStateChanger;
    private bool _isBegining = true;
    public bool IsBegining => _isBegining;

    protected virtual void Awake()
    {
        _lerpCamera = GetComponent<LerpCamera>();
        _entityStateChanger = GetComponent<EntityStateChanger>();
        _cameraTransform.LookAt(transform);
    }

    protected virtual void EndCameraUse(Action OnEndLerp = null)
    {
        _isBegining = false;
        _lerpCamera.LerpCam(_playerComponents.Camera, _initialCameraPosition, _initialCameraRotation, _cameraLerpDuration, OnEndLerp);        
    }

    protected virtual void BeginCameraUse(PlayerComponents playerComponents, Vector3 initialCamPos, Quaternion initialCamRot, Action OnEndLerp = null)
    {
        _isBegining = true;
        CollectCameraData(playerComponents, initialCamPos, initialCamRot);
        _lerpCamera.LerpCam(playerComponents.Camera, _cameraTransform.position, _cameraTransform.rotation, _cameraLerpDuration, OnEndLerp);
    }

    private void CollectCameraData(PlayerComponents playerComponents, Vector3 initialCamPos, Quaternion initialCamRot)
    {
        _playerComponents = playerComponents;
        _initialCameraPosition = initialCamPos;
        _initialCameraRotation = initialCamRot;
    }

    /// <summary>
    /// Will make the Entity go to the state defined in the EntityStateChanger attached to the CameraUser script, will always happend on the end of a CameraLerp
    /// </summary>
    public void UpdateEntityState(bool isBegining)
    {
        _entityStateChanger.UpdateState(_playerComponents.PlayerActionsManagment);
        _playerComponents.CameraFollow.enabled = !isBegining;
        _playerComponents.CameraRotate.enabled = !isBegining;
    }
}
