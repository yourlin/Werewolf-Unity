using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine;

public class GameSetting : Singleton<GameSetting>
{
    [SerializeField]
    static int playerNum = 10;
    [SerializeField]
    static int playSpeed = 1;
    [SerializeField]
    static float gameLoopInterval = 5000f;
	[SerializeField]
    static int requestTimeout = 1;
    [SerializeField]
    static string histroyFilePath = "./history.txt";

    public static int PlaySpeed { get => playSpeed; set => playSpeed = value; }
    public static int PlayerNum { get => playerNum; set => playerNum = value; }
	public static float GameLoopInterval { get => gameLoopInterval; set => gameLoopInterval = value; }
	public static int RequestTimeout { get => requestTimeout; set => requestTimeout = value; }
    public static float MessageEndWaitforSeconds { get; internal set; } = 5f;
    public static string HistroyFilePath { get => histroyFilePath; set => histroyFilePath = value; }
    public static string APIUrl = "";

    static bool isInited = false;

    public static void Init () {
        if(isInited) {
            return;
        }

        LoadConfigFromJSON ();
        Debug.Log ("加载配置");
    }

    public static void LoadConfigFromJSON () {
#if UNITY_EDITOR
        string jsonPath = Path.Combine (Application.streamingAssetsPath, "config.json");
#else
        string jsonPath = Path.Combine ("./", "config.json");
#endif
        if (File.Exists (jsonPath)) {
            string jsonData = File.ReadAllText (jsonPath);
            var settings = JsonConvert.DeserializeObject<JObject> (jsonData);

            playerNum = (int)settings["playerNum"];
            playSpeed = (int)settings["playSpeed"];
            gameLoopInterval = (float)settings["gameLoopInterval"];
            requestTimeout = (int)settings["requestTimeout"];
            histroyFilePath = (string)settings["histroyFilePath"];
            APIUrl = (string)settings ["APIUrl"];

            Debug.Log ("配置加载完成" + APIUrl);
        } else {
            Debug.LogError ("Config file not found: " + jsonPath);
        }
    }
}
