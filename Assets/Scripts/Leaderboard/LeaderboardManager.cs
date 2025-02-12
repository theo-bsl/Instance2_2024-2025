using System.Collections.Generic;
using System.Linq;
using Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Leaderboard
{
    [DefaultExecutionOrder(-100)]
    public class LeaderboardManager : NetworkBehaviour
    {
        [SerializeField] private int _nbPlayerInLeaderboard = 10;
        
        private readonly List<PlayerManager> _allPlayers = new();
        private readonly UnityEvent<ulong[]> _updateLeaderboard = new();
        
        private static LeaderboardManager _leaderboardManager;

        public void Awake()
        {
            if (!_leaderboardManager)
                _leaderboardManager = this;
            else
                enabled = false;
        }

        public void AddNewPlayerID(ulong playerID)
        {
            AddNewPlayerIDRpc(playerID);
        }

        [Rpc(SendTo.Server)]
        private void AddNewPlayerIDRpc(ulong playerID)
        {
            var player = NetworkManager.Singleton.ConnectedClients[playerID].PlayerObject;
            PlayerManager manager = TryGetComponentInChildren<PlayerManager>(player.transform);
            
            if (!_allPlayers.Contains(manager))
                _allPlayers.Add(manager);
            
            manager.Score.OnValueChanged += (_, _) => LeaderBoardUpdate();
            LeaderBoardUpdate();
        }

        private void LeaderBoardUpdate()
        {
            _updateLeaderboard.Invoke(_allPlayers.OrderByDescending(x => x.Score.Value).Take(_nbPlayerInLeaderboard).Select(pm => pm.GetComponentInParent<NetworkObject>().OwnerClientId).ToArray());
        }

        private T TryGetComponentInChildren<T>(Transform playerTransform)
        {
            Transform[] transforms = playerTransform.GetComponentsInChildren<Transform>();
            foreach (Transform childTransform in transforms)
            {
                if (childTransform.TryGetComponent(out T component))
                    return component;
            }

            return default;
        }
        
        public UnityEvent<ulong[]> UpdateLeaderboard => _updateLeaderboard;
        public static LeaderboardManager Instance => _leaderboardManager;
    }
}