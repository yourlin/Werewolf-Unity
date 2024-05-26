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
    public GameObject voteTemplate;

    public Sprite wolfVote;
    public Sprite villagerVote;
    public Sprite witchVote;
    public Sprite prophetVote;

    public PlayerState State {
		get => state; set {
			state = value;
			profile.State = value;

			animator.SetBool ("Busy", value == PlayerState.Busy);
			animator.SetBool ("Death", value == PlayerState.Dead);
			animator.SetBool ("Idle", value == PlayerState.Idle);
			animator.SetBool ("Dying", value == PlayerState.Dying);

            if (profile.State == PlayerState.Busy)
            {
                gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.green);
                gameObject.transform.Find("StateIcon_Dead").gameObject.SetActive(false);
                gameObject.transform.Find("StateIcon").gameObject.SetActive(true);
            }
            else if (profile.State == PlayerState.Idle)
            {
                gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.white);
                gameObject.transform.Find("StateIcon_Dead").gameObject.SetActive(false);
                gameObject.transform.Find("StateIcon").gameObject.SetActive(false);
            }
            else if (profile.State == PlayerState.Dead)
            {
                gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.grey);
                gameObject.transform.Find("StateIcon_Dead").gameObject.SetActive(true);
                gameObject.transform.Find("StateIcon").gameObject.SetActive(false);

                var tarrget = transform.Find("Votes");
                DestroyChildren(tarrget);
            }
            else if (profile.State == PlayerState.Dying)
            {
                gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.red);
                gameObject.transform.Find("StateIcon_Dead").gameObject.SetActive(true);
                gameObject.transform.Find("StateIcon").gameObject.SetActive(false);

                var tarrget = transform.Find("Votes");
                DestroyChildren(tarrget);
            }

        }
    }

	public PlayerProfile Profile { get => profile; set => profile = value; }


    private void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    public void AddVote(int count, PlayerRole role)
    {
        var tarrget = transform.Find("Votes");
        if (count == 0)
        {
            // DestroyChildren(tarrget);
            return;
        }
        var start = tarrget.childCount;
        for (int i = 0; i < count; i++) {
            var _postion = tarrget.position;
            _postion.x += i * 0.2f;
            GameObject voteObject = GameObject.Instantiate(voteTemplate, _postion, UnityEngine.Quaternion.identity, tarrget);
            switch (role)
            {
                case PlayerRole.Wolf:
                    voteObject.GetComponent<SpriteRenderer>().sprite = wolfVote;
                    break;
                case PlayerRole.Witch:
                    voteObject.GetComponent<SpriteRenderer>().sprite = witchVote;
                    break;
                case PlayerRole.Prophet:
                    voteObject.GetComponent<SpriteRenderer>().sprite = prophetVote;
                    break;
                case PlayerRole.Villager:
                    voteObject.GetComponent<SpriteRenderer>().sprite = villagerVote;
                    break;

            }
        }
    }

    public void CleanVote()
    {
        var tarrget = transform.Find("Votes");
        DestroyChildren(tarrget);
    }

    public void DestroyChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.SetParent(null);
            GameObject.Destroy(child.gameObject);
        }
    }

    void Update()
    {

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
            gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.green);
        }
        else if (profile.Role == PlayerRole.Wolf)
        {
            roleLabel.color = Color.red;
            gameObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.red);
        }
    }

}
