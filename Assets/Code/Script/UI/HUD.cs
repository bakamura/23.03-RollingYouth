using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HUD : UI
{
    [SerializeField] private float _containerAnimDuration;
    [SerializeField] private Container _pauseContainer;
    [SerializeField] private Sprite[] _pauseIcons = new Sprite[2];
    [SerializeField] private ControlIconsData[] _controlIcons = new ControlIconsData[2];
    [SerializeField] private Image _pauseImageBtn;
    [SerializeField] private Image _controlImageBtn;

    public static bool IsGamePaused;

    [Serializable]
    private struct ControlIconsData
    {
        public PlayerControlers.ControlTypes ControlType;
        public Sprite Icon;
    }

    private PlayerControlers.ControlTypes _currentControlActive = PlayerControlers.ControlTypes.GYROSCOPE;

    //private void Start()
    //{
    //    RoundButton();
    //}

    public void PauseMenu()
    {
        Time.timeScale = IsGamePaused ? 1f : 0f;
        IsGamePaused = !IsGamePaused;
        if (!_pauseContainer.IsAnimating) _pauseImageBtn.sprite = _pauseContainer.IsOpen ? _pauseIcons[0] : _pauseIcons[1];
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
        UpdateControlBtnImage();
    }

    private void UpdateContainer(Container container)
    {
        if (!container.IsAnimating)
        {
            StartCoroutine(ExpandContainer(container, container.IsOpen ? container.closedSize : container.openSize, _containerAnimDuration));
        }
    }

    private void UpdateControlBtnImage()
    {
        _currentControlActive++;
        _currentControlActive = (int)_currentControlActive >= Enum.GetNames(typeof(PlayerControlers.ControlTypes)).Length ? 0 : _currentControlActive;
        _controlImageBtn.sprite = _controlIcons.Where(x => x.ControlType == _currentControlActive).ToArray()[0].Icon;
    }
}
