using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSoundTrigger : MonoBehaviour
{
    [SerializeField] private SoundManager.SoundTypes _soundType;
    [SerializeField] private float _sfxSoundInterval;
    //triggerEnter Variables
    [SerializeField] private bool _playSoundOnTriggerEnter;
    [SerializeField] private SoundManager.MusicAudioData _musicConfigOnTriggerEnter;
    [SerializeField, Tooltip("if more than 1 the sound played will be randomized")] private SoundManager.SfxAudioData[] _sfxConfigOnTriggerEnter;

    [SerializeField] private bool _playSoundOnTriggerExit;
    [SerializeField] private SoundManager.MusicAudioData _musicConfigOnTriggerExit;
    [SerializeField, Tooltip("if more than 1 the sound played will be randomized")] private SoundManager.SfxAudioData[] _sfxConfigOnTriggerExit;

    [SerializeField] private bool _playSoundOnCollisionEnter;
    [SerializeField] private SoundManager.MusicAudioData _musicConfigOnCollisionEnter;
    [SerializeField, Tooltip("if more than 1 the sound played will be randomized")] private SoundManager.SfxAudioData[] _sfxConfigOnCollisionEnter;

    private float _lastTimeSfxPlayed;

    private enum TriggerTypes
    {
        TriggerEnter,
        TriggerExit,
        CollisionEnter
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playSoundOnTriggerEnter)
        {
            PlayAudio(TriggerTypes.TriggerEnter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_playSoundOnTriggerExit)
        {
            PlayAudio(TriggerTypes.TriggerExit);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_playSoundOnCollisionEnter)
        {
            PlayAudio(TriggerTypes.CollisionEnter);
        }
    }

    private void PlayAudio(TriggerTypes triggerType)
    {
        switch (triggerType)
        {
            case TriggerTypes.TriggerEnter:
                if (_soundType == SoundManager.SoundTypes.MUSIC)
                {
                    SoundManager.Instance.PlayMusic(_musicConfigOnTriggerEnter);
                    break;
                }
                if (Time.time - _lastTimeSfxPlayed >= _sfxSoundInterval)
                {
                    SoundManager.Instance.PlaySoundEffect(_sfxConfigOnTriggerEnter.Length > 1 ?
                        _sfxConfigOnTriggerEnter[Random.Range(0, _sfxConfigOnTriggerEnter.Length - 1)] : _sfxConfigOnTriggerEnter[0]);
                    _lastTimeSfxPlayed = Time.time;
                }
                break;
            case TriggerTypes.TriggerExit:
                if (_soundType == SoundManager.SoundTypes.MUSIC)
                {
                    SoundManager.Instance.PlayMusic(_musicConfigOnTriggerExit);
                    break;
                }
                if (Time.time - _lastTimeSfxPlayed >= _sfxSoundInterval)
                {
                    SoundManager.Instance.PlaySoundEffect(-_sfxConfigOnTriggerExit.Length > 1 ?
                        _sfxConfigOnTriggerExit[Random.Range(0, _sfxConfigOnTriggerExit.Length - 1)] : _sfxConfigOnTriggerExit[0]);
                    _lastTimeSfxPlayed = Time.time;
                }
                break;
            case TriggerTypes.CollisionEnter:
                if (_soundType == SoundManager.SoundTypes.MUSIC)
                {
                    SoundManager.Instance.PlayMusic(_musicConfigOnCollisionEnter);
                    break;
                }
                if (Time.time - _lastTimeSfxPlayed >= _sfxSoundInterval)
                {
                    SoundManager.Instance.PlaySoundEffect(_sfxConfigOnCollisionEnter.Length > 1 ?
                        _sfxConfigOnCollisionEnter[Random.Range(0, _sfxConfigOnCollisionEnter.Length - 1)] : _sfxConfigOnCollisionEnter[0]);
                    _lastTimeSfxPlayed = Time.time;
                }
                break;
        }
    }
}
