using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneHelper : MonoBehaviour
{
    [SerializeField] private MainMenu _mainMenu;
    //[SerializeField] private Animator _animator;

    //private void Awake()
    //{
    //    if (!_animator) _animator = GetComponent<Animator>();
    //}

    public void LoadMainScene()
    {
        if(_mainMenu) _mainMenu.OpenMainScene();
    }

    public void ReturnToMainMenu()
    {
        FadeUi.Instance.UpdateFade(FadeUi.FadeTypes.FADEIN, LoadMainMenu);
    }

    //public void PlayeFinalCutscene()
    //{
    //    if (_animator) _animator.Play("CutsceneFinal");
    //}

    private void LoadMainMenu()
    {
        LevelManager.Instance.LoadLevel("0.MainMenu");
    }
}
