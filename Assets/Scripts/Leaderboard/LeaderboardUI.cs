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
            _leaderboardManager = LeaderboardManager.Instance;
            _leaderboardManager.UpdateLeaderboard.AddListener(UpdateLeaderboard);
        }

        
        private void UpdateLeaderboard(ulong[] leaderboardPlayersList)
        {
            int[] playersScores = new int[leaderboardPlayersList.Length];
            var clients = NetworkManager.Singleton.ConnectedClients;
            Debug.Log("leaderboard lenght : " + leaderboardPlayersList.Length);
            
            for (int i = 0; i < leaderboardPlayersList.Length; i++)
            {
                var player = clients[leaderboardPlayersList[i]].PlayerObject;
                var playerManager = TryGetComponentInChildren<PlayerManager>(player.transform);
                
                // _leaderboardScores[i].SetText(playerManager.Score.Value.ToString());
                playersScores[i] = playerManager.Score.Value;
                // _leaderboardNames[i].SetText(playerManager.PlayerName.Value + " :");
                
                Debug.Log($"player name : {playerManager.PlayerName.Value}");
                
                Debug.Log($"player score : {playerManager.Score.Value.ToString()}");

            }
            ShowLeaderboardRpc(leaderboardPlayersList, playersScores);
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void ShowLeaderboardRpc(ulong[] leaderboardPlayersList, int[] score)
        {
            for (int i = 0; i < leaderboardPlayersList.Length; i++)
            {
                _leaderboardScores[i].SetText(score[i].ToString());
                _leaderboardNames[i].SetText(leaderboardPlayersList[i].ToString() + " :");
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