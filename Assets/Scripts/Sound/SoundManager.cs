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

        public static void PlaySound(SoundType sound, float volume = 1.0f)
        {
            AudioClip[] audioClips = Array.Empty<AudioClip>();

            foreach (var soundList in Instance.Sounds)
            {
                if (soundList.soundType == sound)
                {
                    audioClips = soundList.sounds;
                    break;
                }
            }
            
            AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];
            Instance.AudioSource.PlayOneShot(clip, volume);
        }

        public static void PlaySoundAtPosition(SoundType sound, Vector3 position, float volume = 1.0f)
        {
            AudioClip[] audioClips = Array.Empty<AudioClip>();

            foreach (var soundList in Instance.Sounds)
            {
                if (soundList.soundType == sound)
                {
                    audioClips = soundList.sounds;
                    break;
                }
            }
            
            AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
        
        public void SetVolume(ulong playerId, GlobalSoundType soundType, float volume)
        {
            float dBVolume = Mathf.Log10(volume) * 20;
            switch (soundType)
            {
                case GlobalSoundType.Main:
                    _audioMixer.SetFloat(_mainVolumeStr, dBVolume);
                    PlayerPrefs.SetFloat(_mainVolumeStr + playerId, dBVolume);
                    break;
                case GlobalSoundType.SFX:
                    _audioMixer.SetFloat(_sfxVolumeStr, dBVolume);
                    PlayerPrefs.SetFloat(_sfxVolumeStr + playerId, dBVolume);
                    break;
                case GlobalSoundType.Music:
                    _audioMixer.SetFloat(_musicVolumeStr, dBVolume);
                    PlayerPrefs.SetFloat(_musicVolumeStr + playerId, dBVolume);
                    break;
                case GlobalSoundType.UI:
                    _audioMixer.SetFloat(_uiVolumeStr, dBVolume);
                    PlayerPrefs.SetFloat(_uiVolumeStr + playerId, dBVolume);
                    break;
            }
        }

        public float GetVolume(ulong playerId, GlobalSoundType soundType)
        {
            float volume;
            
            switch (soundType)
            {
                case GlobalSoundType.Main:
                    _audioMixer.GetFloat(_mainVolumeStr + playerId, out volume);
                    return volume;
                case GlobalSoundType.SFX:
                    _audioMixer.GetFloat(_sfxVolumeStr + playerId, out volume);
                    return volume;
                case GlobalSoundType.Music:
                    _audioMixer.GetFloat(_musicVolumeStr + playerId, out volume);
                    return volume;
                case GlobalSoundType.UI:
                    _audioMixer.GetFloat(_uiVolumeStr + playerId, out volume);
                    return volume;
            }
            
            return 0f;
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
        public AudioClip[] sounds;
    }
}
