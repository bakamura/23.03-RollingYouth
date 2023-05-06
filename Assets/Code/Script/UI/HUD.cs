using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : UI
{
    [SerializeField] private float _containerAnimDuration;
    [SerializeField] private Container _pauseContainer;
    public static bool IsGamePaused;

    private void Start()
    {
        RoundButton();
    }

    public void PauseMenu()
    {
        //Time.timeScale = IsGamePaused ? 1f : 0f;
        //IsGamePaused = !IsGamePaused;
        UpdateContainer(_pauseContainer);
    }

    public void SfxSoundButton()
    {
        SoundManager.Instance.UpdateSfxVolume();
    }

    public void MusicSoundButton()
    {
        SoundManager.Instance.UpdateMusicVolume();
    }

    public void ChangeControlersBtn()
    {
        PlayerControlers.Instance.UpdateControlers();
    }

    private void UpdateContainer(Container container)
    {
        if (!container.IsAnimating) StartCoroutine(ExpandContainer(container, container.IsOpen ? container.closedSize : container.openSize, _containerAnimDuration));
    }
}
