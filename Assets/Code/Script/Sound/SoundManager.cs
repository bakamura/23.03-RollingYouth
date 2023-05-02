using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class SoundManager : BaseSingleton<SoundManager>
{
    [SerializeField] private AudioSource _sfxSoundSource;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private float _defaultMusicTransitionDuration;

    private Coroutine _musicLerp;
    private const float _musicLerpTick = .02f;
    private WaitForSeconds _delay;

    protected override void Awake()
    {
        base.Awake();
        _delay = new WaitForSeconds(_musicLerpTick);
    }

    public void PlaySoundEffect(float volume, float[] randomizePitch, AudioSource audioSource = null)
    {
        
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="transitionCompletely">if true will make the sound volume go to currentVolume to 0 then curentVolume to volume</param>
    /// <param name="musicClip"></param>
    /// <param name="transitionDuration"></param>
    /// <param name="volume"></param>
    public void PlayMusic(bool transitionCompletely, AudioClip musicClip, float transitionDuration, float volume = 0)
    {
        if (_musicAudioSource.isPlaying && _musicLerp == null)
        {
            _musicLerp = StartCoroutine(SoundLerp(transitionCompletely, musicClip, _musicAudioSource, transitionDuration, volume));
        }
        else
        {
            _musicAudioSource.clip = musicClip;
            _musicAudioSource.volume = volume;
            _musicAudioSource.Play();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="transitionCompletely">if true will make the sound volume go to currentVolume to 0 then curentVolume to volume</param>
    /// <param name="clip"></param>
    /// <param name="audioSource"></param>
    /// <param name="volume"></param>
    /// <param name="transitionDuration"></param>
    /// <returns></returns>
    private IEnumerator SoundLerp(bool transitionCompletely, AudioClip clip, AudioSource audioSource, float volume, float transitionDuration = 0)
    {
        Contract.Invariant(volume < 0);
        bool operationCompleted = false;
        float duration = transitionDuration > 0 ? transitionDuration : _defaultMusicTransitionDuration;
        float delta = 0;
        float currentVolume = audioSource.volume;
        float tempVolume = transitionCompletely ? 0 : volume;
        while (!operationCompleted)
        {
            audioSource.volume = Mathf.Lerp(currentVolume, tempVolume, delta);
            delta += _musicLerpTick * duration;
            if(delta >= 1)
            {
                if(transitionCompletely)
                {
                    currentVolume = audioSource.volume;
                    tempVolume = volume;
                    transitionCompletely = false;
                    delta = 0;
                }
                else
                {
                    operationCompleted = true;
                }
            }
            yield return _delay;
        }
        _musicLerp = null;
    }
}
