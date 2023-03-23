using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : UI {

    [SerializeField] private Container configContainer;
    [SerializeField] private Container socialContainer;
    [SerializeField] private float containerAnimDuration;

    private void Start() {
        RoundButton();
    }

    public void Play() {
        SceneManager.LoadScene(1); // Provisory
    }

    public void Container(Container container) {
        StartCoroutine(ExpandContainer(container.rectTransform, container.openSize, containerAnimDuration));
    }
}

[Serializable]
public class Container {
    public RectTransform rectTransform;
    public Vector2 closedSize;
    public Vector2 openSize;
}