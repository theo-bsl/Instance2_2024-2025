using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartClient : MonoBehaviour
{
    public void StartGame()
    {
            SceneManager.sceneLoaded += onSceneLoaded;
            SceneManager.LoadScene(1);
       
    }

    private void onSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = "127.0.0.1";
        NetworkManager.Singleton.StartClient();
    }
}