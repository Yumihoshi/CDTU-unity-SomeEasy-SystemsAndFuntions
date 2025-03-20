using System.IO;
using UnityEditor;
using UnityEngine;

namespace CDTU.UI.Editor
{
    [CustomEditor(typeof(UIViewControllerBase))]
    public class UIViewEditor : UnityEditor.Editor
    {
        private bool _isGenerator = false;

        public override void OnInspectorGUI()
        {
            UIViewControllerBase generator = (UIViewControllerBase)target;

            // 判断文件是否存在
            _isGenerator = File.Exists(UIConfig.UIScriptPath + generator.gameObject.name + ".cs");

            if (!_isGenerator && GUILayout.Button("生成UIView代码"))
            {
                GenerateUIViewCode(generator.gameObject.name);
            }

            if (_isGenerator)
            {
                GUILayout.Label($"已生成与面板名同名的代码文件: {generator.gameObject.name}.cs");
                if (GUILayout.Button("打开"))
                {
                    OpenGeneratedScript(generator.gameObject.name);
                }
            }

            DrawDefaultInspector();
        }

        private void GenerateUIViewCode(string className)
        {
            // 确保目录存在
            string directoryPath = Path.GetDirectoryName(UIConfig.UIScriptPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // 生成代码内容
            string codeContent = $@"using UnityEngine;
using CDTU.UI;

namespace CDTU.UI.Views
{{
    /// <summary>
    /// {className} 的视图逻辑
    /// </summary>
    public class {className} : UIView
    {{
        #region UI组件引用
        // 在此处添加UI组件引用
        #endregion

        #region 私有字段
        private bool _isInitialized;
        #endregion

        #region 生命周期方法
        public override void OnOpen()
        {{
            if (!_isInitialized)
            {{
                BindUI();
                _isInitialized = true;
            }}
            
            base.OnOpen();
            // 在此处添加面板打开时的逻辑
        }}

        public override void OnResume()
        {{
            base.OnResume();
            // 在此处添加面板恢复时的逻辑
        }}

        public override void OnPause()
        {{
            base.OnPause();
            // 在此处添加面板暂停时的逻辑
        }}

        public override void OnClose()
        {{
            base.OnClose();
            // 在此处添加面板关闭时的清理逻辑
        }}

        public override void OnUpdate()
        {{
            base.OnUpdate();
            // 在此处添加需要每帧更新的逻辑
        }}
        #endregion

        #region 私有方法
        private void BindUI()
        {{
            // 在此处添加组件绑定代码
        }}
        #endregion

        #region 事件处理
        // 在此处添加UI事件处理方法
        #endregion
    }}
}}";
            try
            {
                // 写入代码文件
                File.WriteAllText(UIConfig.UIScriptPath + className + ".cs", codeContent);
                // 刷新项目资源
                AssetDatabase.Refresh();
                Debug.Log($"成功生成UI视图代码: {className}.cs");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"生成UI视图代码失败: {e.Message}");
            }
        }

        private void OpenGeneratedScript(string className)
        {
            string path = UIConfig.UIScriptPath + className + ".cs";
            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            if (obj != null)
            {
                AssetDatabase.OpenAsset(obj);
            }
            else
            {
                Debug.LogError($"找不到代码文件: {path}");
            }
        }
    }
}