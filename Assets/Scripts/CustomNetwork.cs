using UnityEngine;
using Unity.Netcode;

public class CustomNetworkManager : MonoBehaviour
{
    public GameObject startHostButton;
    public GameObject startClientButton;
    public NetworkManager networkManager; 

    void Start()
    {
       
        if (networkManager == null)
        {
            networkManager = NetworkManager.Singleton;
        }

        if (networkManager.IsClient || networkManager.IsServer)
        {
            startHostButton.SetActive(false);
            startClientButton.SetActive(false);
        }
        else
        {
            startHostButton.SetActive(true);
            startClientButton.SetActive(true);
        }
    }

    public void StartHost()
    {
        networkManager.StartHost();
        startHostButton.SetActive(false);
        startClientButton.SetActive(false);

    }


    public void StartClient()
    {
        networkManager.StartClient();
        startHostButton.SetActive(false);
        startClientButton.SetActive(false);
    }
}
