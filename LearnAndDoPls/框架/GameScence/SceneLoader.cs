using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //todo-放置每个场景都有的东西
    public SceneLoadEventSO loadEventSO;
    public float fadeDuration = 1f;

    private GameSceneSO currentScene;
    private bool isLoading;

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
    }

    private void OnLoadRequestEvent(GameSceneSO sceneToLoad, Vector3 position, bool fade)
    {
        if (isLoading) return;

        StartCoroutine(LoadSceneRoutine(sceneToLoad, position, fade));
    }

    private IEnumerator LoadSceneRoutine(GameSceneSO sceneToLoad, Vector3 position, bool fade)
    {
        isLoading = true;

        if (fade)
        {
            FadeManager.Instance.FadeIn(fadeDuration);
            yield return new WaitForSeconds(fadeDuration);
        }

        // 加载依赖场景
        foreach (var dependency in sceneToLoad.dependentScenes)
        {
            if (dependency != null)
            {
                var dependencyHandle = dependency.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
                yield return dependencyHandle.Task;
            }
        }

        // 卸载当前场景
        if (currentScene != null)
        {
            yield return SceneManager.UnloadSceneAsync(currentScene.sceneReference.AssetGUID);
        }

        // 加载目标场景
        var handle = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        yield return handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            currentScene = sceneToLoad;
            player.transform.position = position;

            if (fade)
            {
                FadeManager.Instance.FadeOut(fadeDuration);
                yield return new WaitForSeconds(fadeDuration);
            }
        }
        else
        {
            Debug.LogError($"Failed to load scene: {sceneToLoad.sceneReference}");
        }

        isLoading = false;
    }
}