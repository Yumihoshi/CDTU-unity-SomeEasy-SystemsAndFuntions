using UnityEngine;

/// <summary>
/// UI控制器基类
/// 提供UI控制器的基本实现
/// </summary>
public abstract class UIViewControllerBase : MonoBehaviour, IUIViewController
{
    protected bool isInitialized;
    protected bool isPaused;
    protected IUIData currentData;

    private event System.Action _onUpdate;
    public event System.Action OnUpdateEvent
    {
        add => _onUpdate += value;
        remove => _onUpdate -= value;
    }

    #region IUIViewController Implementation

    public virtual void Initialize()
    {
        if (isInitialized) return;
        isInitialized = true;
        OnInitialized();
    }

    protected virtual void OnInitialized()
    {
        // 子类重写以添加初始化逻辑
    }

    public virtual void OnOpen()
    {
        if (!isInitialized)
        {
            Initialize();
        }
        gameObject.SetActive(true);
    }

    public virtual void OnResume()
    {
        isPaused = false;
    }

    public virtual void OnPause()
    {
        isPaused = true;
    }

    public virtual void OnClose()
    {
        gameObject.SetActive(false);
        currentData = null;
    }

    public virtual void SetData(IUIData data)
    {
        if (data == null)
        {
            Debug.LogWarning($"[{gameObject.name}] 设置了空数据");
            return;
        }
        currentData = data;
        OnDataChanged();
    }

    protected virtual void OnDataChanged()
    {
        // 子类重写以处理数据变更
    }

    public bool IsInitialized() => isInitialized;
    public bool IsPaused() => isPaused;
    public bool IsActive() => gameObject.activeSelf;
    public bool HasValidData() => currentData != null && currentData.IsValid();

    #endregion

    #region Unity Lifecycle

    protected virtual void Update()
    {
        if (!isPaused && isInitialized)
        {
            _onUpdate?.Invoke();
        }
    }

    protected virtual void OnDisable()
    {
        _onUpdate = null;
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// 获取当前UI数据
    /// </summary>
    protected T GetData<T>() where T : class, IUIData
    {
        return currentData as T;
    }

    /// <summary>
    /// 获取或添加组件
    /// </summary>
    protected T GetOrAddComponent<T>() where T : Component
    {
        T component = GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }

    /// <summary>
    /// 在子物体中查找或添加组件
    /// </summary>
    protected T GetOrAddComponentInChildren<T>(string childPath) where T : Component
    {
        Transform child = transform.Find(childPath);
        if (child == null)
        {
            Debug.LogWarning($"[{gameObject.name}] 找不到子物体: {childPath}");
            return null;
        }

        T component = child.GetComponent<T>();
        if (component == null)
        {
            component = child.gameObject.AddComponent<T>();
        }
        return component;
    }

    /// <summary>
    /// 查找子物体
    /// </summary>
    protected Transform FindChild(string path)
    {
        Transform child = transform.Find(path);
        if (child == null)
        {
            Debug.LogWarning($"[{gameObject.name}] 找不到子物体: {path}");
        }
        return child;
    }

    #endregion
}