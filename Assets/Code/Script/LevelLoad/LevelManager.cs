using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : BaseSingleton<LevelManager>
{
    [SerializeField] private bool _loadSceneOnStart = true;
    [SerializeField] private string _initialScene;

    private string _currentLevelName;

    private void Start()
    {
        if (_loadSceneOnStart) LoadLevel(_initialScene);
    }

    public void LoadLevel(string sceneName)
    {
        if (!string.IsNullOrEmpty(_currentLevelName))
        {
            SceneManager.UnloadSceneAsync(_currentLevelName);
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.completed += FadeOut;
        _currentLevelName = sceneName;
    }

    private void FadeOut(AsyncOperation operation)
    {
        FadeUi.Instance.UpdateFade(FadeUi.FadeTypes.FADEOUT, null);
    }
}
