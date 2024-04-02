using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    int playerNum = 8;
    int playSpeed = 1;

    public int PlaySpeed { get => playSpeed; set => playSpeed = value; }
    public int PlayerNum { get => playerNum; set => playerNum = value; }


    // Start is called before the first frame update
    void Start()
    {
        GameObject prefab = Resources.Load<GameObject>("Assets/Charactors/Player/Player.prefab");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
