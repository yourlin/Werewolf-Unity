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
	Animator animator;
	[SerializeField]
	private int id;
	[SerializeField]
	private string playerName;
	[SerializeField]
	private PlayerRole role;
	[SerializeField]
	private PlayerGender gender;
	[SerializeField]
	private PlayerState state;

	public int Id { get => id; set => id = value; }
	public string PlayerName { get => playerName; set => playerName = value; }
	public PlayerRole Role { get => role; set => role = value; }
	public PlayerGender Gender { get => gender; set => gender = value; }
	public PlayerState State {
		get => state; set {
			state = value;

			animator.SetBool ("Busy", value == PlayerState.Busy);
			animator.SetBool ("Death", value == PlayerState.Dead);
			animator.SetBool ("Idle", value == PlayerState.Idle);
		}
	}

	private void Awake()
	{
		animator = this.GetComponent<Animator> ();
	}
}
