using UnityEngine;
using Unity.Netcode;

public class GamesManager : MonoBehaviour
{
    public GameObject ballPrefab;

    void Start()
    {
        if (NetworkManager.Singleton.IsServer) // Only the server spawns the ball
        {
            SpawnBall();
        }
    }

    private void SpawnBall()
    {
        if (ballPrefab == null)
        {
            Debug.LogError("Ball prefab is not assigned in the GamesManager!");
            return;
        }

        // Instantiate and spawn the ball on the network
        GameObject ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        ball.GetComponent<NetworkObject>().Spawn();
        Debug.Log("Ball instantiated and spawned on the server.");
    }
}
