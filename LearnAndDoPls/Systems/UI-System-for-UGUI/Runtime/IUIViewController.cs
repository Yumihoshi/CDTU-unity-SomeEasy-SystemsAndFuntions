using UnityEngine;

/// <summary>
/// UI控制器接口
/// 定义UI控制器的基本行为
/// </summary>
public interface IUIViewController
{
    /// <summary>
    /// 初始化
    /// </summary>
    void Initialize();
    
    /// <summary>
    /// 打开视图
    /// </summary>
    void OnOpen();
    
    /// <summary>
    /// 恢复视图
    /// </summary>
    void OnResume();
    
    /// <summary>
    /// 暂停视图
    /// </summary>
    void OnPause();
    
    /// <summary>
    /// 关闭视图
    /// </summary>
    void OnClose();
    
    /// <summary>
    /// 设置数据
    /// </summary>
    void SetData(IUIData data);
    
    /// <summary>
    /// 检查是否已初始化
    /// </summary>
    bool IsInitialized();
    
    /// <summary>
    /// 检查是否暂停
    /// </summary>
    bool IsPaused();
    
    /// <summary>
    /// 检查是否激活
    /// </summary>
    bool IsActive();
    
    /// <summary>
    /// 检查是否有有效数据
    /// </summary>
    bool HasValidData();
}