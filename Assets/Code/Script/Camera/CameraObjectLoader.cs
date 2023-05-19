using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CameraObjectLoader : MonoBehaviour
{
    private Camera _playerCamera;

    private void Awake()
    {
        _playerCamera = GetComponentInParent<Camera>();
        UpdateLoadAreaSize();
    }

    private void UpdateLoadAreaSize()
    {
        Vector3 screenSize = _playerCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _playerCamera.farClipPlane)) - _playerCamera.ScreenToWorldPoint(new Vector3(0, 0, _playerCamera.farClipPlane));
        transform.localScale = new Vector3(screenSize.x, screenSize.y, _playerCamera.farClipPlane);
    }
}
