using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    static int playerNum = 6;
	static int playSpeed = 1;
	static float gameLoopInterval = 1000f;
    static int requestTimeout = 1;

    public static int PlaySpeed { get => playSpeed; set => playSpeed = value; }
    public static int PlayerNum { get => playerNum; set => playerNum = value; }
	public static float GameLoopInterval { get => gameLoopInterval; set => gameLoopInterval = value; }
	public static int RequestTimeout { get => requestTimeout; set => requestTimeout = value; }
    public static float MessageEndWaitforSeconds { get; internal set; } = 1f;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log ("Init");
        //GameObject prefab = Resources.Load<GameObject>("Assets/Charactors/Player/Player.prefab");
        var players = GameObject.FindGameObjectsWithTag ("Player");

		for (int i=0; i<players.Length; i++) {
			players [i].SetActive (i < playerNum);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
