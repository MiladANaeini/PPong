using UnityEngine;
using Unity.Netcode;

public class BallMovement : NetworkBehaviour
{
    public Rigidbody2D rb;
    public int startingSpeed;
    public int orangeScore = 0;
    public int blueScore = 0;
    private CanvasManager canvasManager;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Only the server initializes the ball's movement
            InitializeBallMovementServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InitializeBallMovementServerRpc()
    {
        bool isRight = UnityEngine.Random.value >= 0.5f;

        float xVelocity = isRight ? 1f : -1f;
        float yVelocity = UnityEngine.Random.Range(-1f, 1f);

        rb.linearVelocity = new Vector2(xVelocity * startingSpeed, yVelocity * startingSpeed);
    }

    void Start()
    {
        canvasManager = FindObjectOfType<CanvasManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return; // Only the server handles scoring logic

        if (collision.gameObject.CompareTag("RightWall"))
        {
            orangeScore += 1;
            canvasManager.UpdateScores(orangeScore, blueScore);
            ResetBallServerRpc();
        }
        else if (collision.gameObject.CompareTag("LeftWall"))
        {
            blueScore += 1;
            canvasManager.UpdateScores(orangeScore, blueScore);
            ResetBallServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ResetBallServerRpc()
    {
        transform.position = Vector3.zero;

        bool isRight = UnityEngine.Random.value >= 0.5f;
        float xVelocity = isRight ? 1f : -1f;
        float yVelocity = UnityEngine.Random.Range(-1f, 1f);

        rb.linearVelocity = new Vector2(xVelocity * startingSpeed, yVelocity * startingSpeed);
    }
}
