using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByTouch : MonoBehaviour
{
    [SerializeField] private float _dragTresHold;
    [SerializeField] private float _sensitivity;
    [SerializeField] private Transform _currentTarget;
    [SerializeField] private Vector2 _dragArea;
    [SerializeField] private Vector2 _dragPosition;
#if UNITY_EDITOR
    [SerializeField, Tooltip("the pivot needs to be at the bottom-left because this point is (0, 0, 0) for the touch system")] private RectTransform _debugPanel;
#endif

    private Touch _input;
    private bool _isRotating;
    private Rect _dragContainer;

    public bool IsRotating => _isRotating;

    private void Awake()
    {
        _dragContainer = new Rect(_dragPosition, _dragArea);
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            _input = Input.GetTouch(0);

            if (_input.phase == TouchPhase.Moved && _input.deltaPosition.magnitude > _dragTresHold && CheckTouchArea())
            {
                _currentTarget.eulerAngles += _sensitivity * new Vector3(0, _input.deltaPosition.x, 0);
            }
        }
    }

    private bool CheckTouchArea()
    {
        _isRotating = _dragContainer.Contains(_input.position);
        return _isRotating;
    }

#if UNITY_EDITOR
    [ContextMenu("RecalculateDebugArea")]
    private void RecalcDebugArea()
    {
        _debugPanel.anchoredPosition = _dragPosition;
        _debugPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _dragArea.x);
        _debugPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _dragArea.y);        
    }
#endif
}
