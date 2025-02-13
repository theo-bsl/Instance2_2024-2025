using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartClient : MonoBehaviour
{
    [SerializeField] private String serverIp = "127.0.0.1";
    
    public void StartGame()
    {
            SceneManager.sceneLoaded += onSceneLoaded;
            SceneManager.LoadScene(1);
       
    }

    private void onSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = serverIp;
        NetworkManager.Singleton.StartClient();
    }
}