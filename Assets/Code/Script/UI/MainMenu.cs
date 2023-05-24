using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MainMenu : UI {

    [SerializeField] private string _baseSceneName;
    [SerializeField] private float _containerAnimDuration;
    [SerializeField, FormerlySerializedAs("ConfigurationContainer")] private Container _configContainer;
    [SerializeField, FormerlySerializedAs("SocialContainer")] private Container _socialContainer;

    private void Start() {
        RoundButton();
    }

    public void Play() {
        FadeUi.Instance.UpdateFade(FadeUi.FadeTypes.FADEIN, LoadMainScene);
    }

    public void SocialMedias()
    {
        UpdateContainer(_socialContainer);
    }

    public void Configurations()
    {
        UpdateContainer(_configContainer);
    }

    private void UpdateContainer(Container container) {
        if(!container.IsAnimating) StartCoroutine(ExpandContainer(container, container.IsOpen ? container.ClosedSize : container.OpenSize, _containerAnimDuration));
    }

    private void LoadMainScene()
    {
        LevelManager.Instance.LoadLevel(_baseSceneName);
    }
}