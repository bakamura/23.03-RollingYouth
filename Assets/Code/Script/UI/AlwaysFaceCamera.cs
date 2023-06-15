using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    [SerializeField] private float _tickFrequency = .02f;
    private Transform _camera;
    private WaitForSeconds _delay;

    private void Start()
    {
        _camera = PlayerReference.Instance.PlayerComponents.CameraPosition;
        _delay = new WaitForSeconds(_tickFrequency);
        StartCoroutine(FaceCamera());
    }

    IEnumerator FaceCamera()
    {
        while (_camera)
        {
            transform.LookAt(_camera);
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
            yield return _delay;
        }
    }
}
