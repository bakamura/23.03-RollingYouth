using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByTouch : MonoBehaviour
{
    //[SerializeField] private float _dragTresHold;
    [SerializeField] private float _sensitivity;
    [SerializeField] private Transform _currentTarget;
    [SerializeField, Range(0f, 1f)] private float _dragAreaPrecentX;
    [SerializeField, Range(0f, 1f)] private float _dragAreaPrecentY;
    [SerializeField] private RectTransform _area;

#if UNITY_EDITOR
    [SerializeField] private bool _useKeyboard;
#endif

    private Touch _input;
    private bool _isRotating;

    public bool IsRotating => _isRotating;

    private void Awake()
    {
        RecalcDebugArea();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (_useKeyboard)
        {
            _currentTarget.eulerAngles += _sensitivity * Time.deltaTime * new Vector3(0, Input.GetAxis("Horizontal"), 0) / Screen.currentResolution.width;
        }
#endif
        if (Input.touchCount > 0)
        {
            _input = Input.GetTouch(0);

            if (_input.phase == TouchPhase.Moved /*&& _input.deltaPosition.magnitude > _dragTresHold*/ && CheckTouchArea())
            {
                _currentTarget.eulerAngles += _sensitivity * Time.deltaTime * new Vector3(0, _input.deltaPosition.x, 0) / Screen.currentResolution.width;
            }
        }
    }

    private bool CheckTouchArea()
    {
        //DebugText.WriteText($"current {_input.position}. Target: {Screen.height}");
        _isRotating = _input.position.y < Screen.height * _dragAreaPrecentY &&
            _input.position.x < Screen.width * ((1 - _dragAreaPrecentX) / 2 + _dragAreaPrecentX) &&
            _input.position.x > Screen.width * (1 - _dragAreaPrecentX) / 2;
        return _isRotating;
    }

//#if UNITY_EDITOR
    [ContextMenu("RecalculateDebugArea")]
    private void RecalcDebugArea()
    {
        _area.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * _dragAreaPrecentX);
        _area.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * _dragAreaPrecentY);
    }
//#endif
}
