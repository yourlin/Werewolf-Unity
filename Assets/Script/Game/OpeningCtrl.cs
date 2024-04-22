using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class OpeningCtrl : MonoBehaviour {
    ScreenLoader screenLoader;
    // Start is called before the first frame update
    void Awake () {
        screenLoader = GetComponent<ScreenLoader> ();
    }

    // Update is called once per frame
    void Update () {

    }

    public void onStart () {
        Debug.Log ("Start");
        StartCoroutine (StartGame ());

    }

    public void onSetting () {
        Debug.Log ("Show Setting");
    }

    public void onQuit () {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�����˳�����
#else
        Application.Quit ();
#endif
    }


    IEnumerator StartGame () {
        UnityWebRequest request = UnityWebRequest.Get (APIUrl.startGame);
        request.timeout = GameSetting.RequestTimeout;
        yield return request.SendWebRequest ();
        if (request.result == UnityWebRequest.Result.Success) {
            Debug.Log ("开始游戏");

            yield return new WaitForSeconds(2);
            yield return screenLoader.LoadScene (1);
        } else {
            // 请求失败,输出错误信息
            Debug.LogError ("Error: " + request.error);
        }
    }
}
