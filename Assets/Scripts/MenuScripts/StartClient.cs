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
        if (NetworkManager.Singleton == null)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log(SceneManager.GetSceneByBuildIndex(1).name);
            NetworkManager.Singleton.SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(1).name, LoadSceneMode.Single);
        }
    }

    private void onSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = "192.168.1.226";
        NetworkManager.Singleton.StartClient();
    }
}