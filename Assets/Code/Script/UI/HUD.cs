using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HUD : UI
{
    [SerializeField] private float _containerAnimDuration;
    //[SerializeField] private Image _pauseImageBtn;
    //[SerializeField] private Image _controlImageBtn;
    [SerializeField] private TwoModeButtonData[] _twoModeBtns;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private GameObject _controlsUI;
    //[SerializeField] private Sprite[] _pauseIcons = new Sprite[2];
    //[SerializeField] private ControlIconsData[] _controlIcons = new ControlIconsData[2];
    [SerializeField] private Container _pauseContainer;
    [SerializeField] private Container _settingsContainer;

    public static bool IsGamePaused;

    //[Serializable]
    //private struct ControlIconsData
    //{
    //    public PlayerControlers.ControlTypes ControlType;
    //    public Sprite Icon;
    //}

    [Serializable]
    private class TwoModeButtonData
    {
        public string ButtonID;
        public Image ButtonIcon;
        public Sprite[] Icons = new Sprite[2];
        [HideInInspector] public int CurrentIconIndex;
    }

    //private PlayerControlers.ControlTypes _currentControlActive = PlayerControlers.ControlTypes.GYROSCOPE;

    //private void Start()
    //{
    //    RoundButton();
    //}

    public void PauseMenu(string id)
    {
        Time.timeScale = IsGamePaused ? 1f : 0f;
        IsGamePaused = !IsGamePaused;

        //graphics update
        if (!_pauseContainer.IsAnimating) UpdateTwoModeBtn(id); /*_pauseImageBtn.sprite = _pauseContainer.IsOpen ? _pauseIcons[0] : _pauseIcons[1];*/
        UpdateBackground(true);

        if (_settingsContainer.IsOpen) SettingsBtn();
        UpdateContainer(_pauseContainer, UpdateBackground);
    }

    public void SfxSoundButton(string id)
    {
        SoundManager.Instance.UpdateSfxVolume();
        UpdateTwoModeBtn(id);
    }

    public void MusicSoundButton(string id)
    {
        SoundManager.Instance.UpdateMusicVolume();
        UpdateTwoModeBtn(id);
    }

    public void ChangeControlersBtn(string id)
    {
        PlayerControlers.Instance.UpdateControlers();
        //UpdateControlBtnImage();
        UpdateTwoModeBtn(id);
    }

    public void SettingsBtn()
    {
        UpdateContainer(_settingsContainer);
    }

    private void UpdateContainer(Container container, Action<bool> onTransitionEnd = null)
    {
        if (!container.IsAnimating)
        {
            StartCoroutine(ExpandContainer(container, container.IsOpen ? container.ClosedSize : container.OpenSize, _containerAnimDuration, onTransitionEnd));
        }
    }

    //private void UpdateControlBtnImage()
    //{
    //    _currentControlActive++;
    //    _currentControlActive = (int)_currentControlActive >= Enum.GetNames(typeof(PlayerControlers.ControlTypes)).Length ? 0 : _currentControlActive;
    //    _controlImageBtn.sprite = _controlIcons.Where(x => x.ControlType == _currentControlActive).ToArray()[0].Icon;
    //}

    private void UpdateTwoModeBtn(string buttonID)
    {
        TwoModeButtonData temp = _twoModeBtns.First(x => x.ButtonID == buttonID);
        temp.CurrentIconIndex++;
        if (temp.CurrentIconIndex == temp.Icons.Length) temp.CurrentIconIndex = 0;
        temp.ButtonIcon.sprite = temp.Icons[temp.CurrentIconIndex];
    }

    private void UpdateBackground(bool isMenuOpen)
    {
        _backgroundImage.enabled = isMenuOpen;
    }

    public void ToggleControlUI(bool isActive)
    {
        _controlsUI.SetActive(isActive);
    }
}
