using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPlateformConnector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Application.platform == RuntimePlatform.LinuxServer)
        {

            NetworkManager.Singleton.StartServer();
        }
        else if (Application.platform == RuntimePlatform.WindowsServer)
        {
            NetworkManager.Singleton.StartServer();
        }
    }
}
