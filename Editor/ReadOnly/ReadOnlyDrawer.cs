using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 保存之前的 GUI 启用状态
        bool previousGUIState = GUI.enabled;

        // 关闭 GUI 交互
        GUI.enabled = false;

        // 绘制属性字段
        EditorGUI.PropertyField(position, property, label);

        // 恢复 GUI 启用状态，确保后面的字段不会被影响
        GUI.enabled = previousGUIState;
    }
}