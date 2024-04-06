using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerRole {
    Villager = 1, // 村民
    Wolf = 2,     // 狼人
	Prophet = 3   // 预言家
}

public enum PlayerState {
	Idle = 1,
	Dead = 2,
	Busy = 3
}

public enum PlayerGender {
	Male = 1,
	Female = 2
}

public class Player : MonoBehaviour
{
	string playerName;
	PlayerRole role;
	PlayerGender gender;
	PlayerState state;

	public string PlayerName { get => playerName; set => playerName = value; }
	public PlayerRole Role { get => role; set => role = value; }
	public PlayerGender Gender { get => gender; set => gender = value; }
	public PlayerState State { get => state; set => state = value; }

	private void Start()
	{
		
	}

	private void FixedUpdate()
	{
		
	}
}
