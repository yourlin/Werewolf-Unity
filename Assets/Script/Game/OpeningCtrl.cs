using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpeningCtrl : MonoBehaviour {
    ScreenLoader screenLoader;
    public Button StartButton;
    //public Texture2D NormalCursorTexture;
    public Texture2D busyCursorTexture;
    public TMP_Text ErrorMessage;
    // Start is called before the first frame update
    void Awake () {
        screenLoader = GetComponent<ScreenLoader> ();
        GameSetting.Init ();
    }

    // Update is called once per frame
    void Update () {

    }

    public void onStart () {
        Debug.Log ("Start");
        StartCoroutine (StartGame());

    }

    public void onSetting () {
        Debug.Log ("Show Setting");
    }

    public void onQuit () {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

    IEnumerator EnterNextStage()
    {
        yield return screenLoader.LoadScene(1);
    }

    IEnumerator StartGame () {
        ErrorMessage.text = "";
        StartButton.interactable = false;
        Cursor.SetCursor (busyCursorTexture, Vector2.zero, CursorMode.Auto);
        UnityWebRequest request = UnityWebRequest.Get (APIUrl.startGame);
        request.timeout = GameSetting.RequestTimeout;
        yield return request.SendWebRequest ();
        
        if (request.result == UnityWebRequest.Result.Success) {
            Debug.Log ("开始游戏");
            yield return screenLoader.LoadScene(1);
        } else {
            // 请求失败,输出错误信息
            Debug.LogError ("Error: " + request.error);
            ErrorMessage.text = request.error;
            StartButton.interactable = true;
            
        }
        Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
    }
}
