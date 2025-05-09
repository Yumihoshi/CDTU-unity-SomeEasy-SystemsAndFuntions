using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game Scene/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    public SceneType SceneType; // 场景类型
    public AssetReference sceneReference; // Addressables 场景引用
    public List<GameSceneSO> dependentScenes; // 依赖场景
    public int priority; // 加载优先级
}