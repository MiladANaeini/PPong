using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
public class GamesManager : StateMachine
{
    public static GamesManager instance;
    public GameObject hostPaddle;
    public GameObject clientPaddle;
    [SerializeField] private GameObject ballPrefab; 
    private bool isInitialized = false;

    private void Awake()
    {

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

        states.Add(new PlayingState(this));
        states.Add(new PauseState(this));
        NetworkManager.Singleton.StartHost();
        // Register to client connected callback
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

        if (NetworkManager.Singleton.IsServer)
        {
            AssignHostPaddleOwnership();
        }

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


        instance.SwitchState<PlayingState>();

        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
        {
            SpawnBallServerRpc();
        }

        isInitialized = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnBallServerRpc()
    {

        if (ballPrefab != null)
        {
            GameObject ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);

            if (ball.GetComponent<NetworkObject>() != null)
            {
                Debug.Log("Spawning ball on the network...");
                ball.GetComponent<NetworkObject>().Spawn(true);
            }
            else
            {
                Debug.LogError("Ball prefab is missing a NetworkObject component!");
            }
        }
    }

    private void AssignHostPaddleOwnership()
    {
        var hostPaddleObject = hostPaddle.GetComponent<NetworkObject>();
        if (hostPaddleObject.IsSpawned)
        {
            Debug.Log("Host Paddle is already spawned!");
            hostPaddleObject.ChangeOwnership(NetworkManager.Singleton.LocalClientId);
            Debug.Log($"Host Paddle Ownership changed to Client ID: {NetworkManager.Singleton.LocalClientId}");
        }
        else
        {
            hostPaddleObject.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
            Debug.Log($"Host Paddle Spawned and assigned to Client ID: {NetworkManager.Singleton.LocalClientId}");
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client connected: {clientId}");
        AssignClientPaddleOwnership(clientId);
    }

    private void AssignClientPaddleOwnership(ulong clientId)
    {
        var clientPaddleObject = clientPaddle.GetComponent<NetworkObject>();
        if (clientPaddleObject.IsSpawned)
        {
            Debug.Log("Client Paddle is already spawned!");
            clientPaddleObject.ChangeOwnership(clientId);
            Debug.Log($"Client Paddle Ownership changed to Client ID: {clientId}");
        }
        else
        {
            clientPaddleObject.SpawnWithOwnership(clientId);
            Debug.Log($"Client Paddle Spawned and assigned to Client ID: {clientId}");
        }
    }

    private void OnDestroy()
    {
        // Unregister from the callback to avoid memory leaks
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }




}
