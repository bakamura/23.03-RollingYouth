using UnityEngine;

public class PlayerAcessForAds : BaseSingleton<PlayerAcessForAds>
{
    [SerializeField] private PlayerComponents _playerComponents;
    public PlayerComponents PlayerComponents => _playerComponents;
}
