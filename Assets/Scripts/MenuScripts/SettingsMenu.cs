using Sound;
using UnityEngine;
using UnityEngine.UI;

namespace MenuScripts
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Slider _mainSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _uiSlider;

        void Start()
        {
            _mainSlider .onValueChanged.AddListener(volume => SoundManager.Instance.SetVolume(GlobalSoundType.Main, volume));
            _sfxSlider  .onValueChanged.AddListener(volume => SoundManager.Instance.SetVolume(GlobalSoundType.SFX, volume));
            _musicSlider.onValueChanged.AddListener(volume => SoundManager.Instance.SetVolume(GlobalSoundType.Music, volume));
            _uiSlider   .onValueChanged.AddListener(volume => SoundManager.Instance.SetVolume(GlobalSoundType.UI, volume));
        }
    }
}