using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSoundTrigger : MonoBehaviour
{
    [SerializeField] private SoundManager.SoundTypes _soundType;
    [SerializeField] private float _sfxSoundInterval;
    [SerializeField] private bool _soundBasedOnVelocity;
    [SerializeField] private float _baseAudioSourceRangeMultiplyer = 2f;

    [SerializeField] private bool _playSoundOnTriggerEnter;
    //[SerializeField] private SoundManager.MusicAudioData _musicConfigOnTriggerEnter;
    [SerializeField, Tooltip("if more than 1 the sound played will be randomized")] private SoundManager.SfxAudioData[] _sfxConfigOnTriggerEnter;

    [SerializeField] private bool _playSoundOnTriggerExit;
    //[SerializeField] private SoundManager.MusicAudioData _musicConfigOnTriggerExit;
    [SerializeField, Tooltip("if more than 1 the sound played will be randomized")] private SoundManager.SfxAudioData[] _sfxConfigOnTriggerExit;

    [SerializeField] private bool _playSoundOnCollisionEnter;
    //[SerializeField] private SoundManager.MusicAudioData _musicConfigOnCollisionEnter;
    [SerializeField, Tooltip("if more than 1 the sound played will be randomized")] private SoundManager.SfxAudioData[] _sfxConfigOnCollisionEnter;

    private float _lastTimeSfxPlayed;
    private AudioSource _musicAudioSource;

    private enum TriggerTypes
    {
        TriggerEnter,
        TriggerExit,
        CollisionEnter
    }

    private void Awake()
    {
        _musicAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playSoundOnTriggerEnter)
        {
            float soundRange = Vector3.Distance(other.attachedRigidbody.position, other.GetComponent<PlayerComponents>().CurrentCameraPosition.position);
            PlayAudio(TriggerTypes.TriggerEnter, other.attachedRigidbody.velocity.sqrMagnitude, other.attachedRigidbody.position, soundRange * _baseAudioSourceRangeMultiplyer, soundRange * _baseAudioSourceRangeMultiplyer * 1.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_playSoundOnTriggerExit)
        {
            float soundRange = Vector3.Distance(other.attachedRigidbody.position, other.GetComponent<PlayerComponents>().CurrentCameraPosition.position);
            PlayAudio(TriggerTypes.TriggerExit, other.attachedRigidbody.velocity.sqrMagnitude, other.attachedRigidbody.position, soundRange * _baseAudioSourceRangeMultiplyer, soundRange * _baseAudioSourceRangeMultiplyer * 1.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_playSoundOnCollisionEnter)
        {
            Debug.Log(collision.rigidbody.velocity.sqrMagnitude);
            float soundRange = Vector3.Distance(collision.GetContact(0).point, collision.collider.GetComponent<PlayerComponents>().CurrentCameraPosition.position);
            PlayAudio(TriggerTypes.CollisionEnter, collision.rigidbody.velocity.sqrMagnitude, collision.GetContact(0).point, soundRange * _baseAudioSourceRangeMultiplyer, soundRange * _baseAudioSourceRangeMultiplyer * 1.5f);
        }
    }

    private void PlayAudio(TriggerTypes triggerType, float volume, Vector3 hitPoint, float minSoundRange, float maxSoundRange)
    {
        switch (triggerType)
        {
            case TriggerTypes.TriggerEnter:
                if (_soundType == SoundManager.SoundTypes.MUSIC)
                {
                    _musicAudioSource.Play();
                    //SoundManager.Instance.PlayMusic(_musicConfigOnTriggerEnter);
                    break;
                }
                if (Time.time - _lastTimeSfxPlayed >= _sfxSoundInterval)
                {
                    SoundManager.SfxAudioData temp = _sfxConfigOnTriggerEnter.Length > 1 ? 
                        _sfxConfigOnTriggerEnter[Random.Range(0, _sfxConfigOnTriggerEnter.Length - 1)] : _sfxConfigOnTriggerEnter[0];

                    temp.Volume = _soundBasedOnVelocity ? volume * temp.Volume : temp.Volume;

                    SoundManager.Instance.PlaySoundEffect(temp, hitPoint, minSoundRange, maxSoundRange);                    
                }
                break;
            case TriggerTypes.TriggerExit:
                if (_soundType == SoundManager.SoundTypes.MUSIC)
                {
                    _musicAudioSource.Stop();
                    //SoundManager.Instance.PlayMusic(_musicConfigOnTriggerExit);
                    break;
                }
                if (Time.time - _lastTimeSfxPlayed >= _sfxSoundInterval)
                {
                    SoundManager.SfxAudioData temp = _sfxConfigOnTriggerExit.Length > 1 ?
                        _sfxConfigOnTriggerExit[Random.Range(0, _sfxConfigOnTriggerExit.Length - 1)] : _sfxConfigOnTriggerExit[0];

                    temp.Volume = _soundBasedOnVelocity ? volume * temp.Volume : temp.Volume;

                    SoundManager.Instance.PlaySoundEffect(temp, hitPoint, minSoundRange, maxSoundRange);
                }
                break;
            case TriggerTypes.CollisionEnter:
                if (_soundType == SoundManager.SoundTypes.MUSIC)
                {
                    //SoundManager.Instance.PlayMusic(_musicConfigOnCollisionEnter);
                    break;
                }
                if (Time.time - _lastTimeSfxPlayed >= _sfxSoundInterval)
                {
                    SoundManager.SfxAudioData temp = _sfxConfigOnCollisionEnter.Length > 1 ?
                        _sfxConfigOnCollisionEnter[Random.Range(0, _sfxConfigOnCollisionEnter.Length - 1)] : _sfxConfigOnCollisionEnter[0];

                    temp.Volume = _soundBasedOnVelocity ? volume * temp.Volume : temp.Volume;

                    SoundManager.Instance.PlaySoundEffect(temp, hitPoint, minSoundRange, maxSoundRange);
                }
                break;
        }
        _lastTimeSfxPlayed = Time.time;
    }
}
