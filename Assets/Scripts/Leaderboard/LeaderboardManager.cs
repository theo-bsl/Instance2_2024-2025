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

        public override void OnNetworkSpawn()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += DisconnectClient;
        }

        private void DisconnectClient(ulong obj)
        {
            _allPlayers.RemoveAll(pm => pm == null || pm.GetComponentInParent<NetworkObject>().OwnerClientId == obj);
            LeaderBoardUpdate();
        }

        public void AddNewPlayerID(ulong playerID)
        {
            AddNewPlayerIDRpc(playerID);
        }

        [Rpc(SendTo.Server)]
        private void AddNewPlayerIDRpc(ulong playerID)
        {
            if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(playerID, out var client) || client.PlayerObject == null)
                return;

            var player = client.PlayerObject;
            PlayerManager manager = TryGetComponentInChildren<PlayerManager>(player.transform);
            
            if (manager == null || _allPlayers.Contains(manager))
                return;
            
            _allPlayers.Add(manager);
            manager.Score.OnValueChanged += (_, _) => LeaderBoardUpdate();
            manager.OnDestroyed += () => RemovePlayer(manager);

            LeaderBoardUpdate();
        }

        private void LeaderBoardUpdate()
        {
            _allPlayers.RemoveAll(pm => pm == null);
            _updateLeaderboard.Invoke(_allPlayers
                .Where(pm => pm != null && pm.GetComponentInParent<NetworkObject>() != null)
                .OrderByDescending(x => x.Score.Value)
                .Take(_nbPlayerInLeaderboard)
                .Select(pm => pm.GetComponentInParent<NetworkObject>().OwnerClientId)
                .ToArray());
        }

        private void RemovePlayer(PlayerManager manager)
        {
            if (_allPlayers.Contains(manager))
            {
                _allPlayers.Remove(manager);
                LeaderBoardUpdate();
            }
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
