using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningEvent : MonoBehaviour
{
    ScreenLoader screenLoader;
    // Start is called before the first frame update
    void Awake()
    {
        screenLoader = GetComponent<ScreenLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onStart()
    {
        Debug.Log("Start");
        StartCoroutine(screenLoader.LoadScene(1));
    }

    public void onSetting()
    {
        Debug.Log("Show Setting");
    }

    public void onQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//�����˳�����
        #else
            Application.Quit();
        #endif
    }

}
