using System.Collections.Generic;
using Player;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Leaderboard
{
    public class LeaderboardUI : NetworkBehaviour
    {
        [SerializeField] private List<TextMeshProUGUI> _leaderboardNames;
        [SerializeField] private List<TextMeshProUGUI> _leaderboardScores;
        [SerializeField] private LeaderboardManager _leaderboardManager;
        
        public override void OnNetworkSpawn()
        {
            SetInstanceRpc();
            _leaderboardManager.UpdateLeaderboard.AddListener(UpdateLeaderboardRpc);
        }

        [Rpc(SendTo.Server)]
        private void SetInstanceRpc()
        {
            _leaderboardManager = LeaderboardManager.Instance;
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void UpdateLeaderboardRpc(ulong[] leaderboardPlayersList)
        {
            Debug.Log(leaderboardPlayersList[0]);
            
            var clients = NetworkManager.Singleton.ConnectedClients;
            
            for (int i = 0; i < leaderboardPlayersList.Length; i++)
            {
                var player = clients[leaderboardPlayersList[i]].PlayerObject;
                var playerManager = TryGetComponentInChildren<PlayerManager>(player.transform);
                
                Debug.Log($"player name : {playerManager.PlayerName.Value}");
                
                _leaderboardNames[i].SetText(playerManager.PlayerName.Value + " :");
                
                _leaderboardScores[i].SetText(playerManager.Score.Value.ToString());
                
                Debug.Log($"player score : {playerManager.Score.Value.ToString()}");

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
    }
}