using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(EntityStateChanger))]
public class Canon : MonoBehaviour
{
    [Serializable]
    private struct CanonPositions
    {
        public Quaternion Angles;
        [Tooltip("the time it will wait to start a new rotation")]public float WaitTimeToNextAngle;
        [Tooltip("the time it takes to rotate from the current CanonPosition to the next")] public float NextAngleTransitionTime;
    }
    [Header("CanonValues")]
    [SerializeField] private CanonPositions[] _canonPositions;
    [SerializeField] private Vector3 _playerPositionInsideCanon;
    [SerializeField] private float _delayToShootPlayer;
    [SerializeField] private float _tickFrequency;
    [SerializeField] private float _launchForce;

    private bool _isPlayerInside;
    private PlayerComponents _playerComponents;
    private EntityStateChanger _entityStateChanger;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private Color _playerPositionPointColor = Color.red;
    [SerializeField] private float _playerPositionPointSize;
#endif
    private void Awake()
    {
        _entityStateChanger = GetComponent<EntityStateChanger>();
    }

    private void OnEnable()
    {
        StartCoroutine(CanonBehaviour());
    }

    private void OnDisable()
    {
        StopCoroutine(CanonBehaviour());
    }

    private void OnTriggerEnter(Collider other)
    {
        LaunchSetup(other);
    }

    private void Update()
    {
        if (_isPlayerInside && Input.touchCount > 0)
        {
            Touch input = Input.GetTouch(0);
            if(input.phase == TouchPhase.Began)
            {
                Launch();
                StartCoroutine(CanonBehaviour());
            }
        }
    }

    private IEnumerator CanonBehaviour()
    {
        //while (true)
        //{
            float delta = 0;
            byte currentPositionIndex = 0;
            Quaternion currentRotation = transform.rotation;
            while (!_isPlayerInside)
            {
                if (delta >= 1f)
                {
                    yield return new WaitForSeconds(_canonPositions[currentPositionIndex].WaitTimeToNextAngle);
                    currentRotation = transform.rotation;
                    currentPositionIndex = (byte)(currentPositionIndex == _canonPositions.Length - 1 ? 0 : currentPositionIndex + 1);
                    delta = 0;
                }
                else
                {
                    //PERGUNTAR PARA O REINALDO COMO FAZER RODAR RELATIVO A ROTAÇÃO ANTERIOR
                    //transform.rotation = Quaternion.Lerp(currentRotation, Quaternion.Euler(
                    //    _canonPositions[currentPositionIndex].Angles.eulerAngles.x + transform.rotation.eulerAngles.x,
                    //    _canonPositions[currentPositionIndex].Angles.eulerAngles.y + transform.rotation.eulerAngles.y,
                    //    _canonPositions[currentPositionIndex].Angles.eulerAngles.z + transform.rotation.eulerAngles.z), delta);
                    transform.rotation = Quaternion.Lerp(currentRotation, _canonPositions[currentPositionIndex].Angles, delta);
                    float temp = _tickFrequency / _canonPositions[currentPositionIndex].NextAngleTransitionTime;
                    delta += temp;
                    yield return new WaitForSeconds(_tickFrequency);
                }
            }
            //yield return new WaitForSeconds(_delayToShootPlayer);
            //Launch();
        //}
    }

    private void LaunchSetup(Collider target)
    {
        _playerComponents = target.GetComponent<PlayerComponents>();
        _entityStateChanger.UpdateState(_playerComponents.PlayerActionsManagment);
        _playerComponents.PlayerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _playerComponents.PlayerRigidbody.useGravity = false;
        _isPlayerInside = true;
        _playerComponents.PlayerTransform.position = _playerPositionInsideCanon + transform.position;
    }

    private void Launch()
    {
        _playerComponents.PlayerRigidbody.constraints = RigidbodyConstraints.None;
        _playerComponents.PlayerRigidbody.useGravity = true;
        _playerComponents.PlayerRigidbody.AddForce(transform.forward * _launchForce, ForceMode.Impulse);
        _isPlayerInside = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _playerPositionPointColor;
        Gizmos.DrawSphere(transform.position + _playerPositionInsideCanon, _playerPositionPointSize);        
    }
#endif
}
