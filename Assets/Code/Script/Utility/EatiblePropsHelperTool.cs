using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EatiblePropsHelperTool : MonoBehaviour
{
    [SerializeField, Tooltip("the size that will be applied to all EatibleProps in the current scene")] private float _newSize;
    [SerializeField, Tooltip("if true the reposition will take the size of the prop into consideration")] private bool _adaptRepositionToObjectSize;
    [SerializeField, Tooltip("randomize the materials of all eatible, in empty will keep the curent materials")] private Material[] _materialsToRandomize;

    public void ResizeProps()
    {
        //EatibleProp[] props = FindObjectsOfType<EatibleProp>();
        //for (int i = 0; i < props.Length; i++)
        //{
        //    props[i].transform.localScale = new Vector3(_newSize, _newSize, _newSize);
        //}
        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            Selection.transforms[i].localScale = new Vector3(_newSize, _newSize, _newSize);
        }
        //transform.localScale = Vector3.one * _newSize;        
    }

    public void RepositionProps()
    {
        EatibleProp[] props = FindObjectsOfType<EatibleProp>();
        for (int i = 0; i < props.Length; i++)
        {
            if(Physics.Raycast(props[i].transform.position, -Vector3.up, out RaycastHit hit))
            {
                props[i].transform.position = _adaptRepositionToObjectSize ? hit.point + new Vector3(0, _newSize/2f, 0) : hit.point;
            }           
        }
    }

    public void RandomizeMaterials()
    {
        if(_materialsToRandomize.Length > 0)
        {
            EatibleProp[] props = FindObjectsOfType<EatibleProp>();
            for (int i = 0; i < props.Length; i++)
            {
                props[i].GetComponent<MeshRenderer>().sharedMaterial = _materialsToRandomize[Random.Range(0, _materialsToRandomize.Length)];
            }
        }
        else
        {
            Debug.LogWarning("Materials list is empty");
        }
    }
}
