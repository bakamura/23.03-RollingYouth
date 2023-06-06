using UnityEngine;

public class PlayerReference : BaseSingleton<PlayerReference>
{
    [SerializeField] private PlayerComponents _playerComponents;
    public PlayerComponents PlayerComponents => _playerComponents;
}
