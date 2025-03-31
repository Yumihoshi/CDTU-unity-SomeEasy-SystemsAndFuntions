using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using CDTU.Utils;

namespace Managers
{
    public class MySceneManager : Singleton<MySceneManager>
    {
        #region 事件

        /// <summary>
        /// 场景事件参数类
        /// </summary>
        public class SceneEventArgs : EventArgs
        {
            /// <summary>场景名称</summary>
            public string SceneName { get; set; }
            /// <summary>加载进度(0-1)</summary>
            public float Progress { get; set; }
            /// <summary>加载耗时(秒)</summary>
            public float LoadTime { get; set; }
        }

        // 暂时注释掉进度相关事件
        //public event EventHandler<SceneEventArgs> OnLoadingProgressChanged;
        //public event EventHandler<SceneEventArgs> OnLoadingStarted;
        //public event EventHandler<SceneEventArgs> OnLoadingCompleted;

        #endregion

        /// <summary>是否正在加载场景</summary>
        public bool IsLoading { get; private set; }

        #region 生命周期函数

        protected override void Awake()
        {
            base.Awake();
            Debug.Log("场景管理器初始化完成");
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        #endregion

        #region 场景加载方法

        /// <summary>
        /// 直接场景加载方法
        /// </summary>
        /// <param name="sceneName">要加载的场景名称</param>
        public void LoadScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName) || IsLoading)
            {
                Debug.LogError("场景名称无效或正在加载中");
                return;
            }

            try
            {
                SceneManager.LoadScene(sceneName);
                //OnLoadingStarted?.Invoke(this, new SceneEventArgs { SceneName = sceneName });
            }
            catch (Exception e)
            {
                HandleSceneLoadError(sceneName, e);
            }
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName">要加载的场景名称</param>
        public void LoadSceneAsync(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName) || IsLoading)
            {
                Debug.LogError("场景名称无效或正在加载中");
                return;
            }

            try
            {
                StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
            }
            catch (Exception e)
            {
                HandleSceneLoadError(sceneName, e);
            }
        }

        /// <summary>
        /// 异步加载场景的协程实现
        /// </summary>
        private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
        {
            IsLoading = true;
            //float startTime = Time.time;

            //OnLoadingStarted?.Invoke(this, new SceneEventArgs { SceneName = sceneName });

            AsyncOperation asyncOperation = null;
            try
            {
                asyncOperation = SceneManager.LoadSceneAsync(sceneName);
                if (asyncOperation == null) throw new Exception("创建异步加载操作失败");
            }
            catch (Exception e)
            {
                HandleSceneLoadError(sceneName, e);
                yield break;
            }

            while (!asyncOperation.isDone)
            {
                //float progress = Mathf.Clamp01(asyncOperation.progress);
                //OnLoadingProgressChanged?.Invoke(this, new SceneEventArgs
                //{
                //    SceneName = sceneName,
                //    Progress = progress
                //});
                yield return null;
            }

            //float loadTime = Time.time - startTime;
            //OnLoadingCompleted?.Invoke(this, new SceneEventArgs
            //{
            //    SceneName = sceneName,
            //    LoadTime = loadTime
            //});

            IsLoading = false;
        }

        /// <summary>
        /// 处理场景加载错误
        /// </summary>
        private void HandleSceneLoadError(string sceneName, Exception error)
        {
            Debug.LogError($"场景加载错误: {sceneName}, {error.Message}");
            IsLoading = false;
        }

        #endregion

        #region 场景事件处理

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"场景 {scene.name} 加载完成");
        }

        private void OnSceneUnloaded(Scene scene)
        {
            Debug.Log($"场景 {scene.name} 已卸载");
        }

        #endregion
    }
}
