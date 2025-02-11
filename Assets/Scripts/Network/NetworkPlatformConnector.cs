using Unity.Netcode;
using UnityEngine;

namespace Network
{
    public class NetworkPlatformConnector : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
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
}
