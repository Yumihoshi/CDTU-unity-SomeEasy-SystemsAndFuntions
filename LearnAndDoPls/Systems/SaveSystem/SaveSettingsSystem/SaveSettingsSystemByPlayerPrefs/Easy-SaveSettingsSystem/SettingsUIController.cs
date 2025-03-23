using UnityEngine;
using UnityEngine.UI;

 namespace SaveSettingsSystem{

    /// <summary>
    /// Controls the UI elements for audio settings in the game.
    /// </summary>
    /// <remarks>
    /// This controller manages the connection between UI sliders and the audio settings system.
    /// It handles initialization of slider values, updates UI when settings change,
    /// and propagates user interactions to the settings system.
    /// </remarks>
    public class SettingsUIController : MonoBehaviour
    {
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider bgmVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        private void Start()
        {
            // 绑定UI
            masterVolumeSlider.value = SimpleAudioSettings.MasterVolume;
            bgmVolumeSlider.value = SimpleAudioSettings.BGMVolume;
            sfxVolumeSlider.value = SimpleAudioSettings.SFXVolume;

            // 添加监听
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

            // 监听设置变更
            SimpleSettingsManager.Instance.SettingsChanged += UpdateUI;
        }

        private void OnDestroy()
        {
            // 移除监听
            SimpleSettingsManager.Instance.SettingsChanged -= UpdateUI;
        }

        // UI更新
        private void UpdateUI()
        {
            masterVolumeSlider.value = SimpleAudioSettings.MasterVolume;
            bgmVolumeSlider.value = SimpleAudioSettings.BGMVolume;
            sfxVolumeSlider.value = SimpleAudioSettings.SFXVolume;
        }

        // 设置更新
        private void OnMasterVolumeChanged(float value) => SimpleAudioSettings.MasterVolume = value;
        private void OnBGMVolumeChanged(float value) => SimpleAudioSettings.BGMVolume = value;
        private void OnSFXVolumeChanged(float value) => SimpleAudioSettings.SFXVolume = value;

        // 重置按钮
        public void ResetSettings()
        {
            SimpleSettingsManager.Instance.ResetToDefaults();
        }
    }
}