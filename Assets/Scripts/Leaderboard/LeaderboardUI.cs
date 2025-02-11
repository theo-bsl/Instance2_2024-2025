using System;
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
            List<string> playerNames = new();

            var clients = NetworkManager.Singleton.ConnectedClients;

            for (int i = 0; i < leaderboardPlayersList.Length; i++)
            {
                var playerManager = clients[leaderboardPlayersList[i]].PlayerObject.GetComponentInChildren<PlayerManager>();

                playersScores[i] = playerManager.Score.Value;

                // Construct player name as a string (no need for char arrays)
                string playerName = playerManager.transform.name;
                playerNames.Add(playerName);
            }

            // Maintenant, envoyer chaque nom un par un via l'RPC
            for (int i = 0; i < playersScores.Length; i++)
            {
                ShowLeaderboardRpc(playersScores[i], playerNames[i], i);
            }
        }

        [Rpc(SendTo.ClientsAndHost)]
        private void ShowLeaderboardRpc(int score, string playerName, int index)
        {
            // Afficher le score pour ce joueur à l'indice spécifié
            Debug.Log($"Player Name : {playerName}");
            _leaderboardNames[index].SetText(playerName);
            _leaderboardScores[index].SetText(score.ToString());
        }
    }
}