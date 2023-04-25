using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObjectsInSight : MonoBehaviour
{
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private LayerMask _layersToCollideWith;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _checkFrequence;
    [SerializeField] private float _raycastCheckRadius;
    [SerializeField] private Material _fadeMaterial;

    private WaitForSeconds _delay;
    private RaycastHit[] _previousHits;
    //private GameObject _currentTarget;
    //private Material _previousMaterial;
    private List<Material> _previousMaterials =  new List<Material>();
    private Material _instanceFadeMat;

    private Vector3 _rayDirection => _playerPosition.position - _cameraTransform.position;

    private void Awake()
    {
        _delay = new WaitForSeconds(_checkFrequence);
        _instanceFadeMat = Instantiate(_fadeMaterial);
    }

    private void OnEnable()
    {
        StartCoroutine(CheckForObstacles());
    }

    private void OnDisable()
    {
        StopCoroutine(CheckForObstacles());
    }

    IEnumerator CheckForObstacles()
    {
        // for 1 object only
        //while (true)
        //{
        //    if (Physics.SphereCast(_cameraTransform.position, _raycastCheckRadius, _rayDirection, out RaycastHit hit, _rayDirection.magnitude, _layersToCollideWith))
        //    {
        //        if (_currentTarget != hit.collider.gameObject)
        //        {
        //            if (_currentTarget)
        //            {
        //                _currentTarget.GetComponent<MeshRenderer>().material = _previousMaterial;
        //            }
        //            _previousMaterial = hit.collider.GetComponent<MeshRenderer>().material;
        //            _instanceFadeMat.SetTexture("_MainTex", hit.collider.GetComponent<MeshRenderer>().material.GetTexture("_MainTex"));
        //            _instanceFadeMat.SetTexture("_MetallicGlossMap", hit.collider.GetComponent<MeshRenderer>().material.GetTexture("_MetallicGlossMap"));
        //            hit.collider.GetComponent<MeshRenderer>().material = _instanceFadeMat;
        //            _currentTarget = hit.collider.gameObject;
        //        }
        //    }
        //    yield return _delay;
        //}
        // for multiple objects
        while (true)
        {
            if (_previousHits != null)
            {
                for (int i = 0; i < _previousHits.Length; i++)
                {
                    _previousHits[i].collider.GetComponent<MeshRenderer>().material = _previousMaterials[i];
                }
                _previousMaterials.Clear();
            }

            RaycastHit[] hits = Physics.SphereCastAll(_cameraTransform.position, _raycastCheckRadius, _rayDirection, _rayDirection.magnitude, _layersToCollideWith);
            for (int i = 0; i < hits.Length; i++)
            {
                _previousMaterials.Add(hits[i].collider.GetComponent<MeshRenderer>().material);
                _instanceFadeMat.SetTexture("_MainTex", hits[i].collider.GetComponent<MeshRenderer>().material.GetTexture("_MainTex"));
                _instanceFadeMat.SetTexture("_MetallicGlossMap", hits[i].collider.GetComponent<MeshRenderer>().material.GetTexture("_MetallicGlossMap"));
                hits[i].collider.GetComponent<MeshRenderer>().material = _instanceFadeMat;
            }
            _previousHits = hits;
            yield return _delay;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_cameraTransform.position, _cameraTransform.position + _rayDirection);
        }
    }
}
