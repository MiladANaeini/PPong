using UnityEngine;

public class BallMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    public int startingSpeed;
    public int orangeScore = 0;
    public int blueScore = 0;
    private CanvasManager canvasManager;

    void Start()
    {
        canvasManager = FindFirstObjectByType<CanvasManager>();

        bool isRight = UnityEngine.Random.value >= 0.5;

        float xVelocity = isRight ? 1f : -1f;

        float yVelocity = UnityEngine.Random.Range(-1, 1);

        rb.linearVelocity = new Vector2(xVelocity * startingSpeed, yVelocity * startingSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RightWall"))
        {
            blueScore += 1;
            canvasManager.UpdateScores(orangeScore, blueScore);
        }
        if (collision.gameObject.CompareTag("LeftWall"))
        {
            orangeScore += 1;
            canvasManager.UpdateScores(orangeScore, blueScore);
        }
    }
}
