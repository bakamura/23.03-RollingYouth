using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : BaseSingleton<LevelManager>
{
    [SerializeField] private bool _loadSceneOnStart = true;
    [SerializeField] private string _initialScene;
    [SerializeField] private FadeUi _fadeUi;

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
        operation.completed += FadeIn;
    }

    private void FadeIn(AsyncOperation operation)
    {
        _fadeUi.UpdateFade(FadeUi.FadeTypes.FADEIN, null);
    }
}
