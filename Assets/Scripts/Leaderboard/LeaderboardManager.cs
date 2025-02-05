using System;
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
        [SerializeField] private List<PlayerManager> _allPlayers = new();
        private readonly UnityEvent<ulong[]> _updateLeaderboard = new();
        
        private static LeaderboardManager _leaderboardManager;
        public NetworkVariable<int> _playerId = new(0);

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
                return;
            
            Debug.Log("LeaderboardManager is server");
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
            
            _playerId.Value++;
            manager.PlayerName.Value = _playerId.Value;
            Debug.Log($"player ID : {manager.PlayerName.Value}");
            
            if (!_allPlayers.Contains(manager))
                _allPlayers.Add(manager);
            
            LeaderBoardUpdate();
            manager.Score.OnValueChanged += (_, _) => LeaderBoardUpdate();
        }

        private void LeaderBoardUpdate()
        {
            Debug.Log("LeaderboardUpdate");
            var obd = _allPlayers.OrderByDescending(x => x.Score.Value);
            var take = obd.Take(_nbPlayerInLeaderboard);
            var select = take.Select(pm => pm.GetComponentInParent<NetworkObject>().OwnerClientId);
            var array = select.ToArray();
            _updateLeaderboard.Invoke(array);
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