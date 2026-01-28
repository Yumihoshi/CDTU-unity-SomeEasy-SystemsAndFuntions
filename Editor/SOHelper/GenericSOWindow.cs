using UnityEngine;
using UnityEditor;

public class GenericSOWindow : EditorWindow
{
    private ScriptableObject targetSO;
    private Editor cachedEditor;
    private Vector2 scrollPos;

    public static void Open(ScriptableObject so)
    {
        GenericSOWindow window = GetWindow<GenericSOWindow>("快速编辑");
        window.targetSO = so;
        window.Show();
    }

    private void OnGUI()
    {
        if (targetSO == null) { Close(); return; }

        GUILayout.Label($"编辑资源: {targetSO.name}", EditorStyles.boldLabel);
        
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        
        // 创建并绘制该 SO 的 Inspector 界面
        if (cachedEditor == null || cachedEditor.target != targetSO)
        {
            Editor.CreateCachedEditor(targetSO, null, ref cachedEditor);
        }
        
        cachedEditor.OnInspectorGUI();
        
        EditorGUILayout.EndScrollView();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(targetSO);
        }
    }
}