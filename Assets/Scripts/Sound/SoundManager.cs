using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : SingletonPersistent<SoundManager>
    {
        [SerializeField] private SoundList[] _sounds;
        [SerializeField] private AudioMixer _audioMixer;
        
        private AudioSource _audioSource;
        private readonly string _mainVolumeStr = "Master";
        private readonly string _sfxVolumeStr = "SFX";
        private readonly string _musicVolumeStr = "Music";
        private readonly string _uiVolumeStr = "UI";
        
        public void PlaySound(SoundType soundType)
        {
            AudioClip[] audioClips = GetSounds(soundType);
            AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];

            float volume = GetSoundVolume(GetGlobalSoundType(soundType));
            
            Instance.AudioSource.PlayOneShot(clip, volume);
        }

        public void PlaySoundAtPosition(SoundType soundType, Vector3 position)
        {
            AudioClip[] audioClips = GetSounds(soundType);
            AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];

            float volume = GetSoundVolume(GetGlobalSoundType(soundType));
            
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
        
        public void SetVolume(GlobalSoundType soundType, float volume)
        {
            float dBVolume = Mathf.Log10(volume) * 20;
            switch (soundType)
            {
                case GlobalSoundType.Main:
                    _audioMixer.SetFloat(_mainVolumeStr, dBVolume);
                    PlayerPrefs.SetFloat(_mainVolumeStr, dBVolume);
                    break;
                case GlobalSoundType.SFX:
                    _audioMixer.SetFloat(_sfxVolumeStr, dBVolume);
                    PlayerPrefs.SetFloat(_sfxVolumeStr, dBVolume);
                    break;
                case GlobalSoundType.Music:
                    _audioMixer.SetFloat(_musicVolumeStr, dBVolume);
                    PlayerPrefs.SetFloat(_musicVolumeStr, dBVolume);
                    break;
                case GlobalSoundType.UI:
                    _audioMixer.SetFloat(_uiVolumeStr, dBVolume);
                    PlayerPrefs.SetFloat(_uiVolumeStr, dBVolume);
                    break;
            }
        }

        public float GetGlobalVolume(GlobalSoundType globalSoundType)
        {
            float dBVolume = 0;
            
            switch (globalSoundType)
            {
                case GlobalSoundType.Main:
                    _audioMixer.GetFloat(_mainVolumeStr, out dBVolume);
                    break;
                case GlobalSoundType.SFX:
                    _audioMixer.GetFloat(_sfxVolumeStr, out dBVolume);
                    break;
                case GlobalSoundType.Music:
                    _audioMixer.GetFloat(_musicVolumeStr, out dBVolume);
                    break;
                case GlobalSoundType.UI:
                    _audioMixer.GetFloat(_uiVolumeStr, out dBVolume);
                    break;
            }
            
            return Mathf.Pow(10f, dBVolume / 20f);
        }

        private float GetSoundVolume(GlobalSoundType globalSoundType)
        {
            _audioMixer.GetFloat(_mainVolumeStr, out var dbMainVolume);
            
            float dBVolume = 0;
            
            switch (globalSoundType)
            {
                case GlobalSoundType.SFX:
                    _audioMixer.GetFloat(_sfxVolumeStr, out dBVolume);
                    break;
                case GlobalSoundType.Music:
                    _audioMixer.GetFloat(_musicVolumeStr, out dBVolume);
                    break;
                case GlobalSoundType.UI:
                    _audioMixer.GetFloat(_uiVolumeStr, out dBVolume);
                    break;
            }
            
            float linearVolume = Mathf.Pow(10f, dBVolume / 20f);
            float linearMainVolume = Mathf.Pow(10f, dbMainVolume / 20f);
            
            return linearMainVolume * linearVolume; 
        }

        private AudioClip[] GetSounds(SoundType soundType)
        {
            foreach (var soundList in _sounds)
                if (soundList.soundType == soundType)
                    return soundList.sounds;
            
            return Array.Empty<AudioClip>();
        }

        private GlobalSoundType GetGlobalSoundType(SoundType soundType)
        {
            foreach (var soundList in _sounds)
                if (soundList.soundType == soundType)
                    return soundList.globalSoundType;

            return GlobalSoundType.Main;
        }
        
        public SoundList[] Sounds => _sounds;
        public AudioSource AudioSource => _audioSource;
    }

    public enum GlobalSoundType
    {
        Main, SFX, Music, UI
    }
        
    public enum SoundType
    {
        Freeze, SpeedUp, SpeedDown, DamageUp, DamageDown, Hit, Bust, Footstep,
        
        StartGame, EndGame
    }

    [Serializable]
    public struct SoundList
    {
        public SoundType soundType;
        public GlobalSoundType globalSoundType;
        public AudioClip[] sounds;
    }
}
