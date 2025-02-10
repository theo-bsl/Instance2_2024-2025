using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPlateformConnector : MonoBehaviour
{
    public TMP_Text txt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Application.platform == RuntimePlatform.LinuxServer)
        {

            NetworkManager.Singleton.StartServer();
            txt.text = "server";
            Debug.Log("server");
        }
        else if (Application.platform == RuntimePlatform.WindowsServer)
        {
            NetworkManager.Singleton.StartServer();
            txt.text = "server";
            Debug.Log("server");
        }

        if (NetworkManager.Singleton.IsClient)
        {
            txt.text = "client";
            Debug.Log("client");
        }
        else if (NetworkManager.Singleton.IsServer)
        {
            
        }
        else
        {
            txt.text = "couilles dans le pate";
            Debug.Log("couilles dans le pate");
        }
    }
}
