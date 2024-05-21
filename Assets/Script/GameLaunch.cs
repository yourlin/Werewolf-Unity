using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaunch : MonoBehaviour
{
    private void Awake()
    {
        initFramework();

        checkHotUpdate();

        initGameLogic();
    }

    private void initGameLogic()
    {
        //this.gameObject.AddComponent<GameApp>();
        //GameApp.Instance.InitGame();
    }

    private void checkHotUpdate()
    {
        throw new NotImplementedException();
    }

    private void initFramework()
    {
        throw new NotImplementedException();
    }
}
