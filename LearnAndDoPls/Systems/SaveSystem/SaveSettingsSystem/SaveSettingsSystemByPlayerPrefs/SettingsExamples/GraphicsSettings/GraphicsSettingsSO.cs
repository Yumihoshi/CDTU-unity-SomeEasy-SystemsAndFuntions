using UnityEngine;
using SaveSettingsSystem;



[CreateAssetMenu(fileName = "GraphicsSettingsSO", menuName = "Settings/Graphics SettingsSO")]
public class GraphicsSettingsSO : ScriptableObject
{
    [Header("图形设置数据")]
    [SerializeField, Tooltip("全屏模式")]
    public bool fullscreenMode = true;

    [SerializeField, Tooltip("分辨率索引")]
    public int resolutionIndex = 0;

    [SerializeField, Range(0, 2), Tooltip("画质等级")]
    public int qualityLevel = 1;

    [SerializeField, Range(30, 144), Tooltip("帧率上限")]
    public int targetFrameRate = 60;
}
