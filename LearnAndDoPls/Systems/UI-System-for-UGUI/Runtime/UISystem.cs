using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/// <summary>
/// IUISystem 定义了游戏UI系统的核心操作接口，提供了全面的UI生命周期管理，包括：
///
/// 1. UI创建和打开：
///    - 支持层级UI(OpenPanel)和场景内嵌UI(OpenViewInScene)
///    - 类型安全的UI实例化和数据绑定
///    - 自动管理UI层级栈和渲染顺序
/// 
/// 2. UI关闭和销毁：
///    - 支持关闭特定UI、顶层UI或全部UI
///    - 自动处理生命周期回调(OnPause, OnClose)
///    - 支持场景层UI的独立管理
/// 
/// 3. UI状态查询：
///    - 获取指定层的顶层面板
///    - 查找特定类型的UI实例
///    - 确定UI所在层级
/// 
/// 4. 渲染支持：
///    - Canvas自动创建和配置
///    - UI缩放和屏幕适配
///    - 事件系统管理
///
/// 该接口使用泛型设计提供类型安全的UI创建和数据绑定，支持多层UI管理和复杂的UI交互流程。
/// 通过统一的接口约定，确保UI系统的实现具有一致性和可扩展性。
/// </summary>
public interface IUISystem
{
    /// <summary>
    /// <typeparam name="T">T为UI视图类型，必须是UIView的子类且可实例化</typeparam>
    /// <typeparam name="TUIData">TuiData为UI数据类型，必须是引用类型且实现IUIData接口</typeparam>
    /// <returns>创建的UI视图实例</returns>
    /// <exception cref="System.InvalidOperationException">当尝试在SceneLayer使用此方法时抛出</exception>
    /// </summary>
    UIView<TUIData> OpenPanel<T, TUIData>(
        TUIData uiData,
        UILayer uiLayer = UILayer.NormalLayer
    )
    where T : UIView, new()
    where TUIData : class, IUIData;

    /// <summary>
    /// 在场景层中创建一个UI视图，不参与UI栈管理。
    /// 适用于与场景关联的持久性UI元素，如HUD、角色状态显示等。
    /// </summary>
    /// <param name="uiData">初始化UI视图所需的数据</param>
    /// <typeparam name="T">UI视图类型，必须是UIView的子类且可实例化</typeparam>
    /// <typeparam name="TUIData">UI数据类型，必须是引用类型且实现IUIData接口</typeparam>
    /// <returns>创建的UI视图实例</returns>
    UIView<TUIData> OpenViewInScene<T, TUIData>(
       TUIData uiData
    )
    where T : UIView, new()
    where TUIData : class, IUIData;

    /// <summary>
    /// 关闭并销毁场景层中的指定UI视图。
    /// </summary>
    /// <param name="uiView">要关闭的UI视图</param>
    void CloseInSceneLayer(UIView uiView);

    /// <summary>
    /// 通过类型获取场景层中的UI视图。
    /// </summary>
    /// <typeparam name="T">要查找的UI视图类型</typeparam>
    /// <returns>匹配的UI视图，如果不存在则返回null</returns>
    UIView GetInSceneLayer<T>();

    /// <summary>
    /// 检查指定UI视图是否位于场景层中。
    /// </summary>
    /// <param name="uiView">要检查的UI视图</param>
    /// <returns>如果视图在场景层中返回true，否则返回false</returns>
    bool IsInSceneLayer(UIView uiView);

    /// <summary>
    /// 关闭指定层级栈顶的UI面板。
    /// </summary>
    /// <param name="uiLayer">要操作的UI层级</param>
    void CloseTopPanel(UILayer uiLayer);

    /// <summary>
    /// 关闭所有层级中的所有UI面板。
    /// </summary>
    void CloseAll();

    /// <summary>
    /// 重置UI系统，关闭所有面板并清理资源。
    /// </summary>
    void Reset();

    /// <summary>
    /// 获取指定层级栈顶的UI面板。
    /// </summary>
    /// <param name="layer">要查询的UI层级</param>
    /// <returns>栈顶UI面板，如果栈为空则返回null</returns>
    UIView GetTopPanel(UILayer layer);

    /// <summary>
    /// 获取或创建UI面板的画布。
    /// 如果场景中不存在画布，则创建一个新的ScreenSpaceOverlay模式的画布。
    /// </summary>
    /// <returns>UI面板使用的画布</returns>
    Canvas GetOrAddPanelCanvas();
}

/// <summary>
/// UI系统实现类，负责管理所有UI面板和视图的生命周期。
/// 提供面板预加载、创建、显示、隐藏和销毁等功能。
/// 支持多层级UI管理和场景内UI元素管理。
/// </summary>
public class UISystem : IUISystem
{
    private Dictionary<string, GameObject> _panelPrefabDict = new Dictionary<string, GameObject>();
    private List<UIView> _sceneLayerPanelList = new List<UIView>();
    private Dictionary<UILayer, Stack<UIView>> _panelStack = new Dictionary<UILayer, Stack<UIView>>();
    private Canvas _activeCanvas; //面板挂载的Canvas

    #region 通过单例调用 后续可改为其他

    private static UISystem _instance;

    /// <summary>
    /// 获取UI系统的单例实例。
    /// 如果实例不存在，则创建并初始化一个新的实例。
    /// </summary>
    public static UISystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UISystem();
                _instance.OnInit(); // 执行初始化操作
            }

            return _instance;
        }
    }

    /// <summary>
    /// 私有构造函数，确保只能通过Instance属性访问实例。
    /// </summary>
    private UISystem()
    {
        // 私有构造函数，确保只能通过Instance属性访问
    }

    #endregion

    /// <summary>
    /// 初始化UI系统，加载所有UI预制体。
    /// </summary>
    public void OnInit()
    {
        //TODO:通过资源加载系统加载所有Panel的预制体
        foreach (var newPanel in Resources.LoadAll<GameObject>("UIPrefabs"))
        {
            _panelPrefabDict.Add(newPanel.name, newPanel);
        }
    }

    /// <summary>
    /// 创建并打开一个UI面板，将其添加到指定UI层级的栈顶。
    /// </summary>
    /// <param name="uiData">初始化UI面板所需的数据</param>
    /// <param name="uiLayer">UI面板所在的层级，默认为普通层级</param>
    /// <typeparam name="T">UI视图类型</typeparam>
    /// <typeparam name="TUIData">UI数据类型</typeparam>
    /// <returns>创建的UI视图实例</returns>
    /// <exception cref="System.InvalidOperationException">当尝试在SceneLayer使用此方法时抛出</exception>
    public UIView<TUIData> OpenPanel<T, TUIData>
    (
        TUIData uiData,
        UILayer uiLayer = UILayer.NormalLayer
    )
    where T : UIView, new()
    where TUIData : class, IUIData
    {
        if (uiLayer == UILayer.SceneLayer)
        {
            Debug.LogError("场景中的UI请使用OpenViewInScene方法");
        }
        T newPanel = CreateAndInitializePanel<T, TUIData>(uiData, uiLayer);

        if (!_panelStack.ContainsKey(uiLayer))
        {
            _panelStack.Add(uiLayer, new Stack<UIView>());
        }
        _panelStack[uiLayer].Push(newPanel);
        return newPanel as UIView<TUIData>;
    }

    /// <summary>
    /// 在场景层中创建一个UI视图，不参与UI栈管理。
    /// </summary>
    /// <param name="uiData">初始化UI视图所需的数据</param>
    /// <typeparam name="T">UI视图类型</typeparam>
    /// <typeparam name="TUIData">UI数据类型</typeparam>
    /// <returns>创建的UI视图实例</returns>
    public UIView<TUIData> OpenViewInScene<T, TUIData>(TUIData uiData) where T : UIView, new() where TUIData : class, IUIData
    {
        var newPanel = CreateAndInitializePanel<T, TUIData>(uiData, UILayer.SceneLayer);
        _sceneLayerPanelList.Add(newPanel);
        return newPanel as UIView<TUIData>;
    }

    /// <summary>
    /// 创建并初始化UI面板的内部方法。
    /// </summary>
    /// <param name="uiData">UI数据</param>
    /// <param name="uiLayer">UI层级</param>
    /// <typeparam name="T">UI视图类型</typeparam>
    /// <typeparam name="TUIData">UI数据类型</typeparam>
    /// <returns>创建并初始化的UI视图实例</returns>
    private T CreateAndInitializePanel<T, TUIData>(TUIData uiData, UILayer uiLayer) where T : UIView, new() where TUIData : class, IUIData
    {
        //判断场景中是否有画布
        if (_activeCanvas == null) _activeCanvas = GetOrAddPanelCanvas();

        var newPanelObject = Object.Instantiate(_panelPrefabDict[typeof(T).ToString()], _activeCanvas.transform);
        newPanelObject.name = typeof(T).ToString();
        var newPanel = new T
        {
            PanelObject = newPanelObject
        };
        newPanel.UILayer = uiLayer;
        var viewController = newPanel.PanelObject.GetComponent<UIViewControllerBase>();
        if (viewController != null)
        {
            viewController.OnUpdateEvent += newPanel.OnUpdate;
        }

        //传入数据
        newPanel.SetData(uiData);
        newPanel.OnOpen();
        newPanel.OnResume();

        return newPanel;
    }

    /// <summary>
    /// 通过类型获取场景层中的UI视图。
    /// </summary>
    /// <typeparam name="T">要查找的UI视图类型</typeparam>
    /// <returns>匹配的UI视图，如果不存在则返回null</returns>
    public UIView GetInSceneLayer<T>()
    {
        foreach (var view in _sceneLayerPanelList)
        {
            if (view.GetType() == typeof(T))
            {
                return view;
            }
        }

        return null;
    }

    /// <summary>
    /// 检查指定UI视图是否位于场景层中。
    /// </summary>
    /// <param name="uiView">要检查的UI视图</param>
    /// <returns>如果视图在场景层中返回true，否则返回false</returns>
    public bool IsInSceneLayer(UIView uiView)
    {
        return _sceneLayerPanelList.Contains(uiView);
    }

    /// <summary>
    /// 关闭指定层级栈顶的UI面板。
    /// 关闭后会自动激活栈中下一个面板。
    /// </summary>
    /// <param name="uiLayer">要操作的UI层级</param>
    public void CloseTopPanel(UILayer uiLayer)
    {
        if (_panelStack[uiLayer].Count == 0) return;
        var topPanel = GetTopPanel(uiLayer);
        Object.Destroy(topPanel.PanelObject);
        topPanel.OnPause();
        topPanel.OnClose();
        _panelStack[uiLayer].Pop();
        if (_panelStack[uiLayer].Count == 0) return;
        var nextTopPanel = GetTopPanel(uiLayer);
        var viewController = nextTopPanel.PanelObject.GetComponent<UIViewControllerBase>();
        if (viewController != null)
        {
            viewController.OnUpdateEvent += nextTopPanel.OnUpdate;
        }
        nextTopPanel.OnResume();
    }

    /// <summary>
    /// 关闭所有层级中的所有UI面板。
    /// </summary>
    public void CloseAll()
    {
        foreach (var layer in _panelStack.Keys)
        {
            CloseTopPanel(layer);
        }
    }

    /// <summary>
    /// 关闭并销毁场景层中的指定UI视图。
    /// </summary>
    /// <param name="uiView">要关闭的UI视图</param>
    public void CloseInSceneLayer(UIView uiView)
    {
        var view = uiView;
        Object.Destroy(view.PanelObject);
        if (_sceneLayerPanelList.Contains(view))
        {
            _sceneLayerPanelList.Remove(view);
        }
        view.OnPause();
        view.OnClose();
    }

    /// <summary>
    /// 重置UI系统，关闭所有面板并清理画布引用。
    /// </summary>
    public void Reset()
    {
        CloseAll();
        _activeCanvas = null;
    }

    /// <summary>
    /// 获取指定层级栈顶的UI面板。
    /// </summary>
    /// <param name="layer">要查询的UI层级</param>
    /// <returns>栈顶UI面板，如果栈为空则返回null并记录错误</returns>
    public UIView GetTopPanel(UILayer layer)
    {
        if (_panelStack[layer].Count > 0)
            return _panelStack[layer].Peek();
        Debug.LogError("UI栈中无任何面板");
        return null;
    }

    /// <summary>
    /// 获取或创建UI面板的画布。
    /// 如果场景中已存在名为"OverlayCanvas"的画布则使用现有画布，
    /// 否则创建一个新的ScreenSpaceOverlay模式的画布，并配置适当的缩放模式。
    /// 同时确保场景中存在EventSystem用于UI交互。
    /// </summary>
    /// <returns>UI面板使用的画布组件</returns>
    public Canvas GetOrAddPanelCanvas()
    {
        _activeCanvas = GameObject.Find("OverlayCanvas").GetComponent<Canvas>();
        if (_activeCanvas != null)
            return _activeCanvas;

        var panelCanvasGameObject = new GameObject("OverlayCanvas");

        var canvas = panelCanvasGameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var scaler = panelCanvasGameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        var graphicRaycaster = panelCanvasGameObject.AddComponent<GraphicRaycaster>();

        // 判断场景中是否存在EventSystem
        if (!Object.FindFirstObjectByType<EventSystem>())
        {
            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        var uIRoot = new GameObject("UIRoot");
        panelCanvasGameObject.transform.SetParent(uIRoot.transform);
        Object.FindFirstObjectByType<EventSystem>().transform.SetParent(uIRoot.transform);
        return canvas;
    }

    // public T GetOrAddComponent<T>() where T : Component
    // {
    //     return GetTopPanel().PanelObject.GetComponent<T>();
    // }
    //
    // public T GetOrAddComponentInChildren<T>(string childName) where T : Component
    // {
    //     return GetTopPanel().PanelObject.GetComponentsInChildren<T>().FirstOrDefault(child => child.name == childName);
    // }
}