using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ScriptableObject), true)] // 这里的 true 表示对所有 SO 子类生效
public class SOPopupDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 计算按钮的宽度
        float buttonWidth = 25f;
        Rect fieldRect = new Rect(position.x, position.y, position.width - buttonWidth - 5, position.height);
        Rect buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, position.height);

        // 1. 绘制标准的引用框
        EditorGUI.PropertyField(fieldRect, property, label);

        // 2. 绘制“弹出窗口”按钮
        if (property.objectReferenceValue != null)
        {
            if (GUI.Button(buttonRect, "🔍"))
            {
                GenericSOWindow.Open((ScriptableObject)property.objectReferenceValue);
            }
        }
    }
}