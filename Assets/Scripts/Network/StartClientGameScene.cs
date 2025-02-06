using Unity.Netcode;
using UnityEngine;

public class StartClientGameScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NetworkManager.Singleton.StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
