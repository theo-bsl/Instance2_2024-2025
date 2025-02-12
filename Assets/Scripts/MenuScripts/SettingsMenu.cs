using Sound;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace MenuScripts
{
    public class SettingsMenu : NetworkBehaviour
    {
        [SerializeField] private Slider _mainSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _uiSlider;

        public void Start()
        {
            _mainSlider .onValueChanged.AddListener(volume => SoundManager.Instance.SetVolume(GlobalSoundType.Main, volume));
            _sfxSlider  .onValueChanged.AddListener(volume => SoundManager.Instance.SetVolume(GlobalSoundType.SFX, volume));
            _musicSlider.onValueChanged.AddListener(volume => SoundManager.Instance.SetVolume(GlobalSoundType.Music, volume));
            _uiSlider   .onValueChanged.AddListener(volume => SoundManager.Instance.SetVolume(GlobalSoundType.UI, volume));
            
            _mainSlider .value = SoundManager.Instance.GetGlobalVolume(GlobalSoundType.Main);
            _sfxSlider  .value = SoundManager.Instance.GetGlobalVolume(GlobalSoundType.SFX);
            _musicSlider.value = SoundManager.Instance.GetGlobalVolume(GlobalSoundType.Music);
            _uiSlider   .value = SoundManager.Instance.GetGlobalVolume(GlobalSoundType.UI);
        }
    }
}