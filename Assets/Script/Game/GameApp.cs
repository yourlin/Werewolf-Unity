using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class GameApp : UnitySingleton<GameApp> {
    Queue<PlayerMessage> gameMessages = new Queue<PlayerMessage> ();
    bool isRunning = false;

    public TMP_Text roundText;
    public TMP_Text playerName;
    public TMP_Text playerMessage;
    public UnityEngine.UI.Image playerImg;
    public List<Sprite> PlayerImageList;
    private bool isMsgHandling = false;
    public TMP_Text DayText;
    public GameObject Roundboard;
    private int currentRound = 0;
    public GameObject Conclusion;

    public bool IsRunning { get => isRunning; set => isRunning = value; }
    public Queue<PlayerMessage> GameMessages { get => gameMessages; set => gameMessages = value; }
    public Dictionary<int, GameObject> dictPlayerObjects = new Dictionary<int, GameObject> ();

    private List<string> clipList = new List<string> ();
    private int currentPlayerNum = 0;
    private int femaleCount = 0;
    private readonly int femaleCountMax = 6;
    private int maleCount = 0;
    private readonly int maleCountMax = 6;
    static readonly string [] MaleNames = { "John", "David", "Michael", "Robert", "William", "James", "Richard", "Joseph", "Thomas", "Charles" };
    static readonly string [] FemaleNames = { "Emily", "Emma", "Olivia", "Sophia", "Ava", "Isabella", "Mia", "Abigail", "Avery", "Lily" };
    public GameObject playerTemplate;
    private string histroyFilePath = "./history.txt";

    /// <summary>
    /// 初始化
    /// </summary>
    public void InitGame () {
        clipList.Add ("Busy");
        clipList.Add ("Death");
        clipList.Add ("Dying");
        clipList.Add ("Idle_Down");
        clipList.Add ("Idle_Left");
        clipList.Add ("Idle_Right");
        clipList.Add ("Idle_Up");

        GetOnlinePlayers ();

        // get start
        StartCoroutine (run ());
    }

    async void GetOnlinePlayers () {
        using UnityWebRequest request = UnityWebRequest.Get (APIUrl.getPlayer);
        request.timeout = GameSetting.RequestTimeout;
        await request.SendWebRequest ();

        if (request.result == UnityWebRequest.Result.Success) {
            // 请求成功,处理响应数据
            // parse response message
            var respawn = GameObject.FindGameObjectWithTag ("Respawn");
            PlayerProfile [] playerProfiles =
                JsonConvert.DeserializeObject<PlayerProfile []> (
                request.downloadHandler.text,
                new PlayerProfileConverter ()
                );
            Debug.Log ($"{playerProfiles.Length} players will p");
            foreach (var playerProfile in playerProfiles) {
                SpawnPlayer (playerTemplate,
                    playerProfile,
                    respawn.transform.GetChild (currentPlayerNum)
                    );
            }
        } else {
            // 请求失败,输出错误信息
            Debug.LogError ("Error: " + request.error);
        }
    }

    public GameObject SpawnPlayer (GameObject instance, PlayerProfile profile, Transform parent) {
        currentPlayerNum++;
        GameObject playerObject = GameObject.Instantiate (instance, parent.transform.position, Quaternion.identity, parent);
        // enable player avatar sync avatar data
        var player = playerObject.GetComponent<PlayerCtrl> ();
        profile.Gender = UnityEngine.Random.Range (1, 2) == 1 ? PlayerGender.Female : PlayerGender.Male; // 随机性别
        var overrideController = new AnimatorOverrideController (
            player.GetComponent<Animator> ().runtimeAnimatorController
            );

        // 生成前缀
        string newClipPrefix = "";
        if (profile.Gender == PlayerGender.Male) {
            //player.PlayerName = MaleNames [maleCount];
            maleCount++;
            newClipPrefix = $"male_{maleCount % maleCountMax + 1:D2}_";
        } else if (profile.Gender == PlayerGender.Female) {
            //player.PlayerName = FemaleNames [femaleCount];
            femaleCount++;
            newClipPrefix = $"female_{femaleCount % femaleCountMax + 1:D2}_";
        }
        foreach (var clipPrefix in clipList) {
            overrideController [clipPrefix] = Resources.Load<AnimationClip> ($"/Animations/{newClipPrefix}{clipPrefix}.anim");
        }

        Animator animator = playerObject.GetComponent<Animator> ();
        animator.runtimeAnimatorController = overrideController;
        player.Profile = profile;
        dictPlayerObjects.Add (profile.Id, playerObject);
        return playerObject;
    }

    void Start () {
        InitGame ();
    }

    /// <summary>
    /// Stop game
    /// </summary>
    void Stop () {
        IsRunning = false;
    }

    /// <summary>
    /// Game loop
    /// </summary>
    /// <returns></returns>
    IEnumerator run () {
        Debug.Log ("Begin Game Loop");
        IsRunning = true;
        DateTime startTime = DateTime.Now;
        while (IsRunning) {
            StartCoroutine(GetMsg ());
            HandleMsg ();
            TimeSpan timeDifference = DateTime.Now - startTime;
            if (timeDifference.TotalMilliseconds < GameSetting.GameLoopInterval) {
                float waitMilliseconds = GameSetting.GameLoopInterval - (float)timeDifference.TotalMilliseconds;
                yield return new WaitForSeconds (waitMilliseconds / 1000.0f);
                startTime = DateTime.Now;
            }
        }
    }

    /// <summary>
    /// Get a message
    /// </summary>
    /// <returns></returns>
    IEnumerator GetMsg()
    {
        if (isMsgHandling) {
            yield break;
        }

        // mock up 等待1秒
        //int id = UnityEngine.Random.Range (1, GameSetting.PlayerNum);
        //PlayerMessage msg = new PlayerMessage ();
        //msg.Round = id;
        //msg.PlayerName = $"Player{id}";
        //msg.PlayerId = id;
        //msg.TargetId = id - 1 > 0 ? id - 1 : id + 1;
        //msg.Type = PlayerMessageType.WolfMessage;
        //msg.Message = $"This is {msg.PlayerName} speaking, test test test 中文测试 test test test test test test test中文测试。This is {msg.PlayerName} speaking 中文测试,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking, ";
        //gameMessages.Enqueue (msg);

		UnityWebRequest request = UnityWebRequest.Get (APIUrl.getMsg);
		request.timeout = GameSetting.RequestTimeout;
		yield return request.SendWebRequest ();

		if (request.result == UnityWebRequest.Result.Success) {
			// 请求成功,处理响应数据
			
			// parse response message
			PlayerMessage []messages = JsonConvert.DeserializeObject<PlayerMessage []> (request.downloadHandler.text);
            Debug.Log ($"Recv: {messages.Length} messages");
            foreach (var msg in messages) {
                gameMessages.Enqueue (msg);
            }
            
            yield return null;
		} else {
			// 请求失败,输出错误信息
			Debug.LogError ("Error: " + request.error);
		}
    }

    void HandleMsg () {
        while (GameMessages.Count != 0) {
            if (isMsgHandling) {
                Debug.Log ("处理消息中");
                continue;
            }
            isMsgHandling = true;
            PlayerMessage msg = GameMessages.Dequeue ();

            if (IsRoundChanged (msg.Round)) {
                DayText.text = $"Day {msg.Round}";
                StartCoroutine (ShowRoundBoard (msg.Round));
                this.currentRound = msg.Round;
            }

            switch (msg.Type) {
            case PlayerMessageType.PlayerMessage:
                onPlayerMessage (msg);
                break;
            case PlayerMessageType.WolfMessage:
                onWolfMessage (msg);
                break;
            case PlayerMessageType.ProphetMessage:
                onProphetMessage (msg);
                break;
            case PlayerMessageType.Justice:
                onJustice (msg);
                break;
            case PlayerMessageType.GameConclusion:
                onGameConclusion (msg);
                break;
            }
        }
    }

    private void onGameConclusion (PlayerMessage msg) {
        Conclusion.SetActive(true);
        ConclusionCtrl conclusionCtrl = Conclusion.GetComponent<ConclusionCtrl> ();
        conclusionCtrl.SetConclusion ("sdfdsfsdfdsfsdfdsfdsfsd");
    }

    private void onJustice (PlayerMessage msg) {
        foreach (var item in dictPlayerObjects) {
            var player = item.Value.GetComponent<PlayerCtrl> ();
            if (player.State != PlayerState.Dead) {
                player.State = PlayerState.Busy;
            }
        }
    }

    private void onProphetMessage (PlayerMessage msg) {
        Debug.Log ($"{msg.PlayerName} 预言家发言");
        var player = dictPlayerObjects [msg.PlayerId].GetComponent<PlayerCtrl> ();
        roundText.text = $"Round {msg.Round} - Wolf";
        playerName.text = $"{msg.PlayerName}(Wolf)";
        playerImg.sprite = PlayerImageList [msg.PlayerId % GameSetting.PlayerNum];

        StartCoroutine (TypeText (player, playerMessage, msg.Message, 0.01f));

        // 获取被杀死的对象
        var targetPlayer = dictPlayerObjects [msg.TargetId].GetComponent<PlayerCtrl> ();
        targetPlayer.State = PlayerState.Dead;
        Debug.Log ($"Player {targetPlayer.Profile.Id} was detected");
    }

    void onWolfMessage (PlayerMessage msg) {
        Debug.Log ($"{msg.PlayerName} 狼人发言");
        var player = dictPlayerObjects [msg.PlayerId].GetComponent<PlayerCtrl> ();
        roundText.text = $"Round {msg.Round} - Wolf";
        playerName.text = $"{msg.PlayerName}(Wolf)";
        playerImg.sprite = PlayerImageList [msg.PlayerId % GameSetting.PlayerNum];

        StartCoroutine (TypeText (player, playerMessage, msg.Message, 0.01f));

        // 获取被杀死的对象
        var targetPlayer = dictPlayerObjects [msg.TargetId].GetComponent<PlayerCtrl> ();
        targetPlayer.State = PlayerState.Dead;
        Debug.Log ($"Player {targetPlayer.Profile.Id} was killed");
    }

    void onPlayerMessage (PlayerMessage msg) {
        // if the round number changed, show the animation
        // wait for the animation
        // find player by id
        // show message box, text word by word animation and prohaibit
        // wait for the animation

        var player = dictPlayerObjects [msg.PlayerId].GetComponent<PlayerCtrl> ();
        roundText.text = $"Round {msg.Round}";
        playerName.text = msg.PlayerName;
        playerImg.sprite = PlayerImageList [msg.PlayerId % GameSetting.PlayerNum];
        StartCoroutine (TypeText (player, playerMessage, msg.Message, 0.01f));
    }

    IEnumerator TypeText (PlayerCtrl playerCtrl, TMP_Text tMP_text, string str, float interval) {
        playerCtrl.State = PlayerState.Busy;
        int i = 0;
        while (i <= str.Length) {
            tMP_text.text = str.Substring (0, i++);
            yield return new WaitForSeconds (interval);
        }

        // 消息结束的等待
        yield return new WaitForSeconds (GameSetting.MessageEndWaitforSeconds);
        yield return isMsgHandling = false;
        yield return playerCtrl.State = PlayerState.Idle;
    }

    bool IsRoundChanged (int round) {
        return currentRound != round;
    }

    IEnumerator ShowRoundBoard (int roundNum) {
        Debug.Log ("播放round动画");
        var roundText = Roundboard.transform.gameObject.GetComponentInChildren<TMP_Text> ();
        roundText.text = $"Round {roundNum}";
        PlayableDirector director = Roundboard.GetComponent<PlayableDirector> ();
        Roundboard.SetActive (true);
        director.Play ();
        yield return new WaitForSeconds (3);
        Roundboard.SetActive (false);
    }

    void AppendMessageToHistory (PlayerMessage msg) {
        FileHelper.AppendTextToFile (histroyFilePath, msg.ToString ());
    }
}
