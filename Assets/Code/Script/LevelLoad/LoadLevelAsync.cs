using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelAsync : MonoBehaviour
{
    [SerializeField] private string SceneToLoad;

    private bool _isSceneLoaded = false;

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player")){
        if (!_isSceneLoaded)
        {            
            AsyncOperation operation = SceneManager.LoadSceneAsync(SceneToLoad, LoadSceneMode.Additive);
            operation.completed += OnSceneLoaded;
        }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (/*other.CompareTag("Player") &&*/ _isSceneLoaded)
        {
            AsyncOperation operation = SceneManager.UnloadSceneAsync(SceneToLoad);
            operation.completed += OnSceneUnloaded;
        }
    }

    private void OnDestroy()
    {
        if (/*other.CompareTag("Player") &&*/ _isSceneLoaded)
        {
            AsyncOperation operation = SceneManager.UnloadSceneAsync(SceneToLoad);
            operation.completed += OnSceneUnloaded;
        }
    }

    private void OnSceneLoaded(AsyncOperation operation)
    {
        _isSceneLoaded = true;
    }

    private void OnSceneUnloaded(AsyncOperation operation)
    {
        _isSceneLoaded = false;
    }
}
