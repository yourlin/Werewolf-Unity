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


    public static int StartGameLang = 0;

    // Start is called before the first frame update
    void Awake () {
        screenLoader = GetComponent<ScreenLoader> ();
        GameSetting.Init ();
    }

    // Update is called once per frame
    void Update () {

    }

    public void onStartCN() {
        Debug.Log ("Start CN");
        StartGameLang = 0;
        StartCoroutine (EnterNextStage());

    }

    public void onStartEN()
    {
        Debug.Log("Start EN");
        StartGameLang = 1;
        StartCoroutine(EnterNextStage());
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
        string requestUrl = APIUrl.startGameCN;
        if (OpeningCtrl.StartGameLang == 1)
        {
            requestUrl = APIUrl.startGameEN;
        }
        UnityWebRequest request = UnityWebRequest.Get (requestUrl);
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
