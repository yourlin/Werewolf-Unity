using System;
using System.Xml;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
            
        }
    }

	public PlayerProfile Profile { get => profile; set => profile = value; }

	private void Awake()
	{
		animator = this.GetComponent<Animator> ();
    }

    void Update()
    {
        //      Vector3 namePos = Camera.main.WorldToScreenPoint(this.transform.position);
        //namePos.z = 0;
        //nameLabel.transform.position = namePos + new Vector3(0.0f, 35.0f, 0.0f);
        //      nameLabel.text = profile.Name;
        //      roleLabel.transform.position = namePos + new Vector3(0.0f, 55.0f, 0.0f);
        //      roleLabel.text = profile.Role.ToString();

        // Debug.Log(profile.Role);

        if (profile.State == PlayerState.Busy)
        {
            gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.green);
            gameObject.transform.Find("StateIcon_Dead").gameObject.SetActive(false);
        }
        else if (profile.State == PlayerState.Idle)
        {
            gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.white);
            gameObject.transform.Find("StateIcon_Dead").gameObject.SetActive(false);
        }
        else if (profile.State == PlayerState.Dead)
        {
            gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.grey);
            gameObject.transform.Find("StateIcon_Dead").gameObject.SetActive(true);
        }
        else if (profile.State == PlayerState.Dying)
        {
            gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.red);
            gameObject.transform.Find("StateIcon_Dead").gameObject.SetActive(true);
        }

        if (profile.Role == PlayerRole.Villager)
        {
            roleLabel.color = Color.gray;
            gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.white);
        }
        else if (profile.Role == PlayerRole.Prophet)
        {
            roleLabel.color = Color.yellow;
            gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.yellow);
        }
        else if (profile.Role == PlayerRole.Witch)
        {
            roleLabel.color = Color.green;
            gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.blue);
        }
        else if (profile.Role == PlayerRole.Wolf)
        {
            roleLabel.color = Color.red;
            gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.red);
        }
    }

}
