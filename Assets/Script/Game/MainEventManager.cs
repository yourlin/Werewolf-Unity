using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainEventManager : MonoBehaviour
{
    ScreenLoader screenLoader;
    public GameObject HistoryPanel;
    public GameObject HistoryContent;
    public GameObject MessagePrefab;
    public GameObject App;
    GameApp gameApp;
    public Button ManualBtn;
    public Button AutoBtn;


    // Start is called before the first frame update
    void Awake()
    {
        gameApp = App.GetComponent<GameApp> ();
        screenLoader = GetComponent<ScreenLoader> ();
    }

    public async void onQuit () {
        FileHelper.ClearFile (GameSetting.HistroyFilePath);
        await StopGame ();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit ();
        #endif

    }

    public async void onBackToMenu () {

        gameApp.IsRunning = false;
        FileHelper.ClearFile (GameSetting.HistroyFilePath);
        await StopGame ();
        
        StartCoroutine (screenLoader.LoadScene (0));
    }

    private async Task<string> StopGame () {

        using UnityWebRequest request = UnityWebRequest.Get(APIUrl.stopGame);
        _ = request.SendWebRequest();

        while (!request.isDone)
        {
            await Task.Yield(); // 等待请求完成
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            return request.downloadHandler.text;
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
            return null;
        }
    }

    public void onCloseHistory () {
        HistoryPanel.SetActive (false);
    }

    public void onHistory () {
        foreach (Transform child in HistoryContent.transform) {
            Destroy (child.gameObject);
        }

        string [] lines= FileHelper.ReadFromFile (GameSetting.HistroyFilePath);
        Debug.Log ($"显示历史记录{lines.Length}条");
        //List<PlayerMessage> messageList = new List<PlayerMessage> ();
        
        foreach (string line in lines) {
            // 尝试将每一行反序列化为 PlayerMessage 对象
            PlayerMessage message = JsonConvert.DeserializeObject<PlayerMessage> (line, new PlayerMessageConverter ());

            // 如果反序列化成功,则添加到列表中
            if (message != null) {

                // 实例化 SingleMessage Prefab
                GameObject singleMessageInstance = Instantiate (MessagePrefab, HistoryContent.transform);

                // 在这里,您可以设置 singleMessageInstance 的属性,例如显示消息内容等
                PlayerProfile profile = gameApp.dictPlayerObjects [message.PlayerId].GetComponent<PlayerCtrl> ().Profile;
                singleMessageInstance.GetComponent<TMP_Text> ().text = message.Message.content;
                SingleMessageCtrl singleMessageCtrl = singleMessageInstance.GetComponent<SingleMessageCtrl> ();
                singleMessageCtrl.Role.text = profile.Role.ToString();
                singleMessageCtrl.Name.text = message.PlayerName;
                singleMessageCtrl.Avatar.sprite = gameApp.GetPlayerImg (profile);
            }
        }

        HistoryPanel.SetActive(true);

    }

    public void onChangeSkip (bool value) {
        Debug.Log(value);
        gameApp.SetSkip (value);
        AutoBtn.gameObject.SetActive(!value);
        ManualBtn.gameObject.SetActive(value);
        //AutoBtn.enabled = !value;
        //ManualBtn.enabled = value;
    }
}
