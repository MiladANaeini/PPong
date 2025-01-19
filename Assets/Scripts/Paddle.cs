using UnityEngine;
using Unity.Netcode;

public class Paddle : NetworkBehaviour
{
    public float moveSpeed = 5f;

    private void Update()
    {
        if (!IsOwner) return; // Only the paddle's owner can control it

        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = Vector2.down;
        }

        if (direction != Vector2.zero)
        {
            MovePaddleServerRpc(direction);
        }
    }

    [ServerRpc]
    private void MovePaddleServerRpc(Vector2 direction)
    {
        MovePaddle(direction);
        MovePaddleClientRpc(direction);
    }

    [ClientRpc]
    private void MovePaddleClientRpc(Vector2 direction)
    {
        if (!IsOwner) // Only non-owners update via the ClientRpc
        {
            MovePaddle(direction);
        }
    }

    private void MovePaddle(Vector2 direction)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
