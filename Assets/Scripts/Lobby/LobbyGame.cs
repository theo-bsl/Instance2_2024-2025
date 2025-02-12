using System.Collections.Generic;
using System.Linq;
using Leaderboard;
using Player;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Lobby
{
    public class LobbyGame : NetworkBehaviour
    {
        [SerializeField] private LeaderboardManager _leaderboard;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private SpawnManager _spawnManager;
        [SerializeField] private LobbySizeManager _lobbySizeManager;
        private List<string> _playerNameOnLobby = new List<string>();
        
        private string _playerName;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += ManageNewPlayer;
                NetworkManager.Singleton.OnClientDisconnectCallback += ManageDisconnectPlayer;
            }
            else
            {
                enabled = false;
                return;
            }

            _scoreManager.OnScoreMax.AddListener(CloseLobby);
            _timeManager.OnTimerFinished.AddListener(CloseLobby);
        }

        private void ManageDisconnectPlayer(ulong obj)
        {
            _playerNameOnLobby.Remove(_playerName);
            Debug.Log($"Player Disconnected: {_playerName} line 43");
        }


        private void ManageNewPlayer(ulong id)
        {
            if (_lobbySizeManager.ManageNewPlayer(id))
            {
                var r = NetworkManager.Singleton.ConnectedClients[id].PlayerObject;
                PlayerManager player = r.GetComponentInChildren<PlayerManager>();
                _playerName = player.transform.name;
                if (_playerNameOnLobby.Contains(_playerName))
                {
                    ManageDisconnectPlayer(id);
                    Debug.Log($"Player Disconnected: {_playerName} line 57");
                    return;
                }

                _playerNameOnLobby.Add(player.transform.name);
                _leaderboard.AddNewPlayerID(id);
                _scoreManager.ManageNewPlayer(id);
                _spawnManager.ManageNewPlayer(id);
            }
        }


        private void CloseLobby()
        {
            var spawnedObjects = NetworkManager.Singleton.SpawnManager.SpawnedObjectsList.ToList();

            for (var i = spawnedObjects.Count - 1; i >= 0; i--)
            {
                spawnedObjects[i].Despawn();
            }

            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }
}