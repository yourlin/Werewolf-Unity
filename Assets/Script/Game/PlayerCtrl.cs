using System;
using System.Xml;
using TMPro;
using UnityEngine;

[Serializable]
public class PlayerCtrl : MonoBehaviour
{
	Animator animator;
	[SerializeField]
	private PlayerState state;

    [SerializeField]
    private PlayerProfile profile;

	public TMP_Text nameLabel;
    public TMP_Text roleLabel;

    public PlayerState State {
		get => state; set {
			state = value;
			profile.State = value;

			animator.SetBool ("Busy", value == PlayerState.Busy);
			animator.SetBool ("Death", value == PlayerState.Dead);
			animator.SetBool ("Idle", value == PlayerState.Idle);
			animator.SetBool ("Dying", value == PlayerState.Dying);

			if (value == PlayerState.Busy)
			{
                gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.green);
            }
            else if (value == PlayerState.Idle)
            {
                gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.white);
            }
            else if (value == PlayerState.Dead)
            {
                gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.grey);
            }
            else if (value == PlayerState.Dying)
            {
                gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.red);
            }
            
        }
    }

	public PlayerProfile Profile { get => profile; set => profile = value; }

	private void Awake()
	{
		animator = this.GetComponent<Animator> ();
    }

    void Update()
    {
        Vector3 namePos = Camera.main.WorldToScreenPoint(this.transform.position);
		namePos.z = 0;
		nameLabel.transform.position = namePos + new Vector3(0.0f, 35.0f, 0.0f);
        nameLabel.text = profile.Name;
        roleLabel.transform.position = namePos + new Vector3(0.0f, 55.0f, 0.0f);
        roleLabel.text = profile.Role.ToString();
        Debug.Log(profile.Role);
        if (profile.Role == PlayerRole.Villager)
        {
            roleLabel.color = Color.gray;
        }
        else if (profile.Role == PlayerRole.Prophet)
        {
            roleLabel.color = Color.yellow;
        }
        else if (profile.Role == PlayerRole.Witch)
        {
            roleLabel.color = Color.green;
        }
        else if (profile.Role == PlayerRole.Wolf)
        {
            roleLabel.color = Color.red;
        }
    }

}
