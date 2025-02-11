using Unity.Netcode;
using Player;
using UnityEngine;

namespace Lobby
{
    public class LobbySizeManager : NetworkBehaviour
    {
        [SerializeField] private int _maxPlayerNumber = 10;

        public bool ManageNewPlayer(ulong id)
        {
            var playerNumber = NetworkManager.Singleton.ConnectedClients.Count;

            if (playerNumber > _maxPlayerNumber)
            {
                Debug.Log("Diconnected client : " + id);
                NetworkManager.Singleton.DisconnectClient(id);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}