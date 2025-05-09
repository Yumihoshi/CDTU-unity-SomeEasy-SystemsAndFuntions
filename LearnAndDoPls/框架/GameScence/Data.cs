using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public string sceneToSave;
    // public Dictionary<string, Vector3> characterPointDict = new Dictionary<string, Vector3>();
    // public Dictionary<string, float> floatSavedData = new Dictionary<string, float>();
    // public Dictionary<string, bool> boolSavedData = new Dictionary<string, bool>();

    public void SaveGameScene(GameSceneSO savedScene)
    {
        sceneToSave = JsonUtility.ToJson(savedScene);
    }

    public GameSceneSO GetSavedGameScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
        return newScene;
    }
}