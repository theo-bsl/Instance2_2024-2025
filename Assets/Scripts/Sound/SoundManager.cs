using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Sound
{
    [RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundList[] _sounds;
        [SerializeField] private AudioMixer _audioMixer;
        
        private static SoundManager _instance;
        
        private AudioSource _audioSource;
        private readonly string _mainVolumeStr = "Master";
        private readonly string _sfxVolumeStr = "SFX";
        private readonly string _musicVolumeStr = "Music";
        private readonly string _uiVolumeStr = "UI";

        private void Awake()
        {
            if (!_instance)
                _instance = this;
            else
                Destroy(gameObject);
        }

        public static void PlaySound(SoundType sound, float volume = 1.0f)
        {
            var audioClips = Instance.Sounds[(int)sound].Sounds;
            var clip = audioClips[Random.Range(0, audioClips.Length)];
            
            Instance.AudioSource.PlayOneShot(clip, volume);
        }

        public static void PlaySoundAtPosition(SoundType sound, Vector3 position, float volume = 1.0f)
        {
            var audioClips = Instance.Sounds[(int)sound].Sounds;
            var clip = audioClips[Random.Range(0, audioClips.Length)];
            
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
        
        public void SetVolume(GlobalSoundType soundType, float volume)
        {
            float dBVolume = Mathf.Log10(volume) * 20;
            switch (soundType)
            {
                case GlobalSoundType.Main:
                    _audioMixer.SetFloat(_mainVolumeStr, dBVolume);
                    break;
                case GlobalSoundType.SFX:
                    _audioMixer.SetFloat(_sfxVolumeStr, dBVolume);
                    break;
                case GlobalSoundType.Music:
                    _audioMixer.SetFloat(_musicVolumeStr, dBVolume);
                    break;
                case GlobalSoundType.UI:
                    _audioMixer.SetFloat(_uiVolumeStr, dBVolume);
                    break;
            }
        }
        
        public static SoundManager Instance => _instance;
        public SoundList[] Sounds => _sounds;
        public AudioSource AudioSource => _audioSource;
        
#if UNITY_EDITOR
        private void OnEnable()
        {
            string[] names = Enum.GetNames(typeof(SoundType));
            Array.Resize(ref _sounds, names.Length);
            
            for (var i = 0; i < _sounds.Length; i++)
                _sounds[i].name = names[i];
        }
#endif
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
        [HideInInspector] public string name;
        [SerializeField] private AudioClip[] sounds;
        
        public AudioClip[] Sounds => sounds;
    }
}
