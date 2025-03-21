using UnityEngine;

namespace SaveSystem
{
    public static class Save_SettingsSystem_Functions
    {
        public static void SaveByPlayerPrefs<T>(string key, T data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
#if UNITY_EDITOR
            Debug.Log($"Save Successfully: {key}");
#endif
        }

        public static T LoadByPlayerPrefs<T>(string key) where T : new()
        {
            string json = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(json))
            {
                return new T();
            }
            return JsonUtility.FromJson<T>(json);
        }
    }
}