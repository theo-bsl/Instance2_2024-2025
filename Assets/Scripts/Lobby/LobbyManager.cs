using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Lobby
{
    [RequireComponent(typeof(LobbyNetworkManager))]
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] private List<PlayerManager> _players = new();
        
        public List<PlayerManager> Players => _players;
    }
}
