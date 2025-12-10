using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using TMPro;

/// <summary>
/// UI 组件代码生成器
/// - 自动扫描子物体的 UI 组件
/// - 生成 View 层（组件绑定）和 Logic 层（业务逻辑）
/// - 支持 Button、Text、Image、InputField、Toggle、Slider 等
/// </summary>
public class UIComponentGenerator : EditorWindow
{
    private GameObject selectedObject;
    private string customNamespace = "Game.UI";
    private string viewScriptPath = "Assets/Scripts/UI/Views";
    private string logicScriptPath = "Assets/Scripts/UI/Logic";

    [MenuItem("Tools/UI Component Generator")]
    public static void ShowWindow()
    {
        GetWindow<UIComponentGenerator>("UI Component Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("UI 组件代码生成器（View + Logic）", EditorStyles.boldLabel);

        // 显示当前选中的对象
        GameObject currentSelection = Selection.activeGameObject;
        if (currentSelection != null && currentSelection.GetComponent<Canvas>() != null)
        {
            EditorGUILayout.HelpBox($"当前选中: {currentSelection.name}", MessageType.Info);
            if (GUILayout.Button("使用选中的对象", GUILayout.Height(30)))
            {
                selectedObject = currentSelection;
            }
        }

        selectedObject = (GameObject)EditorGUILayout.ObjectField("目标 UI 对象", selectedObject, typeof(GameObject), true);

        customNamespace = EditorGUILayout.TextField("命名空间", customNamespace);

        GUILayout.Space(10);
        GUILayout.Label("文件生成路径", EditorStyles.boldLabel);
        viewScriptPath = EditorGUILayout.TextField("View 层路径", viewScriptPath);
        logicScriptPath = EditorGUILayout.TextField("Logic 层路径", logicScriptPath);

        GUILayout.Space(10);

        // 快捷生成按钮 - 直接使用当前选中的对象
        EditorGUI.BeginDisabledGroup(currentSelection == null || currentSelection.GetComponent<Canvas>() == null);
        if (GUILayout.Button($"为选中对象生成脚本 ({(currentSelection != null ? currentSelection.name : "无选中")})", GUILayout.Height(40)))
        {
            if (currentSelection != null)
            {
                selectedObject = currentSelection;
                GenerateUIScripts(currentSelection);
            }
        }
        EditorGUI.EndDisabledGroup();
    }

    private void GenerateUIScripts(GameObject uiObject)
    {
        // 扫描 UI 组件
        var components = ScanUIComponents(uiObject);

        if (components.Count == 0)
        {
            EditorUtility.DisplayDialog("提示", "未找到任何 UI 组件", "OK");
            return;
        }

        string viewCode = GenerateViewCode(uiObject, components);
        string logicCode = GenerateLogicCode(uiObject, components);

        // 创建目录
        Directory.CreateDirectory(viewScriptPath);
        Directory.CreateDirectory(logicScriptPath);

        // 保存到指定路径
        string viewPath = Path.Combine(viewScriptPath, $"{uiObject.name}View.cs");
        string logicPath = Path.Combine(logicScriptPath, $"{uiObject.name}Logic.cs");

        File.WriteAllText(viewPath, viewCode, Encoding.UTF8);
        File.WriteAllText(logicPath, logicCode, Encoding.UTF8);

        AssetDatabase.Refresh();

        // 自动添加脚本到 GameObject 并绑定组件（支持等待编译完成）
        AutoAddScriptsAndBindComponents(uiObject, components, viewPath);

        EditorUtility.DisplayDialog("成功", $"已生成脚本并自动添加到对象：\n\nView: {viewPath}\n\nLogic: {logicPath}", "OK");
    }

    /// <summary>
    /// 自动添加脚本到 GameObject 并绑定组件
    /// </summary>
    private void AutoAddScriptsAndBindComponents(GameObject uiObject, Dictionary<string, (string path, string type)> components, string viewPath)
    {
        // 获取或添加 View 脚本
        string scriptName = $"{uiObject.name}View";
        var viewScriptType = GetTypeFromAssembly(scriptName);

        // 如果脚本类型存在且未添加，先添加它
        MonoBehaviour scriptInstance = uiObject.GetComponent(scriptName) as MonoBehaviour;
        if (scriptInstance == null && viewScriptType != null)
        {
            scriptInstance = uiObject.AddComponent(viewScriptType) as MonoBehaviour;
        }

        // 如果 View 脚本实例存在，就进行绑定操作
        if (scriptInstance != null)
        {
            BindComponentsToGameObject(uiObject, components); // 绑定 UI 组件

            // 在添加脚本后，确保 Unity 编辑器能够正确刷新脚本字段引用
            EditorUtility.SetDirty(scriptInstance);  // 标记脚本为已修改，通知 Unity 进行更新
            AssetDatabase.SaveAssets();  // 保存修改的资源

            // 强制刷新和重新编译
            AssetDatabase.Refresh();

            // 延迟脚本的编译和引用更新，确保脚本完全加载
            EditorApplication.delayCall += () =>
            {
                if (uiObject != null)
                {
                    // 确保脚本绑定完成后执行，避免引用未更新
                    BindComponentsToGameObject(uiObject, components);
                }
            };
        }
        else
        {
            Debug.LogError($"[UIComponentGenerator] 无法添加或获取脚本 {scriptName}");
        }
    }

    /// <summary>
    /// 从程序集获取类型
    /// </summary>
    private System.Type GetTypeFromAssembly(string typeName)
    {
        foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = assembly.GetType(customNamespace + "." + typeName);
            if (type != null) return type;

            type = assembly.GetType(typeName);
            if (type != null) return type;
        }
        return null;
    }

    /// <summary>
    /// 绑定组件到 GameObject 的脚本字段
    /// </summary>
    private void BindComponentsToGameObject(GameObject uiObject, Dictionary<string, (string path, string type)> components)
    {
        string scriptName = $"{uiObject.name}View";
        var component = uiObject.GetComponent(scriptName);
        if (component == null) return;
        var serializedObject = new SerializedObject(component);

        foreach (var kvp in components)
        {
            string fieldName = kvp.Key;
            string path = kvp.Value.path;
            string type = kvp.Value.type;
            Transform targetTransform = uiObject.transform.Find(path);
            if (targetTransform != null)
            {
                Component targetComponent = GetComponentOfType(targetTransform, type);
                if (targetComponent != null)
                {
                    var property = serializedObject.FindProperty(fieldName);
                    if (property != null && property.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        property.objectReferenceValue = targetComponent;
                    }
                }
            }
        }
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(component);
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// 扫描 UI 对象的所有子物体，找出 UI 组件
    /// </summary>
    private Dictionary<string, (string path, string type)> ScanUIComponents(GameObject uiObject)
    {
        var components = new Dictionary<string, (string, string)>();

        foreach (Transform child in uiObject.GetComponentsInChildren<Transform>(true))
        {
            if (child == uiObject.transform) continue; // 跳过自身

            // 获取所有 UI 组件类型
            var componentTypes = GetAllUIComponentTypes(child);
            string path = GetRelativePath(uiObject.transform, child);

            foreach (var componentType in componentTypes)
            {
                // 用 GameObject 的实际名字 + 组件类型作为变量名
                string baseName = MakeSafeVariableName(child.gameObject.name);
                string varName = componentTypes.Count > 1
                    ? $"{baseName}{componentType}" // 多个组件时加后缀，如 titleText, titleImage
                    : baseName; // 单个组件直接用名字

                // 避免重名
                int suffix = 1;
                string finalName = varName;
                while (components.ContainsKey(finalName))
                {
                    finalName = $"{varName}{suffix++}";
                }

                components[finalName] = (path, componentType);
            }
        }

        return components;
    }

    private string GetRelativePath(Transform root, Transform target)
    {
        var path = target.name;
        var current = target.parent;
        while (current != null && current != root)
        {
            path = $"{current.name}/{path}";
            current = current.parent;
        }
        return path;
    }

    /// <summary>
    /// 获取物体上所有 UI 组件类型
    /// </summary>
    private List<string> GetAllUIComponentTypes(Transform child)
    {
        var types = new List<string>();

        if (child.GetComponent<Button>() != null) types.Add("Button");
        if (child.GetComponent<Text>() != null) types.Add("Text");
        if (child.GetComponent<Image>() != null) types.Add("Image");
        if (child.GetComponent<InputField>() != null) types.Add("InputField");
        if (child.GetComponent<Toggle>() != null) types.Add("Toggle");
        if (child.GetComponent<Slider>() != null) types.Add("Slider");
        if (child.GetComponent<ScrollRect>() != null) types.Add("ScrollRect");
        if (child.GetComponent<Dropdown>() != null) types.Add("Dropdown");
        if (child.GetComponent<TMP_Text>() != null) types.Add("TMP_Text");
        if (child.GetComponent<TMP_InputField>() != null) types.Add("TMP_InputField");

        return types;
    }

    /// <summary>
    /// 获取指定类型的组件实例
    /// </summary>
    private Component GetComponentOfType(Transform transform, string componentTypeName)
    {
        switch (componentTypeName)
        {
            case "Button": return transform.GetComponent<Button>();
            case "Text": return transform.GetComponent<Text>();
            case "Image": return transform.GetComponent<Image>();
            case "InputField": return transform.GetComponent<InputField>();
            case "Toggle": return transform.GetComponent<Toggle>();
            case "Slider": return transform.GetComponent<Slider>();
            case "ScrollRect": return transform.GetComponent<ScrollRect>();
            case "Dropdown": return transform.GetComponent<Dropdown>();
            case "TMP_Text": return transform.GetComponent<TMP_Text>();
            case "TMP_InputField": return transform.GetComponent<TMP_InputField>();
            default: return null;
        }
    }

    /// <summary>
    /// 生成安全的变量名
    /// </summary>
    private string MakeSafeVariableName(string name)
    {
        // 去除特殊字符，首字母小写
        var safeName = System.Text.RegularExpressions.Regex.Replace(name, "[^a-zA-Z0-9]", "");
        if (safeName.Length == 0) safeName = "element";
        return char.ToLower(safeName[0]) + safeName.Substring(1);
    }

    /// <summary>
    /// 生成 View 层代码
    /// </summary>
    private string GenerateViewCode(GameObject uiObject, Dictionary<string, (string path, string type)> components)
    {
        StringBuilder sb = new StringBuilder();

        // 头部
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine("using UnityEngine.UI;");
        sb.AppendLine("using TMPro;");
        sb.AppendLine("using System;");
        sb.AppendLine("");
        if (!string.IsNullOrEmpty(customNamespace))
        {
            sb.AppendLine($"namespace {customNamespace}");
            sb.AppendLine("{");
        }

        // 类定义
        sb.AppendLine($"    /// <summary>");
        sb.AppendLine($"    /// {uiObject.name} View 层 - UI 组件绑定");
        sb.AppendLine($"    /// </summary>");
        sb.AppendLine($"    public partial class {uiObject.name}View : MonoBehaviour");
        sb.AppendLine($"    {{");

        // 声明组件字段
        sb.AppendLine($"        // UI Components");
        foreach (var kvp in components)
        {
            sb.AppendLine($"        [SerializeField] private {kvp.Value.type} {kvp.Key};");
        }

        // 初始化方法
        sb.AppendLine("");
        sb.AppendLine($"        private void Start()");
        sb.AppendLine($"        {{");
        sb.AppendLine($"            BindComponents();");
        sb.AppendLine($"        }}");

        // 绑定组件方法
        sb.AppendLine("");
        sb.AppendLine($"        private void BindComponents()");
        sb.AppendLine($"        {{");
        foreach (var kvp in components)
        {
            sb.AppendLine($"            if ({kvp.Key} == null)");
            sb.AppendLine($"            {{");
            sb.AppendLine($"                Debug.LogWarning($\"[{uiObject.name}View] {kvp.Key} ({kvp.Value.type}) 未绑定\");");
            sb.AppendLine($"            }}");
        }
        sb.AppendLine($"        }}");

        // 文本更新方法（如果有 Text/TMP_Text 组件）
        var texts = components.Where(c => c.Value.type == "Text" || c.Value.type == "TMP_Text").ToList();
        if (texts.Count > 0)
        {
            sb.AppendLine("");
            sb.AppendLine($"        /// <summary>更新文本内容</summary>");
            foreach (var txt in texts)
            {
                sb.AppendLine($"        public void Set{CapitalizeFirst(txt.Key)}(string content)");
                sb.AppendLine($"        {{");
                sb.AppendLine($"            if ({txt.Key} != null) {txt.Key}.text = content;");
                sb.AppendLine($"        }}");
            }
        }

        // 关闭方法
        sb.AppendLine("");
        sb.AppendLine($"        public void Close()");
        sb.AppendLine($"        {{");
        sb.AppendLine($"            gameObject.SetActive(false);");
        sb.AppendLine($"        }}");

        sb.AppendLine($"    }}");

        if (!string.IsNullOrEmpty(customNamespace))
        {
            sb.AppendLine("}");
        }

        return sb.ToString();
    }

    /// <summary>
    /// 生成 Logic 层代码
    /// </summary>
    private string GenerateLogicCode(GameObject uiObject, Dictionary<string, (string path, string type)> components)
    {
        StringBuilder sb = new StringBuilder();

        // 头部
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine("");
        if (!string.IsNullOrEmpty(customNamespace))
        {
            sb.AppendLine($"namespace {customNamespace}");
            sb.AppendLine("{");
        }

        // 类定义（partial）
        sb.AppendLine($"    /// <summary>");
        sb.AppendLine($"    /// {uiObject.name} Logic 层 - 业务逻辑处理");
        sb.AppendLine($"    /// </summary>");
        sb.AppendLine($"    public partial class {uiObject.name}View : MonoBehaviour");
        sb.AppendLine($"    {{");

        // TODO: 在此添加业务逻辑处理方法
        sb.AppendLine($"        public void Initialize()");
        sb.AppendLine($"        {{");
        sb.AppendLine($"            // 初始化逻辑");
        sb.AppendLine($"        }}");

        sb.AppendLine($"    }}");

        if (!string.IsNullOrEmpty(customNamespace))
        {
            sb.AppendLine("}");
        }

        return sb.ToString();
    }


    /// <summary>
    /// 首字母大写
    /// </summary>
    private string CapitalizeFirst(string str)
    {
        if (str.Length == 0) return str;
        return char.ToUpper(str[0]) + str.Substring(1);
    }
}

