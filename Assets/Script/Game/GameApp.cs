using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

using System;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using NUnit.Framework;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameApp : MonoBehaviour
{
    Queue<PlayerMessage> gameMessages = new Queue<PlayerMessage> ();
    [SerializeField]
    bool isMockEnding = false;
    [SerializeField]
    private bool isSkipMessageWait;
    bool isRunning = false;

    public GameObject messagePanel;
    public TMP_Text ErrorMessage;
    public TMP_Text roundText;
    public TMP_Text playerName;
    public TMP_Text playerMessage;
    public TMP_Text RuleMessage;
    public UnityEngine.UI.Image playerImg;
    public List<Sprite> PlayerImageList;
    private bool isMsgHandling = false;
    public TMP_Text DayText;
    public GameObject Roundboard;
    private int currentRound = 0;
    public GameObject Conclusion;
    public Button PlayBtn;
    

    public bool IsRunning { get => isRunning; set => isRunning = value; }
    public Queue<PlayerMessage> GameMessages { get => gameMessages; set => gameMessages = value; }
    public bool IsSkipMessageWait { get => isSkipMessageWait; set => isSkipMessageWait = value; }

    public Dictionary<int, GameObject> dictPlayerObjects = new Dictionary<int, GameObject> ();

    private List<string> clipList = new List<string> ();
    private int currentPlayerNum = 0;
    private readonly int femaleCountMax = 6;
    private readonly int maleCountMax = 6;
    public GameObject playerTemplate;
    public GameObject WorldTime;
    public TMP_Text GameStageTxt;

    private bool isEnd = false;

    private DG.Tweening.Sequence se;


    void Start()
    {
        InitGame();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void InitGame () {
        GameSetting.Init ();
        StopFlag();

        clipList.Add ("Busy");
        clipList.Add ("Death");
        clipList.Add ("Dying");
        clipList.Add ("Idle_Down");
        clipList.Add ("Idle_Left");
        clipList.Add ("Idle_Right");
        clipList.Add ("Idle_Up");


        if (OpeningCtrl.StartGameLang == 0)
        {
            RuleMessage.text = @"
**模型id**

ACTOR: anthropic.claude-3-sonnet-20240229-v1:0
REVISOR: anthropic.claude-3-sonnet-20240229-v1:0
SUMMARY: mistral.mixtral-8x7b-instruct-v0:1/meta.llama3-8b-instruct-v1:0

采用了 CoT+Relfextion+Few-Shots 模式

**游戏规则**

1.游戏分坏人和好人两大阵营, 
- 坏人阵营只有狼人,好人阵营有女巫，预言家和村民
- 阵营配置:2狼 1预言家 1女巫 4村民
- 坏人阵营:消灭所有好人,或者保证坏人数目大于好人数目
- 好人阵营:女巫和预言家要利用自己特殊能力保护村民消灭所有坏人

2.游戏分白天和夜晚两个阶段交替进行:
- 夜晚所有玩家闭眼，行动不会暴露身份
-- 夜晚狼人的行动必须统一投票淘汰一名玩家,优先淘汰有身份玩家(非村民)
-- 夜晚预言家必须查验一名玩家身份
-- 夜晚女巫必须使用一种药水, 救援玩家优先级高于淘汰玩家
-- 夜晚普通村民晚上无法行动
- 白天所有玩家睁眼, 分为讨论和投票两环节
-- 白天:讨论环节，每个玩家必须参与讨论发言
-- 白天:投票环节，每个玩家必须投票或者放弃

3.道具解释：
- 女巫只有一瓶毒药和一瓶解药
-- 毒药可以立马淘汰一名玩家
-- 解药可以让淘汰的玩家复活
            ";
        }
        else {
            RuleMessage.text = @"
**Model id**

ACTOR: anthropic.claude-3-sonnet-20240229-v1:0
REVISOR: anthropic.claude-3-sonnet-20240229-v1:0
SUMMARY: mistral.mixtral-8x7b-instruct-v0:1/meta.llama3-8b-instruct-v1:0

Adopt CoT+Relfextion+Few-Shots Method for PE

**Game Rule**

1.The game is divided into two teams: the bad guys and the good guys.
- The bad guys tream only has werewolves, while the good guys team has a witch, a prophet, and villagers.
- Team configuration: 2Wolf 1Prophet 1Witch 4Villager
- Bad guys team: Eliminate all good guys or ensure the number of bad guys is greater than the number of good guys.
- Good guys team: The witch and the prophet must use their special abilities to protect the villagers and eliminate all bad guys.

2.The game alternates between night and day phases:
- At night, all players keep their eyes closed, and actions do not reveal identities.
-- At night, the werewolves must unanimously vote to eliminate one player, prioritizing the elimination of players with roles (non-villagers).
-- At night, the prophet must verify the identity of one player.
-- At night, the witch must use one potion, with the priority of rescuing a player over eliminating a player.
-- At night, ordinary villagers cannot take any action.

3. During the day, all players open their eyes, and the phase is divided into discussion and voting segments.
-- Day: Discussion segment, each player must participate in the discussion and speak.
-- Day: Voting segment, each player must vote or abstain.

4.Items:
-The witch has only one poison vial and one antidote vial.
-- The poison vial can immediately eliminate one player.
-- The antidote vial can revive an eliminated player.
            ";
        }
    }

    IEnumerator GetOnlinePlayers () {
        using UnityWebRequest request = UnityWebRequest.Get (APIUrl.getPlayer);
        request.timeout = GameSetting.RequestTimeout;
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) {
            // 请求成功,处理响应数据
            // parse response message
            PlayerProfile[] playerProfiles =
                JsonConvert.DeserializeObject<PlayerProfile []> (
                request.downloadHandler.text,
                new PlayerProfileConverter ()
                );
            Debug.Log ($"{playerProfiles.Length} players will play the game");
            var respawn = GameObject.FindGameObjectWithTag("Respawn");
            GameSetting.PlayerNum = playerProfiles.Length;
            foreach (var playerProfile in playerProfiles) {
                yield return new WaitForSeconds(0.5f);
                SpawnPlayer (playerTemplate,
                    playerProfile,
                    respawn.transform.GetChild (currentPlayerNum)
                    );
            }

            EyesOpen(true);
        } else {
            // 请求失败,输出错误信息
            Debug.LogError ("Error: " + request.error);
        }

        // StartCoroutine(RunLoop());
    }

    public GameObject SpawnPlayer (GameObject instance, PlayerProfile profile, Transform parent) {
        currentPlayerNum++;
        GameObject playerObject = GameObject.Instantiate (instance, parent.transform.position, UnityEngine.Quaternion.identity, parent);
        // enable player avatar sync avatar data
        var player = playerObject.GetComponent<PlayerCtrl> ();

        // 生成AC前缀，载入对应的AC
        Animator animator = playerObject.GetComponent<Animator> ();
        string ac = "";
        if (profile.Gender == PlayerGender.Male) {
            ac = $"male_{profile.Id % maleCountMax + 1:D2}";
        } else if (profile.Gender == PlayerGender.Female) {
            ac = $"female_{profile.Id % femaleCountMax + 1:D2}";
        }
        animator.runtimeAnimatorController = Resources.Load<AnimatorOverrideController> (ac);
        if (currentPlayerNum < 3) {
            animator.SetInteger ("Direction", 1);
        } else if (currentPlayerNum < 6) {
            animator.SetInteger ("Direction", 0);
        } else if (currentPlayerNum < 9) {
            animator.SetInteger ("Direction", 2);
        } else {
            animator.SetInteger ("Direction", 3);
        }
        player.Profile = profile;
        if (profile.State == PlayerState.Dead) {
            animator.SetBool ("Death", true);
        }
        dictPlayerObjects.Add (profile.Id, playerObject);
        return playerObject;
    }

    void StartFlag() {
        IsRunning = true;
        isEnd = false;
    }

    void StopFlag () {
        IsRunning = false;
        isEnd = true;
    }

    IEnumerator StartGame()
    {
        StartCoroutine(ShowRoundBoard($"Game Start"));
        ErrorMessage.text = "";
        string requestUrl = APIUrl.startGameCN;
        if (OpeningCtrl.StartGameLang == 1)
        {
            requestUrl = APIUrl.startGameEN;
        }
        Debug.Log(requestUrl);
        UnityWebRequest request = UnityWebRequest.Get(requestUrl);
        request.timeout = GameSetting.RequestTimeout;
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            StartCoroutine(GetOnlinePlayers());
            yield return new WaitForSeconds(3);
            StartCoroutine(RunLoop());
        }
        else
        {
            // 请求失败,输出错误信息
            Debug.LogError("Error: " + request.error);
            ErrorMessage.text = request.error;

        }
    }

    IEnumerator FakeStartGame()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(GetOnlinePlayers());
        yield return new WaitForSeconds(3);
        StartCoroutine(RunLoop());
    }

    /// <summary>
    /// Game loop
    /// </summary>
    /// <returns></returns>
    IEnumerator RunLoop() {
        StartFlag();
        Debug.Log ($"Begin Game Loop: {IsRunning}, {isEnd}");
        DateTime startTime = DateTime.Now;
        while (IsRunning) {
            if (isEnd && GameMessages.Count == 0) {
                IsRunning = false;
                break;
            }

            if (!isEnd) {
                if (isMockEnding) {
                    StartCoroutine (GetEndingMsg ());
                } else {
                    StartCoroutine (GetMsg ());
                }
            }
            Debug.Log($"Begin Game Loop: {IsRunning}, {isEnd}");
            StartCoroutine (HandleMsg ());
            TimeSpan timeDifference = DateTime.Now - startTime;
            if (timeDifference.TotalMilliseconds < GameSetting.GameLoopInterval) {
                float waitMilliseconds = GameSetting.GameLoopInterval - (float)timeDifference.TotalMilliseconds;
                yield return new WaitForSeconds (waitMilliseconds / 1000.0f);
                startTime = DateTime.Now;
            }
            Debug.Log($"Begin Game Loop: {IsRunning}, {isEnd}");
        }

        Debug.Log($"End Game Loop: {IsRunning}, {isEnd}");
    }

    /// <summary>
    /// Get a message
    /// </summary>
    /// <returns></returns>
    IEnumerator GetMsg () {
        if (isMsgHandling) {
            yield break;
        }

        Debug.Log ("请求Msg");
        UnityWebRequest request = UnityWebRequest.Get (APIUrl.getMsg);
        request.timeout = GameSetting.RequestTimeout;
        yield return request.SendWebRequest ();

        if (request.result == UnityWebRequest.Result.Success) {
            //Debug.Log(request.downloadHandler.text);
            // 请求成功,处理响应数据
            var jsonData = JsonConvert.DeserializeObject<JObject> (request.downloadHandler.text);
            isEnd = (bool)jsonData ["end"];
            if (!isEnd) {
                Debug.Log ("游戏进行中");
            }
            Debug.Log(request.downloadHandler.text);

            List<PlayerMessage> messages = JsonConvert.DeserializeObject<List<PlayerMessage>> (
                jsonData ["messages"].ToString (),
                new PlayerMessageConverter ());
            if (messages.Count > 0)
            {
                Debug.Log($"Recv: {messages.Count} messages");
                messages.Reverse();
                foreach (var msg in messages)
                {
                    AppendMessageToHistory(msg); // 添加到历史记录里
                    gameMessages.Enqueue(msg);
                }
            }

            // parse response players
            PlayerProfile[] playerProfiles =
                JsonConvert.DeserializeObject<PlayerProfile[]>(
                jsonData["players"].ToString(),
                new PlayerProfileConverter()
                );

            foreach (var playerProfile in playerProfiles) {
                // Debug.Log($"{playerProfile.Name}, {playerProfile.State},{playerProfile.Role},");
                var targetPlayer = dictPlayerObjects[playerProfile.Id].GetComponent<PlayerCtrl>();
                if (playerProfile.State == PlayerState.Dead || playerProfile.State == PlayerState.Dying)
                {
                    targetPlayer.State = PlayerState.Dead;
                }
                else
                {

                }
            }


            yield return null;
        } else {
            // 请求失败,输出错误信息
            Debug.LogError ("Error: " + request.error);
        }
    }

    IEnumerator GetEndingMsg () {
        if (isMsgHandling || isEnd) {
            yield break;
        }

        Debug.Log ("请求Ending Msg");
        UnityWebRequest request = UnityWebRequest.Get (APIUrl.getEndingMsg);
        request.timeout = GameSetting.RequestTimeout;
        yield return request.SendWebRequest ();

        if (request.result == UnityWebRequest.Result.Success) {
            // 请求成功,处理响应数据
            var jsonData = JsonConvert.DeserializeObject<JObject> (request.downloadHandler.text);
            Debug.Log (request.downloadHandler.text);
            isEnd = (bool)jsonData ["end"];
            if (!isEnd) {
                Debug.Log ("游戏进行中");
            }
            List<PlayerMessage> messages = JsonConvert.DeserializeObject<List<PlayerMessage>> (
                jsonData ["messages"].ToString (),
                new PlayerMessageConverter ());

            if (messages.Count > 0) {
                Debug.Log ($"Recv: {messages.Count} messages");
                Debug.Log (request.downloadHandler.text);
                foreach (var msg in messages) {
                    AppendMessageToHistory (msg); // 添加到历史记录里
                    gameMessages.Enqueue (msg);
                }
            }

            yield return null;
        } else {
            // 请求失败,输出错误信息
            Debug.LogError ("Error: " + request.error);
        }
    }

    IEnumerator HandleMsg () {
        while (GameMessages.Count != 0) {
            if (isMsgHandling) {
                yield return new WaitForSeconds (1);
                continue;
            }
            isMsgHandling = true;
            PlayerMessage msg = GameMessages.Dequeue ();

            // 回合切换动画
            if (IsRoundChanged (msg.Round)) {
                DayText.text = $"Day {msg.Round}";
                StartCoroutine (ShowRoundBoard ($"Round {msg.Round}"));
                this.currentRound = msg.Round;
            }
            yield return new WaitForSeconds(2);
            switch (msg.Stage) {
            case GameStage.Assistant:
                onGameConclusion (msg);
                break;
            default:
                onPlayerMessage (msg);
                break;
            }
        }
    }

    private void onGameConclusion (PlayerMessage msg) {
        Conclusion.SetActive (true);
        ConclusionCtrl conclusionCtrl = Conclusion.GetComponent<ConclusionCtrl> ();
        conclusionCtrl.SetConclusion (msg.Message.content);
    }

    private void onJustice (PlayerMessage msg) {
        foreach (var item in dictPlayerObjects) {
            var p = item.Value.GetComponent<PlayerCtrl> ();
            if (p.State != PlayerState.Dead) {
                p.State = PlayerState.Busy;
            }
        }
        var player = dictPlayerObjects [msg.PlayerId].GetComponent<PlayerCtrl> ();
        roundText.text = $"Round {msg.Round} - Wolf";
        playerName.text = $"{msg.PlayerName}(Wolf)";
        playerImg.sprite = GetPlayerImg(player.Profile);
        StartCoroutine (TypeText (player, playerMessage, msg.Message.content, 0.01f));
        // 获取被杀死的对象
        if (msg.TargetId != 0) {
            var targetPlayer = dictPlayerObjects [msg.TargetId].GetComponent<PlayerCtrl> ();
            targetPlayer.State = PlayerState.Dead;
            Debug.Log ($"Player {targetPlayer.Profile.Id} was voted");
        }
    }

    private void onProphetMessage (PlayerMessage msg) {
        Debug.Log ($"{msg.PlayerName} 预言家发言");
        Debug.Log ($"{msg.ToString ()}");
        var player = dictPlayerObjects [msg.PlayerId].GetComponent<PlayerCtrl> ();
        roundText.text = $"Round {msg.Round} - Wolf";
        playerName.text = $"{msg.PlayerName}(Wolf)";
        playerImg.sprite = GetPlayerImg (player.Profile);

        StartCoroutine (TypeText (player, playerMessage, msg.Message.content, 0.01f));

        // 获取被探测的对象
        if (msg.TargetId != 0) {
            var targetPlayer = dictPlayerObjects [msg.TargetId].GetComponent<PlayerCtrl> ();
            targetPlayer.State = PlayerState.Dead;
            Debug.Log ($"Player {targetPlayer.Profile.Id} was detected");
        }
    }

    void onWolfMessage (PlayerMessage msg) {
        Debug.Log ($"{msg.PlayerName} 狼人发言");
        var player = dictPlayerObjects [msg.PlayerId].GetComponent<PlayerCtrl> ();
        roundText.text = $"Round {msg.Round} - Wolf";
        playerName.text = $"{msg.PlayerName}(Wolf)";
        playerImg.sprite = GetPlayerImg (player.Profile);

        StartCoroutine (TypeText (player, playerMessage, msg.Message.content, 0.01f));

        // 获取被杀死的对象
        if (msg.TargetId != 0) {
            var targetPlayer = dictPlayerObjects [msg.TargetId].GetComponent<PlayerCtrl> ();
            targetPlayer.State = PlayerState.Dead;
            Debug.Log ($"Player {targetPlayer.Profile.Id} was killed");
        }
    }

    void onPlayerMessage (PlayerMessage msg) {
        // if the round number changed, show the animation
        // wait for the animation
        // find player by id
        // show message box, text word by word animation and prohaibit
        // wait for the animation

        var player = dictPlayerObjects [msg.PlayerId].GetComponent<PlayerCtrl> ();
        roundText.text = $"Round {msg.Round}";
        playerName.text = msg.PlayerName + " (" + player.Profile.Role.ToString() +")";
        playerImg.sprite = GetPlayerImg (player.Profile);

       

        StartCoroutine (TypeText (player, playerMessage, msg.Message.content, 0.01f));
        var worldTime = WorldTime.GetComponent<WorldTime.WorldTime> ();
        GameStageTxt.text = msg.Stage.ToString();
        //worldTime =  msg.Stage;
        if (msg.CurrentTime.Split ("-") [0] == "DAY") {
            worldTime.SetDayTime ();
            EyesOpen();
        } else {
            worldTime.SetNightTime ();
            EyesClose();
        }

    }

    IEnumerator TypeText (PlayerCtrl playerCtrl, TMP_Text tMP_text, string str, float interval) {
        messagePanel.SetActive (true);
        playerCtrl.State = PlayerState.Busy;
        int i = 0;
        while (i <= str.Length) {
            tMP_text.text = str.Substring (0, i++);
            yield return new WaitForSeconds (interval);
        }

        // 消息结束的等待
        if (!IsSkipMessageWait) {
            yield return new WaitUntil (() => Input.GetMouseButtonDown (0));
            Debug.Log ("点击继续");
        } else {
            yield return new WaitForSeconds (GameSetting.MessageEndWaitforSeconds);
        }
        
        yield return isMsgHandling = false;
        yield return playerCtrl.State = PlayerState.Idle;
        messagePanel.SetActive (false);
    }

    bool IsRoundChanged (int round) {
        return currentRound != round;
    }

    IEnumerator ShowRoundBoard (string content) {
        Debug.Log ("播放round动画");
        var roundText = Roundboard.transform.gameObject.GetComponentInChildren<TMP_Text> ();
        roundText.text = content;
        PlayableDirector director = Roundboard.GetComponent<PlayableDirector> ();
        Roundboard.SetActive (true);
        director.Play();
        yield return new WaitForSeconds (3);
        Roundboard.SetActive (false);
    }

    void AppendMessageToHistory (PlayerMessage msg) {
        FileHelper.AppendTextToFile (GameSetting.HistroyFilePath, msg.ToString ());
    }

    public Sprite GetPlayerImg (PlayerProfile profile) {
        int offset = 0;
        if (profile.Gender == PlayerGender.Male) {
            offset = profile.Id % maleCountMax + 6;
        } else if (profile.Gender == PlayerGender.Female) {
            offset = profile.Id % femaleCountMax;
        }

        return PlayerImageList [offset];
    }

    public void ToggleSkip () {
        IsSkipMessageWait = !IsSkipMessageWait;
    }

    public void SetSkip (bool value) {
        IsSkipMessageWait = value;
        if (value) {
            
        }
    }

    public void EyesOpen(bool flag = false) {
        PlayBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/Icon_Look");
        PlayBtn.enabled = flag;
    }

    public void EyesClose(bool flag = false)
    {
        PlayBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>("images/Icon_DontLook");
        PlayBtn.enabled = flag;
        Debug.Log(IsRunning);
        if (!IsRunning)
        {
            StartCoroutine(StartGame());
        }
            
    }

}
