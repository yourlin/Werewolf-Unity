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
    /// 订阅(场景切换)
    /// </summary>
    /// <param name="targetScene">目标场景</param>
    private void OnSwitchScenes(string targetScene)
    {
        StartCoroutine(SwitchScenes(targetScene));
    }


    /// <summary>
    /// 切换场景方法
    /// </summary>
    /// <param name="targetScene">目标场景</param>
    private IEnumerator SwitchScenes(string targetScene)
    {
        //将加载显示UI界面优先级调高于其他场景的UI界面
        UI.GetComponentInParent<Canvas>().sortingOrder = 1;

        //调用方法 将加载显示UI界面的透明度逐渐调高至完全不透明
        yield return Fade(1);

        //关闭当前激活的场景 (异步)
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        //加载需要的场景并设置为激活
        yield return LoadSceneSetActive(targetScene);

        //调用方法 将加载显示UI界面的透明度逐渐调低至完全透明
        yield return Fade(0);

        //恢复加载显示UI界面的优先级
        UI.GetComponentInParent<Canvas>().sortingOrder = -1;
    }



    /// <summary>
    /// 加载场景并设置为激活
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <returns></returns>
    private IEnumerator LoadSceneSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
    }

    /// <summary>
    /// 逐渐修改UI界面的透明度
    /// </summary>
    /// <param name="targetAlpha">修改值</param>
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