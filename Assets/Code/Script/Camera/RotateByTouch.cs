using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByTouch : MonoBehaviour
{
    [SerializeField] private float _dragTresHold;
    [SerializeField, Range(0f, 1f)] private float _sensitivity;
    [SerializeField] private Transform _currentTarget;
    [SerializeField, Range(0f, 1f)] private float _dragAreaPrecentY;
    [SerializeField, Range(0f, 1f)] private float _dragAreaPrecentX;
    [SerializeField] private Vector2 _dragPosition;
#if UNITY_EDITOR
    [SerializeField, Tooltip("the pivot needs to be at the middle center for better correct calc")] private RectTransform _debugPanel;
#endif

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
                _currentTarget.eulerAngles += Screen.width * _sensitivity / Screen.width * new Vector3(0, _input.deltaPosition.x, 0);
            }
        }
    }

    private bool CheckTouchArea()
    {
        _isRotating = _input.position.y <= Screen.height * _dragAreaPrecentY && _input.position.x <= Screen.width * ((1 - _dragAreaPrecentX) / 2 + _dragAreaPrecentX) && _input.position.x >= Screen.width * (1 - _dragAreaPrecentX) / 2;
        return _isRotating;
    }

#if UNITY_EDITOR
    [ContextMenu("RecalculateDebugArea")]
    private void RecalcDebugArea()
    {
        _debugPanel.anchoredPosition = _dragPosition;
        _debugPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * _dragAreaPrecentX);
        _debugPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * _dragAreaPrecentY);
    }
#endif
}
