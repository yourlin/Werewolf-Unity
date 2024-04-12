using System;
using UnityEngine;

[Serializable]
public class PlayerCtrl : MonoBehaviour
{
	Animator animator;
	[SerializeField]
	private PlayerState state;

    [SerializeField]
    private PlayerProfile profile;

    public PlayerState State {
		get => state; set {
			state = value;
			profile.State = value;

			animator.SetBool ("Busy", value == PlayerState.Busy);
			animator.SetBool ("Death", value == PlayerState.Dead);
			animator.SetBool ("Idle", value == PlayerState.Idle);
			animator.SetBool ("Dying", value == PlayerState.Dying);
		}
	}

	public PlayerProfile Profile { get => profile; set => profile = value; }

	private void Awake()
	{
		animator = this.GetComponent<Animator> ();
	}
}
