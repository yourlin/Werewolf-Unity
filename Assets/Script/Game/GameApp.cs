using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using static System.Net.Mime.MediaTypeNames;
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

    public bool IsRunning { get => isRunning; set => isRunning = value; }
	public Queue<PlayerMessage> GameMessages { get => gameMessages; set => gameMessages = value; }


	/// <summary>
	/// 初始化
	/// </summary>
	public void InitGame()
	{
		
	}

	void Start () {
		// get start
		StartCoroutine (run ());
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
		int id = UnityEngine.Random.Range (1, 12);
		PlayerMessage msg = new PlayerMessage ();
        msg.Round = id;
        msg.PlayerName = $"Player {id}";
		msg.PlayerId = id;
		msg.Type = PlayerMessageType.PlayerMessage;
		msg.Message = $"This is {msg.PlayerName} speaking, test test test test test test test test test test test test test test test test test test testThis is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking,This is {msg.PlayerName} speaking, ";
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
			Debug.Log($"GameMessages.Count = {GameMessages.Count}");
			PlayerMessage msg = GameMessages.Dequeue();
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

            isMsgHandling = false;
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
		throw new NotImplementedException();
	}

	private void onWolfMessage(PlayerMessage msg)
	{
		throw new NotImplementedException();
	}

	void onPlayerMessage (PlayerMessage msg) {
        // if the round number changed, show the animation
        // wait for the animation
        // find player by id
        // show message box, text word by word animation and prohaibit
        // wait for the animation

		roundText.text = $"Round { msg.Round}";
        playerName.text =  msg.PlayerName;
        playerImg.sprite = PlayerImageList[msg.PlayerId % GameSetting.PlayerNum];
		Debug.Log($"{msg.PlayerName} 开始说话");
        StartCoroutine(TypeText(playerMessage, msg.Message, 0.01f));
        
    }

    IEnumerator TypeText(TMP_Text tMP_text, string str, float interval)
    {
        int i = 0;
        while (i <= str.Length)
        {
            tMP_text.text = str.Substring(0, i++);
            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(GameSetting.MessageEndWaitforSeconds);
		Debug.Log("等待1s");
    }
}
