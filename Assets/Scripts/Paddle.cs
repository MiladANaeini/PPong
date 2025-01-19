using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float moveSpeed = 5f;



    void Update()
    {
        bool isPressingUp = Input.GetKey(KeyCode.W);
        bool isPressingDown = Input.GetKey(KeyCode.S);

        if (isPressingUp)
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
        else if (isPressingDown)
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }
    }

}