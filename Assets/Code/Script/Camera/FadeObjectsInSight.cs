using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObjectsInSight : MonoBehaviour
{
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private LayerMask _layersToCollideWith;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _checkFrequence;
    //[SerializeField] private float _raycastCheckRadius = .2f;
    [SerializeField] private Material _fadeMaterial;
    [SerializeField] private PlayerComponents _playerComponents;

    private WaitForSeconds _delay;
    private List<MeshRenderer> _previousHits = new List<MeshRenderer>();
    //private GameObject _currentTarget;
    //private Material _previousMaterial;
    private List<Material> _previousMaterials =  new List<Material>();

    private Dictionary<int, FadeData> _currentObjects = new Dictionary<int, FadeData>();
    private Dictionary<int, FadeData> _previousObjects;
    private Material _instanceFadeMat;

    [System.Serializable]
    private class FadeData
    {
        public int ObjectID;
        public Material Material;
        public MeshRenderer MeshRenderer;

        public FadeData(Material mat, MeshRenderer meshRenderer, int Id)
        {
            Material = mat;
            MeshRenderer = meshRenderer;
            ObjectID = Id;
        }
    }

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
        #region For1Object
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
        #endregion
        #region ForMultipleObjects
        // for multiple objects
        while (true)
        {
            RaycastHit[] hits = Physics.SphereCastAll(_cameraTransform.position, _playerComponents.ObjectGrow.ObjectToGrow.localScale.x / 2f/*_raycastCheckRadius*/, _rayDirection.normalized, _rayDirection.magnitude * .95f, _layersToCollideWith);
            //Debug.Log(hits.Length);
            FadeObjects(hits);
            RemoveFade(hits);
            #region OldVersion
            ////if (_previousHits.Count > 0)
            ////{
            //for (int i = 0; i < _previousHits.Count; i++)
            //{
            //    if (_previousHits[i]) _previousHits[i].material = _previousMaterials[i];
            //}
            //_previousMaterials.Clear();
            //_previousHits.Clear();
            ////}

            //RaycastHit[] hits = Physics.SphereCastAll(_cameraTransform.position, _playerComponents.ObjectGrow.ObjectToGrow.localScale.x / 2f/*_raycastCheckRadius*/, _rayDirection.normalized, _rayDirection.magnitude * .95f, _layersToCollideWith);
            //for (int i = 0; i < hits.Length; i++)
            //{
            //    MeshRenderer temp = hits[i].collider.GetComponent<MeshRenderer>();
            //    if (temp)
            //    {
            //        _previousHits.Add(temp);
            //        _previousMaterials.Add(temp.material);
            //        _instanceFadeMat.SetTexture("_MainTex", temp.material.GetTexture("_MainTex"));
            //        _instanceFadeMat.SetTexture("_MetallicGlossMap", temp.material.GetTexture("_MetallicGlossMap"));
            //        temp.material = _instanceFadeMat;
            //    }
            //}
            #endregion
            yield return _delay;
        }
        #endregion
    }

    private void FadeObjects(RaycastHit[] hits)
    {        
        _previousObjects = new Dictionary<int, FadeData>(_currentObjects);
        for (int i = 0; i < hits.Length; i++)
        {
            MeshRenderer mesh = hits[i].collider.GetComponent<MeshRenderer>();
            FadeData temp = new FadeData(mesh.material, mesh, mesh.gameObject.GetInstanceID());
            if (!_currentObjects.ContainsKey(temp.ObjectID))
            {
                _currentObjects.Add(temp.ObjectID, temp);
                _instanceFadeMat.SetTexture("_MainTex", temp.Material.GetTexture("_MainTex"));
                _instanceFadeMat.SetTexture("_MetallicGlossMap", temp.Material.GetTexture("_MetallicGlossMap"));
                temp.MeshRenderer.material = _instanceFadeMat;
                //Debug.Log($"added {temp.MeshRenderer.gameObject.name}");
            }
        }
        //Debug.Log($"current has size {_currentObjects.Values.Count}");
    }

    private void RemoveFade(RaycastHit[] hits)
    {
        //Debug.Log($"previous has size {_previousObjects.Values.Count}");
        if (_previousObjects != _currentObjects && _previousObjects != null)
        {
            for(int i = 0; i < hits.Length; i++)
            {
                int temp = hits[i].collider.gameObject.GetInstanceID();
                if (_previousObjects.ContainsKey(temp))
                {
                    _previousObjects.Remove(temp);
                }
            }

            foreach (FadeData fadeData in _previousObjects.Values)
            {
                _previousObjects[fadeData.ObjectID].MeshRenderer.material = _previousObjects[fadeData.ObjectID].Material;
                _currentObjects.Remove(fadeData.ObjectID);
                //Debug.Log($"removed {_previousObjects[fadeData.ObjectID].MeshRenderer.gameObject.name}");
            }
        }
        else if (hits.Length == 0 && _currentObjects.Count > 0)
        {
            foreach (FadeData fadeData in _currentObjects.Values)
            {
                _currentObjects[fadeData.ObjectID].MeshRenderer.material = _currentObjects[fadeData.ObjectID].Material;
            }
            _currentObjects.Clear();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_cameraTransform.position, _cameraTransform.position + _rayDirection);
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(_cameraTransform.position + _rayDirection * .95f, _playerComponents.ObjectGrow.ObjectToGrow.localScale.x / 2f/*_raycastCheckRadius*/);
        }
    }
#endif
}
