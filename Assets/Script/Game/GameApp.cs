using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.Playables;
//using UnityEngine.Networking;
//using Unity.Plastic.Newtonsoft.Json;

public class GameApp : UnitySingleton<GameApp> {
	Queue<PlayerMessage> gameMessages= new Queue<PlayerMessage>();
	bool isRunning = false;

	public TMP_Text roundText;
    public TMP_Text playerName;
	public TMP_Text playerMessage;
    public UnityEngine.UI.Image playerImg;
	public List<Sprite> PlayerImageList;
    private bool isMsgHandling = false;
	public TMP_Text mainRound;
	public GameObject Roundboard;
	private int currentRound = 0;

	public bool IsRunning { get => isRunning; set => isRunning = value; }
	public Queue<PlayerMessage> GameMessages { get => gameMessages; set => gameMessages = value; }
	public Dictionary<int, GameObject> dictPlayerObjects= new Dictionary<int, GameObject>();

	/// <summary>
	/// 初始化
	/// </summary>
	public void InitGame()
	{
		var players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (var obj in players) {
			Player player = obj.GetComponent<Player>();
			dictPlayerObjects.Add(player.Id, obj);
			// todo 动态生成Clip？
			//player.GetComponent<Animation>().GetClip("Player_Idel_Left")
		}

		// get start
		StartCoroutine (run ());
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
			getMsg ();
            HandleMsg();
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
	void getMsg () {
		if (isMsgHandling)
		{
			return;
		}
		Debug.Log ("Request a message");

		// mock up 等待1秒
		int id = UnityEngine.Random.Range (1, GameSetting.PlayerNum);
		PlayerMessage msg = new PlayerMessage ();
        msg.Round = id;
        msg.PlayerName = $"Player{id}";
		msg.PlayerId = id;
		msg.TargetId = id - 1 > 0 ? id - 1 : id+1;
		msg.Type = PlayerMessageType.WolfMessage;
		msg.Message = $"This is {msg.PlayerName} speaking, test test test 中文测试 test test test test test test test中文测试。This is {msg.PlayerName} speaking 中文测试,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking, ";
		gameMessages.Enqueue (msg);

		/*
		using UnityWebRequest request = UnityWebRequest.Get (APIUrl.getMsg);
		request.timeout = GameSetting.RequestTimeout;
		yield return request.SendWebRequest ();

		if (request.result == UnityWebRequest.Result.Success) {
			// 请求成功,处理响应数据
			Debug.Log ("Response: " + request.downloadHandler.text);
			// parse response message
			PlayerMessage msg = JsonConvert.DeserializeObject<PlayerMessage> (request.downloadHandler.text);
			yield return gameMessages.Add (msg);
		} else {
			// 请求失败,输出错误信息
			Debug.LogError ("Error: " + request.error);
		}
		*/
	}

    void HandleMsg () {
		while (GameMessages.Count != 0) {
			if (isMsgHandling)
			{
				Debug.Log("处理消息中");
				continue;
			}
			isMsgHandling = true;
			PlayerMessage msg = GameMessages.Dequeue();
			
			if (IsRoundChanged (msg.Round)) {
				mainRound.text = $"Round {msg.Round}";
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

	private void onGameConclusion(PlayerMessage msg)
	{
		throw new NotImplementedException();
	}

	private void onJustice(PlayerMessage msg)
	{
		throw new NotImplementedException();
	}

	private void onProphetMessage(PlayerMessage msg)
	{
		Debug.Log ($"{msg.PlayerName} 预言家发言");
		var player = dictPlayerObjects [msg.PlayerId].GetComponent<Player> ();
		roundText.text = $"Round {msg.Round} - Wolf";
		playerName.text = $"{msg.PlayerName}(Wolf)";
		playerImg.sprite = PlayerImageList [msg.PlayerId % GameSetting.PlayerNum];

		StartCoroutine (TypeText (player, playerMessage, msg.Message, 0.01f));

		// 获取被杀死的对象
		var targetPlayer = dictPlayerObjects [msg.TargetId].GetComponent<Player> ();
		targetPlayer.State = PlayerState.Dead;
		Debug.Log ($"Player {targetPlayer.Id} was killed");
	}

    void onWolfMessage(PlayerMessage msg)
	{
		Debug.Log ($"{msg.PlayerName} 狼人发言");
		var player = dictPlayerObjects [msg.PlayerId].GetComponent<Player> ();
		roundText.text = $"Round {msg.Round} - Wolf";
		playerName.text = $"{msg.PlayerName}(Wolf)";
		playerImg.sprite = PlayerImageList [msg.PlayerId % GameSetting.PlayerNum];

		StartCoroutine (TypeText (player, playerMessage, msg.Message, 0.01f));

		// 获取被杀死的对象
		var targetPlayer = dictPlayerObjects [msg.TargetId].GetComponent<Player> ();
		targetPlayer.State = PlayerState.Dead;
		Debug.Log ($"Player {targetPlayer.Id} was killed");
	}

	void onPlayerMessage (PlayerMessage msg) {
		// if the round number changed, show the animation
		// wait for the animation
		// find player by id
		// show message box, text word by word animation and prohaibit
		// wait for the animation

		var player = dictPlayerObjects [msg.PlayerId].GetComponent<Player> ();
		roundText.text = $"Round { msg.Round}";
        playerName.text =  msg.PlayerName;
        playerImg.sprite = PlayerImageList[msg.PlayerId % GameSetting.PlayerNum];
        StartCoroutine(TypeText(player, playerMessage, msg.Message, 0.01f));
    }

    IEnumerator TypeText(Player player, TMP_Text tMP_text, string str, float interval)
    {
		player.State = PlayerState.Busy;
		int i = 0;
        while (i <= str.Length)
        {
            tMP_text.text = str.Substring(0, i++);
            yield return new WaitForSeconds(interval);
        }

		// 消息结束的等待
        yield return new WaitForSeconds(GameSetting.MessageEndWaitforSeconds);
		yield return isMsgHandling = false;
		yield return player.State = PlayerState.Idle;
	}

	bool IsRoundChanged(int round) {
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
}
