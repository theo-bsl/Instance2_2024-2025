using Player;
using Unity.Netcode;
using UnityEngine;

public class SetCameraOnCanva : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            this.enabled = false;
        }
        
        NetworkManager.Singleton.OnClientConnectedCallback += ManageNewPlayer;
    }

    public void ManageNewPlayer(ulong id)
    {
        var r = NetworkManager.Singleton.ConnectedClients[id].PlayerObject;
        cam = r.transform.GetComponentInChildren<Camera>();
        GetComponent<Canvas>().worldCamera = cam;
    }
}
