using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamesManager : StateMachine
{
    public static GamesManager instance;
    public GameObject myBall;
    private bool isInitialized = false;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }

    }

    void Start()
    {
        states.Add(new PlayingState(this));
        states.Add(new PauseState(this));

    }

    void Update()
    {

            if (!isInitialized)
            {
                InitializeGame();
            }
            UpdateStateMachine();
        
    }
    private void InitializeGame()
    {
        Debug.Log("Initializing game...");
        instance.SwitchState<PlayingState>();
        SpawnBall();
        isInitialized = true;
    }

    public void SpawnBall()
    {
        if (myBall != null)
        {
            myBall = Instantiate(myBall, Vector3.zero, Quaternion.identity);
            Debug.Log("Ball instantiated successfully.");
        }
        else
        {
            Debug.LogError("Ball prefab is not assigned in the GamesManager.");
        }
    }

}
