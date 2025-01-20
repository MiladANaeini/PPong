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

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;

        if (NetworkManager.Singleton.IsServer)
        {
            AssignHostPaddleOwnership();
        }

    }
    private void OnServerStarted()
    {
        if (!isInitialized)
        {
            InitializeGame();
        }
    }
    void Update()
    {

        UpdateStateMachine();
    }
    public void InitializeGame()
    {

        if (isInitialized) return;
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
                ball.GetComponent<NetworkObject>().Spawn(true);
            }
            else
            {
                Debug.LogError("No Ball prefab");
            }
        }
    }

    private void AssignHostPaddleOwnership()
    {
        var hostPaddleObject = hostPaddle.GetComponent<NetworkObject>();
        if (hostPaddleObject.IsSpawned)
        {
            Debug.Log("Host already spawned!");
            hostPaddleObject.ChangeOwnership(NetworkManager.Singleton.LocalClientId);
        }
        else
        {
            hostPaddleObject.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        AssignClientPaddleOwnership(clientId);
    }

    private void AssignClientPaddleOwnership(ulong clientId)
    {
        var clientPaddleObject = clientPaddle.GetComponent<NetworkObject>();
        if (clientPaddleObject.IsSpawned)
        {
            Debug.Log("Client  already spawned!");
            clientPaddleObject.ChangeOwnership(clientId);
        }
        else
        {
            clientPaddleObject.SpawnWithOwnership(clientId);
        }
    }

    private void OnDestroy()
    {
        // avoid memory leaks
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        }
    }




}
