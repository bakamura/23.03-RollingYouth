using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByTouch : MonoBehaviour
{
    [SerializeField] private float _dragTresHold;
    [SerializeField] private float _sensitivity;
    [SerializeField] private Transform _currentTarget;
    [SerializeField, Range(0f, 1f)] private float _dragAreaPrecentX;
    [SerializeField, Range(0f, 1f)] private float _dragAreaPrecentY;
    [SerializeField] private RectTransform _area;

    private Touch _input;
    private bool _isRotating;

    public bool IsRotating => _isRotating;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            _input = Input.GetTouch(0);

            if (_input.phase == TouchPhase.Moved && _input.deltaPosition.magnitude > _dragTresHold && CheckTouchArea())
            {
                _currentTarget.eulerAngles += _sensitivity * Time.deltaTime * new Vector3(0, _input.deltaPosition.x, 0);
            }
        }
    }

    private bool CheckTouchArea()
    {
        _isRotating = _area.rect.Contains(_input.position);
        return _isRotating;
    }

#if UNITY_EDITOR
    [ContextMenu("RecalculateDebugArea")]
    private void RecalcDebugArea()
    {
        _area.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.currentResolution.height * _dragAreaPrecentX);
        _area.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.currentResolution.width * _dragAreaPrecentY);
    }
#endif
}
