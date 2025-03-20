/// <summary>
/// UI层级定义
/// </summary>
public enum UILayer
{
    /// <summary>
    /// 场景层：用于显示3D模型和场景内UI
    /// </summary>
    SceneLayer = 0,
    
    /// <summary>
    /// 背景层：用于显示背景和主菜单背景
    /// </summary>
    BackgroundLayer = 1000,
    
    /// <summary>
    /// 普通层：用于显示主要功能面板
    /// </summary>
    NormalLayer = 2000,
    
    /// <summary>
    /// 信息层：用于显示状态信息和提示
    /// </summary>
    InfoLayer = 3000,
    
    /// <summary>
    /// 顶层：用于显示弹窗和对话框
    /// </summary>
    TopLayer = 4000,
    
    /// <summary>
    /// 提示层：用于显示工具提示和通知
    /// </summary>
    TipLayer = 5000
}
