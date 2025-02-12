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

        public void Awake()
        {
            if (!IsOwner)
            {
                enabled = false;
                return;
            }
            
            _mainSlider .onValueChanged.AddListener(volume => SoundManager.Instance.SetVolume(NetworkObjectId, GlobalSoundType.Main, volume));
            _sfxSlider  .onValueChanged.AddListener(volume => SoundManager.Instance.SetVolume(NetworkObjectId, GlobalSoundType.SFX, volume));
            _musicSlider.onValueChanged.AddListener(volume => SoundManager.Instance.SetVolume(NetworkObjectId, GlobalSoundType.Music, volume));
            _uiSlider   .onValueChanged.AddListener(volume => SoundManager.Instance.SetVolume(NetworkObjectId, GlobalSoundType.UI, volume));
            
            _mainSlider.value  = SoundManager.Instance.GetVolume(NetworkObjectId, GlobalSoundType.Main);
            _sfxSlider.value   = SoundManager.Instance.GetVolume(NetworkObjectId, GlobalSoundType.SFX);
            _musicSlider.value = SoundManager.Instance.GetVolume(NetworkObjectId, GlobalSoundType.Music);
            _uiSlider.value    = SoundManager.Instance.GetVolume(NetworkObjectId, GlobalSoundType.UI);
        }
    }
}