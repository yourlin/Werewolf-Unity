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
    /// ����(�����л�)
    /// </summary>
    /// <param name="targetScene">Ŀ�곡��</param>
    private void OnSwitchScenes(string targetScene)
    {
        StartCoroutine(SwitchScenes(targetScene));
    }


    /// <summary>
    /// �л���������
    /// </summary>
    /// <param name="targetScene">Ŀ�곡��</param>
    private IEnumerator SwitchScenes(string targetScene)
    {
        //��������ʾUI�������ȼ�����������������UI����
        UI.GetComponentInParent<Canvas>().sortingOrder = 1;

        //���÷��� ��������ʾUI�����͸�����𽥵�������ȫ��͸��
        yield return Fade(1);

        //�رյ�ǰ����ĳ��� (�첽)
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        //������Ҫ�ĳ���������Ϊ����
        yield return LoadSceneSetActive(targetScene);

        //���÷��� ��������ʾUI�����͸�����𽥵�������ȫ͸��
        yield return Fade(0);

        //�ָ�������ʾUI��������ȼ�
        UI.GetComponentInParent<Canvas>().sortingOrder = -1;
    }



    /// <summary>
    /// ���س���������Ϊ����
    /// </summary>
    /// <param name="sceneName">������</param>
    /// <returns></returns>
    private IEnumerator LoadSceneSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
    }

    /// <summary>
    /// ���޸�UI�����͸����
    /// </summary>
    /// <param name="targetAlpha">�޸�ֵ</param>
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