/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MySoundManager (version 2.20)
 */

using UnityEngine;
using System.Collections.Generic;

namespace MyClasses
{
    public partial class MySoundManager : MonoBehaviour
    {
        #region ----- Define -----

        private readonly string KEY_BGM_MUTE = "MyBGM_Mute";
        private readonly string KEY_BGM_VOLUME = "MyBGM_Volume";
        private readonly string KEY_SFX_MUTE = "MySFX_Mute";
        private readonly string KEY_SFX_VOLUME = "MySFX_Volume";
        private readonly string KEY_ENABLE_VIBRATE = "MyVibrate";

        #endregion

        #region ----- Variable -----

        private AudioSource _audioSourceBGM;
        private List<AudioSource> _listAudioSourceSFX;
        private Dictionary<string, AudioSource> _dictionaryAudioSource = new Dictionary<string, AudioSource>();

        #endregion

        #region ----- Singleton -----

        private static object _singletonLock = new object();
        private static MySoundManager _instance;

        public static MySoundManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_singletonLock)
                    {
                        _instance = (MySoundManager)FindObjectOfType(typeof(MySoundManager));
                        if (_instance == null)
                        {
                            GameObject obj = new GameObject(typeof(MySoundManager).Name);
                            _instance = obj.AddComponent<MySoundManager>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region ----- Property -----

        public bool IsEnableBGM
        {
            get { return VolumeBGM > 0 && !IsMuteBGM; }
        }

        public bool IsMuteBGM
        {
            get { return PlayerPrefs.GetInt(KEY_BGM_MUTE, 0) == 1; }
            set
            {
                PlayerPrefs.SetInt(KEY_BGM_MUTE, value ? 1 : 0);
                _audioSourceBGM.volume = value ? 0 : VolumeBGM;
            }
        }

        public float VolumeBGM
        {
            get { return PlayerPrefs.GetFloat(KEY_BGM_VOLUME, 1f); }
            set
            {
                float volume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(KEY_BGM_VOLUME, volume);
                _audioSourceBGM.volume = IsMuteBGM ? 0 : volume;
            }
        }

        public bool IsEnableSFX
        {
            get { return VolumeSFX > 0 && !IsMuteSFX; }
        }

        public bool IsMuteSFX
        {
            get { return PlayerPrefs.GetInt(KEY_SFX_MUTE, 0) == 1; }
            set
            {
                PlayerPrefs.SetInt(KEY_SFX_MUTE, value ? 1 : 0);
                float volume = value ? 0 : VolumeSFX;
                for (int i = _listAudioSourceSFX.Count - 1; i >= 0; i--)
                {
                    _listAudioSourceSFX[i].volume = volume;
                }
            }
        }

        public float VolumeSFX
        {
            get { return PlayerPrefs.GetFloat(KEY_SFX_VOLUME, 1f); }
            set
            {
                float volume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(KEY_SFX_VOLUME, volume);
                if (IsMuteSFX)
                {
                    volume = 0;
                }
                for (int i = _listAudioSourceSFX.Count - 1; i >= 0; i--)
                {
                    _listAudioSourceSFX[i].volume = volume;
                }
            }
        }

        public bool IsEnableVibrate
        {
            get { return PlayerPrefs.GetInt(KEY_ENABLE_VIBRATE, 1) == 1; }
            set { PlayerPrefs.SetInt(KEY_ENABLE_VIBRATE, value ? 1 : 0); }
        }

        #endregion

        #region ----- MonoBehaviour Implementation -----

        /// <summary>
        /// Awake.
        /// </summary>
        void Awake()
        {
            _audioSourceBGM = gameObject.AddComponent<AudioSource>();
            _listAudioSourceSFX = new List<AudioSource>();
        }

        #endregion

        #region ----- BGM -----

        /// <summary>
        /// Play a BGM.
        /// </summary>
        /// <param name="isLoop">enable looping for the audio clip</param>
        /// <param name="delayTime">delay time specified in seconds</param>
        public void PlayBGM(string filename, bool isLoop = true, float delayTime = 0)
        {
#if DEBUG_MY_SOUND
            Debug.Log("[" + typeof(MySoundManager).Name + "] <color=#0000FFFF>PlayBGM()</color>: filename=\"" + filename + "\"");
#endif

            AudioClip audioClip = Resources.Load<AudioClip>(filename);
            PlayBGM(audioClip, isLoop, delayTime);
        }

        /// <summary>
        /// Play a BGM.
        /// </summary>
        /// <param name="isLoop">enable looping for the audio clip</param>
        /// <param name="delayTime">delay time specified in seconds</param>
        public void PlayBGM(AudioClip audioClip, bool isLoop = true, float delayTime = 0)
        {
            _audioSourceBGM.clip = audioClip;
            _audioSourceBGM.loop = isLoop;
            _audioSourceBGM.volume = IsMuteBGM ? 0 : VolumeBGM;
            _audioSourceBGM.PlayDelayed(delayTime);
        }

        /// <summary>
        /// Pause BGM.
        /// </summary>
        public void PauseBGM()
        {
            _audioSourceBGM.Pause();
        }

        /// <summary>
        /// Resume BGM.
        /// </summary>
        public void ResumeBGM()
        {
            _audioSourceBGM.UnPause();
        }

        /// <summary>
        /// Stop BGM.
        /// </summary>
        public void StopBGM()
        {
            _audioSourceBGM.Stop();
        }

        /// <summary>
        /// Is BGM playing right now?
        /// </summary>
        public bool IsPlayingBGM()
        {
            return _audioSourceBGM != null && _audioSourceBGM.clip != null && _audioSourceBGM.isPlaying;
        }

        /// <summary>
        /// Is BGM playing right now?
        /// </summary>
        public bool IsPlayingBGM(string filename)
        {
            if (IsPlayingBGM())
            {
                return _audioSourceBGM.clip.name.Equals(filename);
            }

            return false;
        }

        /// <summary>
        /// Is BGM playing right now?
        /// </summary>
        public bool IsPlayingBGM(AudioClip audioClip)
        {
            if (IsPlayingBGM())
            {
                return audioClip != null && _audioSourceBGM.clip.name.Equals(audioClip.name);
            }

            return false;
        }

        #endregion

        #region ----- SFX -----

        /// <summary>
        /// Play a SFX.
        /// </summary>
        /// <param name="delayTime">delay time specified in seconds</param>
        /// <param name="localVolume">adjust local volumne (1 = original volume)</param>
        public AudioSource PlaySFX(string filename, float delayTime = 0, float localVolume = 1)
        {
#if DEBUG_MY_SOUND
            Debug.Log("[" + typeof(MySoundManager).Name + "] <color=#0000FFFF>PlaySFX()</color>: filename=\"" + filename + "\"");
#endif

            AudioClip audioClip = Resources.Load<AudioClip>(filename);
            return PlaySFX(audioClip, delayTime, localVolume);
        }

        /// <summary>
        /// Play a SFX.
        /// </summary>
        /// <param name="delayTime">delay time specified in seconds</param>
        /// <param name="localVolume">adjust local volumne (1 = original volume)</param>
        public AudioSource PlaySFX(AudioClip audioClip, float delayTime = 0, float localVolume = 1)
        {
            AudioSource audioSource = _GetAudioSourceSFX();
            audioSource.clip = audioClip;
            audioSource.volume = IsMuteSFX ? 0 : VolumeSFX * localVolume;
            audioSource.time = 0;
            audioSource.PlayDelayed(delayTime);
            return audioSource;
        }

        /// <summary>
        /// Pause all SFXs.
        /// </summary>
        public void PauseAllSFXs()
        {
            for (int i = _listAudioSourceSFX.Count - 1; i >= 0; i--)
            {
                _listAudioSourceSFX[i].Pause();
            }
        }

        /// <summary>
        /// Resume all SFXs.
        /// </summary>
        public void ResumeAllSFXs()
        {
            for (int i = _listAudioSourceSFX.Count - 1; i >= 0; i--)
            {
                _listAudioSourceSFX[i].UnPause();
            }
        }

        /// <summary>
        /// Stop all SFXs.
        /// </summary>
        public void StopAllSFXs()
        {
            for (int i = _listAudioSourceSFX.Count - 1; i >= 0; i--)
            {
                _listAudioSourceSFX[i].Stop();
            }
        }

        /// <summary>
        /// Return an available audio source.
        /// </summary>
        private AudioSource _GetAudioSourceSFX()
        {
            foreach (AudioSource audioSource in _listAudioSourceSFX)
            {
                if (audioSource.clip == null || (!audioSource.isPlaying && audioSource.time == 0))
                {
                    return audioSource;
                }
            }

            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.playOnAwake = false;
            _listAudioSourceSFX.Add(newAudioSource);
            return newAudioSource;
        }

        #endregion

        #region ----- Vibrate -----

        /// <summary>
        /// Vibrate.
        /// </summary>
        public void Vibrate()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (IsEnableVibrate)
            {
                Handheld.Vibrate();
            }
#endif
        }

        #endregion
    }
}