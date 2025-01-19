using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
public class GamesManager : StateMachine
{
    public static GamesManager instance;

    [SerializeField] private GameObject ballPrefab; // Assign this in the Inspector
    private bool isInitialized = false;

    private void Awake()
    {
        Debug.Log(NetworkManager.Singleton != null ? "NetworkManager is initialized." : "NetworkManager is null.");

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }

private void Start()
{
        Debug.Log("GamesManager Start");

        // Automatically start as a host for testing purposes
        if (!NetworkManager.Singleton.IsListening)
        {
            Debug.Log("Starting Host...");
            NetworkManager.Singleton.StartHost();
        }

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
        Debug.Log($"Is Server: {NetworkManager.Singleton.IsServer}, Is Host: {NetworkManager.Singleton.IsHost}");

        instance.SwitchState<PlayingState>();

        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
        {
            SpawnBallServerRpc(); // Only the server/host spawns the ball
        }

        isInitialized = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnBallServerRpc()
    {
        Debug.Log("SpawnBallServerRpc called.");

        if (ballPrefab != null)
        {
            Debug.Log("Instantiating ball...");
            GameObject ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);

            if (ball.GetComponent<NetworkObject>() != null)
            {
                Debug.Log("Spawning ball on the network...");
                ball.GetComponent<NetworkObject>().Spawn(true);
                Debug.Log("Ball spawned successfully.");
            }
            else
            {
                Debug.LogError("Ball prefab is missing a NetworkObject component!");
            }
        }
        else
        {
            Debug.LogError("Ball prefab is not assigned in the GamesManager.");
        }
    }

}
