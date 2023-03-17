using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plunger : MonoBehaviour
{
    [SerializeField] private float _dragTresHold;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch input = Input.GetTouch(0);

            if (input.phase == TouchPhase.Moved && input.deltaPosition.magnitude > _dragTresHold)
            {
                transform.position += new Vector3(0, input.deltaPosition.y);
            }
        }
    }
}
