using UnityEngine;
using Unity.Netcode;

public class BallSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject ballPrefab;

    [ServerRpc]
    public void SpawnBallServerRpc()
    {
        Debug.Log($"IsHost: {NetworkManager.Singleton.IsHost}, IsServer: {NetworkManager.Singleton.IsServer}");
        Debug.Log(ballPrefab != null ? "Ball prefab assigned." : "Ball prefab is missing!");

        GameObject ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        ball.GetComponent<NetworkObject>().Spawn(true);
    }
}
