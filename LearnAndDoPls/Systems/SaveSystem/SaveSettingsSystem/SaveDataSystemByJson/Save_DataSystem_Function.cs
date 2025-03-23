using UnityEngine; 
using System;

namespace SaveDataSystem
{
    public static class Save_DataSystem_Function
    {


        private static bool IsSerializable<T>()
        {
            Type type = typeof(T);//Type为System中的
            return type.IsSerializable ||
                   type.IsPrimitive ||
                   type == typeof(string) ||
                   type == typeof(decimal);
        }




        public static void SaveToJson<T>(string filePath, T data)
        {
            if (!IsSerializable<T>())
            {
                Debug.LogError($"类型 {typeof(T).Name} 不支持序列化，无法保存到JSON。");
                return;
            }

            string json = JsonUtility.ToJson(data, true);
            System.IO.File.WriteAllText(filePath, json);
            Debug.Log($"数据已保存到 {filePath}");
        }



    }






}