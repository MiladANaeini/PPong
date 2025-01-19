using UnityEngine;
using Unity.Netcode;

public class BallMovement : NetworkBehaviour
{
    public Rigidbody2D rb;
    public int startingSpeed;
    private CanvasManager canvasManager;

    private NetworkVariable<int> orangeScore = new NetworkVariable<int>(0);
    private NetworkVariable<int> blueScore = new NetworkVariable<int>(0);

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            InitializeBallMovementServerRpc();
        }
        orangeScore.OnValueChanged += (oldValue, newValue) => UpdateScores();
        blueScore.OnValueChanged += (oldValue, newValue) => UpdateScores();
    }

    [ServerRpc(RequireOwnership = false)]
    private void InitializeBallMovementServerRpc()
    {
        ResetBall();
    }

    private void Start()
    {
        canvasManager = FindAnyObjectByType<CanvasManager>();
        UpdateScores();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return; 

        if (collision.gameObject.CompareTag("RightWall"))
        {
            orangeScore.Value += 1; 
            ResetBallServerRpc();  
        }
        else if (collision.gameObject.CompareTag("LeftWall"))
        {
            blueScore.Value += 1;  
            ResetBallServerRpc();  
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ResetBallServerRpc()
    {
        ResetBall();
    }
    private void ResetBall()
    {
        transform.position = Vector3.zero;

        bool isRight = UnityEngine.Random.value >= 0.5f;
        float xVelocity = isRight ? 1f : -1f;
        float yVelocity = UnityEngine.Random.Range(-1f, 1f);

        rb.linearVelocity = new Vector2(xVelocity * startingSpeed, yVelocity * startingSpeed);
    }

    private void UpdateScores()
    {
        if (canvasManager != null)
        {
            canvasManager.UpdateScores(orangeScore.Value, blueScore.Value);
        }
    }
}
