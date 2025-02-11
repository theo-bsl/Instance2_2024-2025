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
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = "192.168.1.226";
        NetworkManager.Singleton.StartClient();
    }
}