using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialCutscene : MonoBehaviour
{
    [SerializeField] private MainMenu _mainMenu;
    public void LoadMainScene()
    {
        _mainMenu.OpenMainScene();
    }
}
