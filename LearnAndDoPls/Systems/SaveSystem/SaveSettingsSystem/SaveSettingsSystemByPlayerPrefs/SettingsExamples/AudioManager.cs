using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Collections;
using SaveSystem;

/// <summary>
/// 音频设置管理类，负责处理和存储所有音频相关的设置。
/// <para>
/// 该类管理三种音量设置：
/// - 主音量 (Master Volume)
/// - 背景音乐音量 (BGM Volume)
/// - 音效音量 (SFX Volume)
/// </para>
/// <para>
/// 特性：
/// - 实现数据持久化存储（通过 PlayerPrefs）
/// - 提供音量范围限制（0-1）
/// - 支持音量设置的事件通知
/// - 提供实际音量计算（考虑主音量影响）
/// </para>
/// </summary>
public class AudioManager : BaseSettingsManager<AudioSettings>
{
    // 添加单例实现
    private static AudioManager instance;
    [SerializeField] private bool dontDestroyOnLoad = false; // 添加可选的DontDestroyOnLoad标志

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<AudioManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("AudioManager");
                    instance = go.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    [Header("音量数据")]
    [SerializeField] private AudioSettingsSO audioSettingsSO;
    private AudioSettings audioSettings;

    // 重命名事件以避免与基类冲突
    public event EventHandler<EventArgs> AudioVolumeChanged;

    public override event EventHandler SettingsChanged;

    #region 音频源配置
    [Header("音频源/播放器")]
    [Tooltip("背景音乐播放器")]
    [SerializeField] private AudioSource bgmSource;
    [Tooltip("音效播放器列表")]
    private List<AudioSource> sfxSourceList = new();

    [Header("音频剪辑数据")]
    private Dictionary<string, AudioClip> bgmClipDictionary = new();
    private Dictionary<string, AudioClip> sfxClipDictionary = new();

    [Serializable]
    public class BGMClipData
    {
        public string name;
        public AudioClip clip;
    }

    [Serializable]
    public class SFXClipData
    {
        public string name;
        public AudioClip clip;
    }

    [SerializeField] private List<BGMClipData> audioClipList = new();
    [SerializeField] private List<SFXClipData> sfxClipList = new();

    [Header("音量控制UI")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    // 音量改变事件
    public event EventHandler<EventArgs> OnVolumeChanged;
    #endregion

    protected override void InitializeSettings()
    {
        if (audioSettingsSO == null)
        {
            Debug.LogError($"{nameof(AudioManager)}: audioSettingsSO is not assigned!");
            return;
        }
        if (bgmSource == null)
        {
            Debug.LogError($"{nameof(AudioManager)}: bgmSource is not assigned!");
        }

        audioSettings = new AudioSettings(audioSettingsSO);
        audioSettings.OnVolumeChanged += HandleVolumeChanged;

        // 订阅设置变更事件
        if (audioSettings is ISaveSettings saveSettings)
        {
            saveSettings.SettingsChanged += HandleSettingsChanged;
        }
        
        // 初始化完成后绑定UI和加载设置
        BindVolumeSliders();
        Load();
        UpdateUI();
    }

    protected override void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
        
        base.Awake();
        InitializeAudioSources(); // 在base.Awake()之后初始化音频源
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (audioSettings != null)
        {
            audioSettings.OnVolumeChanged -= HandleVolumeChanged;

            // 取消订阅事件，防止内存泄漏
            if (audioSettings is ISaveSettings saveSettings)
            {
                saveSettings.SettingsChanged -= HandleSettingsChanged;
            }
        }
    }

    /// <summary>
    /// 处理设置变更事件
    /// </summary>
    protected override void HandleSettingsChanged(object sender, EventArgs e)
    {
        base.HandleSettingsChanged(sender, e);
        // 将设置变更事件向上传播
        SettingsChanged?.Invoke(this, e);
    }

    private void BindVolumeSliders()
    {
        // 绑定滑动条事件
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(value => audioSettings.MasterVolume = value);
#if UNITY_EDITOR
        else
            Debug.LogWarning("masterVolumeSlider is null");
#endif

        if (bgmVolumeSlider != null)
            bgmVolumeSlider.onValueChanged.AddListener(value => audioSettings.BGMVolume = value);
#if UNITY_EDITOR
        else
            Debug.LogWarning("bgmVolumeSlider is null");
#endif

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(value => audioSettings.SFXVolume = value);
#if UNITY_EDITOR
        else
            Debug.LogWarning("sfxVolumeSlider is null");
#endif
    }

    /// <summary>
    /// 音量改变时的处理方法.通过事件audioSettings.OnVolumeChanged激活
    /// 更新所有音源的音量
    /// </summary> <summary>
    /// 更新音量条
    /// </summary>
    private void HandleVolumeChanged()
    {
        UpdateVolumes();
        //todo-可以自己考虑将sliders逻辑分离，但是我模块化就算了
        UpdateSliders();
        // 触发事件通知外部监听者，外部观察者可以订阅
        AudioVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateSliders()
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = audioSettings.MasterVolume;
        if (bgmVolumeSlider != null)
            bgmVolumeSlider.value = audioSettings.BGMVolume;
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = audioSettings.SFXVolume;
    }

    private void UpdateUI()
    {
        UpdateSliders();
        UpdateVolumes();
    }

    #region 初始化方法
    /// <summary>
    /// 初始化音频源和音频剪辑字典
    /// </summary>
    private void InitializeAudioSources()
    {
        // 初始化BGM字典
        foreach (var clipData in audioClipList)
        {
            if (clipData.clip != null && !string.IsNullOrEmpty(clipData.name))
            {
                bgmClipDictionary[clipData.name] = clipData.clip;
            }
        }

        // 初始化SFX字典
        foreach (var clipData in sfxClipList)
        {
            if (clipData.clip != null && !string.IsNullOrEmpty(clipData.name))
            {
                sfxClipDictionary[clipData.name] = clipData.clip;
            }
        }
    }
    #endregion

    #region 音频播放控制
    /// <summary>
    /// 通过名称播放背景音乐
    /// </summary>
    /// <param name="BGMname"></param>
    public void PlayBGMByName(string BGMname)
    {
        if (string.IsNullOrEmpty(BGMname)) return;

        if (bgmClipDictionary.TryGetValue(BGMname, out AudioClip clip))
        {
            PlayBGM(clip);
        }
        else
        {
            Debug.LogWarning($"未找到名为 {BGMname} 的背景音乐");
        }
    }

    /// <summary>
    /// 通过名称播放音效
    /// </summary>
    /// <param name="SFXname">音效名称</param>
    public void PlaySFXByName(string SFXname)
    {
        if (string.IsNullOrEmpty(SFXname)) return;

        if (sfxClipDictionary.TryGetValue(SFXname, out AudioClip clip))
        {
            PlaySFX(clip);
        }
        else
        {
            Debug.LogWarning($"未找到名为 {SFXname} 的音效");
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        bgmSource.clip = clip;
        bgmSource.volume = audioSettings.GetActualBGMVolume();
        bgmSource.Play();
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBGM()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.clip = clip;
        sfxSource.volume = audioSettings.GetActualSFXVolume();
        sfxSource.Play();

        sfxSourceList.Add(sfxSource);
        StartCoroutine(RemoveSFXSource(sfxSource, clip.length));
    }

    /// <summary>
    /// 延迟销毁音效音源
    /// </summary>
    private IEnumerator RemoveSFXSource(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (sfxSourceList.Contains(source))
        {
            sfxSourceList.Remove(source);
        }
        Destroy(source);
    }

    /// <summary>
    /// 更新所有音源的音量
    /// </summary>
    private void UpdateVolumes()
    {
        if (audioSettings == null) return;

        if (bgmSource != null && bgmSource.clip != null)
        {
            bgmSource.volume = audioSettings.GetActualBGMVolume();
        }

        foreach (var source in sfxSourceList.ToArray())
        {
            if (source != null)
            {
                source.volume = audioSettings.GetActualSFXVolume();
            }
        }
    }
    #endregion

    #region 保存数据接口
    public override void Save()
    {
        if (audioSettings != null)
        {
            audioSettings.Save();
        }
    }

    public override void Load()
    {
        if (audioSettings != null)
        {
            audioSettings.Load();
        }
    }

    public override void ResetToDefault()
    {
        if (audioSettings != null)
        {
            audioSettings.ResetToDefault();
        }
    }
    #endregion
}

