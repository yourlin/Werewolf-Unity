using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
//using UnityEngine.Networking;
//using Unity.Plastic.Newtonsoft.Json;

public class GameLoop : MonoBehaviour {
	Queue<PlayerMessage> gameMessages= new Queue<PlayerMessage>();
	bool isRunning = false;

	public bool IsRunning { get => isRunning; set => isRunning = value; }
	public Queue<PlayerMessage> GameMessages { get => gameMessages; set => gameMessages = value; }

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
			TimeSpan timeDifference = DateTime.Now - startTime;
			if (timeDifference.TotalMilliseconds < GameSetting.GameLoopInterval) {
				float waitMilliseconds = GameSetting.GameLoopInterval - (float)timeDifference.TotalMilliseconds;
				Debug.Log ($"wait for {waitMilliseconds}ms");
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
		Debug.Log ("Request a message");


		// mock up 等待1秒
		int id = UnityEngine.Random.Range (1, 12);
		PlayerMessage msg = new PlayerMessage ();
		msg.PlayerName = $"Player {id}";
		msg.PlayerId = id;
		msg.Message = "test test test test test test test test test test test test test test test test test test test ";
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
		while (isRunning) {
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
	}
}
