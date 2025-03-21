using UnityEngine;

namespace SaveSystem
{

    /// <summary>
    /// 一个保存和加载设置工具类
    /// </summary>
    public static class Save_Load_SettingsSystem_Functions
    {
        /// <summary>
        /// 通过PlayerPrefs保存设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <typeparam name="T">什么类型都行，但是推荐就string</typeparam>
        public static void SaveByPlayerPrefs<T>(string key, T data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
#if UNITY_EDITOR
            Debug.Log($"Save Successfully: {key}");
#endif
        }
        /// <summary>
        /// 通过PlayerPrefs在注册表的东西读取
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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