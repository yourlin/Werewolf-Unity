using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public CanvasGroup UI;

    private void Awake()
    {
        UI = FindFirstObjectByType<CanvasGroup>();
    }

    private void OnEnable()
    {
        SceneSwitchEventHandler.SwitchScenes += OnSwitchScenes;
    }

    private void OnDisable()
    {
        SceneSwitchEventHandler.SwitchScenes += OnSwitchScenes;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetScene">Ŀ�곡��</param>
    private void OnSwitchScenes(string targetScene)
    {
        StartCoroutine(SwitchScenes(targetScene));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetScene">Ŀ�곡��</param>
    private IEnumerator SwitchScenes(string targetScene)
    {
        UI.GetComponentInParent<Canvas>().sortingOrder = 1;

        yield return Fade(1);

        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        yield return LoadSceneSetActive(targetScene);

        yield return Fade(0);

        UI.GetComponentInParent<Canvas>().sortingOrder = -1;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadSceneSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetAlpha"></param>
    /// <returns></returns>

    private IEnumerator Fade(float targetAlpha)
    {
        UI.blocksRaycasts = true;
        float speed = Mathf.Abs(UI.alpha - targetAlpha) / 1.5f;
        while (!Mathf.Approximately(UI.alpha, targetAlpha))
        {
            UI.alpha = Mathf.MoveTowards(UI.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }

        UI.blocksRaycasts = false;
    }

}