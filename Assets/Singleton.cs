using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour
{
    public static Singleton instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += ManageDisconnectPlayer;
        }
    }

    private void ManageDisconnectPlayer(ulong obj)
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(0);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Destroy(gameObject);
    }
}