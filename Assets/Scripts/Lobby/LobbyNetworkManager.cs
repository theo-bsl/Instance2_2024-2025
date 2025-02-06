using Unity.Netcode;
using UnityEngine;

namespace Lobby
{
    public class LobbyNetworkManager : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (!IsServer)
                GetComponent<LobbyGame>().enabled = false;
        }
    }
}