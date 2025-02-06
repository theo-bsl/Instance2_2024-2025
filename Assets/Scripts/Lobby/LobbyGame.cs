using Leaderboard;
using UnityEngine;
using Unity.Netcode;

namespace Lobby
{
    public class LobbyGame : NetworkBehaviour
    {
        [SerializeField] private LeaderboardManager _leaderboard;
        
        public override void OnNetworkSpawn()
        {
            if (IsServer)
                NetworkManager.Singleton.OnClientConnectedCallback += ManageNewPlayer;
            else
                enabled = false;
        }
        
        private void ManageNewPlayer(ulong id)
        {
            _leaderboard.AddNewPlayerID(id);
        }
    }
}