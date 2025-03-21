public class AudioManager : BaseSettingsManager<AudioSettings>
{
    // 添加单例实现
    private static AudioManager instance;
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

    // ...existing code...

    protected void Awake()
    {
        // 检查是否存在其他实例
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        // 此处我们允许每个场景有自己的AudioManager
        // 如果需要在场景之间保持，可以在特定场景中手动调用DontDestroyOnLoad
        InitializeAudioSources();
    }

    // ...existing code...