using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private PlayerComponents _playerComponents;
    [SerializeField] private float _soundsInterval = .2f;
    [SerializeField] private SfxConfigs[] _soundsConfigurations;

    [Serializable]
    private struct SfxConfigs
    {
        public PhysicMaterial SurfaceType;
        public float[] RandomizePicth;
        public AudioClip AudioClip;
        [Min(1f)] public float VolumeMultiplier;
        //public SoundManager.SfxAudioData AudioData;
    }

    private AudioSource _audioSource;
    private float _lastTimeSfxPlayed;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        RecalculateAudioSourceRange();
        _playerComponents.ObjectGrow.OnObjectGrow += RecalculateAudioSourceRange;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time - _lastTimeSfxPlayed >= _soundsInterval)
        {
            PlaySound(collision.collider.sharedMaterial, _playerComponents.PlayerRigidbody.velocity.sqrMagnitude);
            _lastTimeSfxPlayed = Time.time;
        }
    }

    private void PlaySound(PhysicMaterial physicMaterial, float volume)
    {
        for(int i = 0; i < _soundsConfigurations.Length; i++)
        {
            if(_soundsConfigurations[i].SurfaceType == physicMaterial)
            {
                //Debug.Log($"play sound with volume {volume}");
                SoundManager.Instance.PlaySoundEffectInComponent(volume * _soundsConfigurations[i].VolumeMultiplier, false, _soundsConfigurations[i].AudioClip, _soundsConfigurations[i].RandomizePicth, _audioSource);
                //SoundManager.Instance.PlaySoundEffect(_soundsConfigurations[i].AudioData);
                break;
            }
        }
    }

    private void RecalculateAudioSourceRange()
    {
        float range = Vector3.Distance(_playerComponents.PlayerTransform.position, _playerComponents.Camera.position);
        _audioSource.maxDistance = range;
        _audioSource.minDistance = range;
    }
}
