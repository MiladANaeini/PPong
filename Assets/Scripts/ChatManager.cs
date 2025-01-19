using UnityEngine;
using TMPro;
using Unity.Netcode;

public class ChatManager : NetworkBehaviour
{
    public TMP_InputField chatInputField; 
    public TMP_Text chatDisplay;         

    private void Start()
    {
        chatInputField.onEndEdit.AddListener(HandleInputEndEdit);
    }

    private void HandleInputEndEdit(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            if (IsHost || IsServer)
            {
                AddMessageClientRpc(NetworkManager.Singleton.LocalClientId, message);
            }
            else
            {
              
                SendMessageServerRpc(message);
            }

            chatInputField.text = string.Empty; 
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendMessageServerRpc(string message, ServerRpcParams serverRpcParams = default)
    {
        
        AddMessageClientRpc(serverRpcParams.Receive.SenderClientId, message);
    }

    [ClientRpc]
    private void AddMessageClientRpc(ulong clientId, string message)
    {
        chatDisplay.text += $"Player {clientId}: {message}\n";
    }
}
