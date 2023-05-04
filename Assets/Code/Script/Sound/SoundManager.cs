using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : BaseSingleton<SoundManager>
{
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private float _defaultMusicTransitionDuration;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;

    private Coroutine _musicLerp;
    private const float _musicLerpTick = .02f;
    private WaitForSeconds _delay;
    private List<AudioSource> _sfxAudioSources = new List<AudioSource>();
    private Queue<MusicAudioData> _musicQueue = new Queue<MusicAudioData>();

    #region "AudioDatas"
    [System.Serializable]
    public struct MusicAudioData
    {
        public bool TransitionCompletely;
        public AudioClip Clip;
        public float Volume;
        public float TransitionDuration;

        public MusicAudioData(bool transitionCompletely, AudioClip clip, float volume, float transitionDuration = 0)
        {
            Contract.Ensures(volume >= 0 && volume <= 1);
            TransitionCompletely = transitionCompletely;
            TransitionDuration = transitionDuration;
            Clip = clip;
            Volume = volume;
        }
    }

    [System.Serializable]    
    public struct SfxAudioData
    {
        public AudioClip Clip;
        public float Volume;
        public Vector3 SoundOrigin;
        public bool IsSoundSpatial;
        public float[] RandomizePitch;
        public AudioSource AudioSource;
        //public float AudioInterval;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="volume"></param>
        /// <param name="soundOrigin"></param>
        /// <param name="isSoundSpatial"></param>
        /// <param name="randomizePitch">an array with the size of 2. 0 = min value, 1 = max value</param>
        /// <param name="audioSource"></param>
        public SfxAudioData(AudioClip clip, float volume, Vector3 soundOrigin, /*float audioInterval,*/ bool isSoundSpatial = true, float[] randomizePitch = null, AudioSource audioSource = null)
        {
            Contract.Ensures(volume >= 0 && volume <= 1);
            Contract.Ensures(randomizePitch.Length == 2);
            Clip = clip;
            Volume = volume;
            SoundOrigin = soundOrigin;
            IsSoundSpatial = isSoundSpatial;
            RandomizePitch = randomizePitch;
            AudioSource = audioSource;
            //AudioInterval = audioInterval;
        }
    }

    public enum SoundTypes
    {
        MUSIC,
        SFX
    }
    #endregion

#if UNITY_EDITOR
    [SerializeField] private bool _debugMessages;
#endif

    protected override void Awake()
    {
        base.Awake();
        _delay = new WaitForSeconds(_musicLerpTick);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="volume"></param>
    /// <param name="randomizePitch">an array with the size of 2. 0 = min value, 1 = max value </param>
    /// <param name="audioSource"></param>
    public void PlaySoundEffectInComponent(float volume, bool overrideCurrentSoundEffect, float[] randomizePitch = null, AudioSource audioSource = null)
    {
        Contract.Ensures(volume >= 0 && volume <= 1);
        Contract.Ensures(randomizePitch.Length == 2);
        if (!audioSource.isPlaying || overrideCurrentSoundEffect)
        {
            if (audioSource.isPlaying) audioSource.Stop();
            audioSource.volume = volume;
            audioSource.pitch = randomizePitch != null ? Random.Range(randomizePitch[0], randomizePitch[1]) : Random.Range(-3f, 3f);
            audioSource.Play();
        }
#if UNITY_EDITOR
        else
        {
            if (_debugMessages)
                Debug.Log($"there is already a sound playing in {audioSource.gameObject.name}");
        }
#endif
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    /// <param name="soundOrigin"></param>
    /// <param name="isSoundSpatial"></param>
    /// <param name="randomizePitch">an array with the size of 2. 0 = min value, 1 = max value</param>
    public void PlaySoundEffect(SfxAudioData data/*AudioClip clip, float volume, Vector3 soundOrigin, bool isSoundSpatial = true, float[] randomizePitch = null*/)
    {
        Contract.Ensures(data.Volume >= 0 && data.Volume <= 1);
        Contract.Ensures(data.RandomizePitch.Length == 2);

        AudioSource audioSource = GetAvailableSfxSoundPlayer();

        audioSource.clip = data.Clip;
        audioSource.volume = data.Volume;
        audioSource.spatialBlend = data.IsSoundSpatial ? 1 : 0;
        audioSource.pitch = data.RandomizePitch != null ? Random.Range(data.RandomizePitch[0], data.RandomizePitch[1]) : Random.Range(-3f, 3f);
        audioSource.gameObject.transform.position = data.SoundOrigin;
        audioSource.Play();
    }

    private AudioSource GetAvailableSfxSoundPlayer()
    {
        for (int i = 0; i < _sfxAudioSources.Count; i++)
        {
            if (!_sfxAudioSources[i].isPlaying) return _sfxAudioSources[i];
        }

        GameObject temp = new GameObject("SfxSoundSource");
        AudioSource audioSource = temp.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = _sfxMixerGroup;
        temp.transform.SetParent(transform);
        _sfxAudioSources.Add(audioSource);
        return audioSource;
    }

    public void PlayMusic(MusicAudioData data)
    {
        _musicQueue.Enqueue(data);
        if (_musicLerp == null) _musicLerp = StartCoroutine(MusicSoundLerp(_musicQueue.Dequeue()));
    }

    private IEnumerator MusicSoundLerp(MusicAudioData data)
    {
        bool operationCompleted = false;
        float duration = data.TransitionDuration > 0 ? data.TransitionDuration : _defaultMusicTransitionDuration;
        float delta = 0;
        float currentVolume = _musicAudioSource.volume;
        float tempVolume = data.TransitionCompletely ? 0 : data.Volume;
        while (!operationCompleted)
        {
            _musicAudioSource.volume = Mathf.Lerp(currentVolume, tempVolume, delta);
            delta += _musicLerpTick * duration;
            if (delta >= 1)
            {
                if (data.TransitionCompletely)
                {
                    currentVolume = _musicAudioSource.volume;
                    tempVolume = data.Volume;
                    data.TransitionCompletely = false;
                    delta = 0;
                }
                else
                {
                    operationCompleted = true;
                }
                _musicAudioSource.clip = data.Clip;
            }
            yield return _delay;
        }
        _musicLerp = null;
        if (_musicQueue.Count > 0)
        {
            PlayMusic(_musicQueue.Dequeue());
        }
    }
}
